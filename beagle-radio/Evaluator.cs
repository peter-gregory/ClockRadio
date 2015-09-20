using System;
using System.Collections.Generic;
using System.Text;

namespace beagleradio {

	public class Evaluator {

		public const char QUOTECHAR = '"';

		public enum OperatorToken {
			UnaryMinus,
			UnaryPlus,
			Plus,
			Minus,
			Multiply,
			Divide,
			Modulus,
			LogicalOr,
			LogicalAnd,
			LogicalEqual,
			LogicalNotEqual,
			LogicalNot,
			LessThan,
			GreaterThan,
			LessThanOrEqual,
			GreaterThanOrEqual,
			OpenParen,
			CloseParen,
			FinishExpression,
			Conditional,
			EndConditional
		};

		public class MethodInfo {

			public string Name { get; set; }
			public int ArgumentCount { get; set; }
			public List<Object> Values { get; set; }
			public Object[] Args { get; set; }
			public Object ReturnValue { get; set; }

			public MethodInfo(string name, int argumentCount, List<Object> values, Object[] args) {
				Name = name;
				ArgumentCount = argumentCount;
				Values = values;
				Args = args;
			}
		};

		public class VariableInfo {
			public string Name { get; set; }
			public Object Result { get; set; }

			public VariableInfo(string name) {
				this.Name = name;
			}
		};

		public event EventHandler<MethodInfo> ExecuteMethod;
		public event EventHandler<VariableInfo> FindVariable;

		public int Offset { get; private set; }
		public string Source { get; private set; }

		private Random random;
		private int sourceLength;
		private List<OperatorToken> functions;
		private List<Object> values;

		public Evaluator(string source) {
			this.Offset = 0;
			this.Source = source;
			this.sourceLength = source.Length;
		}

		public static bool ConvertToBoolean(Object value) {
			bool result = false;
			if (value != null) {
				if (value is bool || value is Boolean) {
					result = (bool)value;
				} else if (value is int || value is Int16 || value is Int32) {
					result = (int)value != 0;
				} else if (value is long || value is Int64) {
					result = (long)value != 0;
				} else if (value is Single || value is double) {
					result = (double)value != 0.0;
				} else if (value is string || value is String) {
					bool.TryParse((string)value, out result);
				} else {
					throw new Exception("Expression parser failed to convert " + value + " (" + value.GetType().Name + ") to bool");
				}
			}
			return result;
		}

		public static int ConvertToInteger(Object value) {
			int result = 0;
			if (value != null) {
				if (value is bool || value is Boolean) {
					result = (bool)value ? 1 : 0;
				} else if (value is int || value is Int16 || value is Int32) {
					result = (int)value;
				} else if (value is long || value is Int64) {
					result = (int)(long)value;
				} else if (value is Single || value is double) {
					result = (int)(double)value;
				} else if (value is string || value is String) {
					int.TryParse((string)value, out result);
				} else {
					throw new Exception("Expression parser failed to convert " + value + " (" + value.GetType().Name + ") to int");
				}
			}
			return result;
		}

		public static long ConvertToLong(Object value) {
			long result = 0;
			if (value != null) {
				if (value is bool || value is Boolean) {
					result = (bool)value ? 1 : 0;
				} else if (value is int || value is Int16 || value is Int32) {
					result = (int)value;
				} else if (value is long || value is Int64) {
					result = (long)value;
				} else if (value is Single || value is double) {
					result = (long)(double)value;
				} else if (value is string || value is String) {
					long.TryParse((string)value, out result);
				} else {
					throw new Exception("Expression parser failed to convert " + value + " (" + value.GetType().Name + ") to long");
				}
			}
			return result;
		}

		public static double ConvertToDouble(Object value) {
			double result = 0;
			if (value != null) {
				if (value is bool || value is Boolean) {
					result = (bool)value ? 1 : 0;
				} else if (value is int || value is Int16 || value is Int32) {
					result = (int)value;
				} else if (value is long || value is Int64) {
					result = (long)value;
				} else if (value is Single || value is double) {
					result = (double)value;
				} else if (value is string || value is String) {
					double.TryParse((string)value, out result);
				} else {
					throw new Exception("Expression parser failed to convert " + value + " (" + value.GetType().Name + ") to double");
				}
			}
			return result;
		}

		public static DateTime ConvertToDateTime(Object value) {
			DateTime result = DateTime.Now;
			if (value != null) {
				if (value is DateTime) {
					result = (DateTime)value;
				} else if (value is long || value is Int64) {
					result = new DateTime((long)value);
				} else if (value is Single || value is double) {
					result = new DateTime((long)(double)value);
				} else if (value is string || value is String) {
					DateTime.TryParse((string)value, out result);
				} else {
					throw new Exception("Expression parser failed to convert " + value + " (" + value.GetType().Name + ") to DateTime");
				}
			}
			return result;
		}

		private void PushValue(Object value) {
			values.Add(value);
		}

		private Object PopValue() {
			Object result = values[values.Count - 1];
			values.RemoveAt(values.Count - 1);
			return result;
		}

		private void PushOperator(OperatorToken token) {
			functions.Add(token);
		}

		private OperatorToken PopOperator() {
			OperatorToken result = functions[functions.Count - 1];
			functions.RemoveAt(functions.Count - 1);
			return result;
		}

		private OperatorToken PeekOperator() {
			return functions[functions.Count - 1];
		}

		private Object ExecuteRandom(int argumentCount, List<Object> values) {
			if (argumentCount != 2) {
				throw new Exception("Expression parser Random function takes 2 arguments");
			}
			int right = ConvertToInteger(PopValue());
			int left = ConvertToInteger(PopValue());
			if (random == null) random = new Random();
			return ((random.Next()) % (right - left)) + left;
		}

		private void InternalLessThan(List<Object> values) {
			IComparable right = (IComparable)PopValue();
			IComparable left = (IComparable)PopValue();
			if (left == null && right == null) {
				PushValue(false);
			} else if (left == null || right == null) {
				PushValue(left == null);
			} else if (left.GetType().IsAssignableFrom(right.GetType())) {
				PushValue(left.CompareTo(right) < 0);
			} else if (left is bool) {
				PushValue(left.CompareTo(ConvertToBoolean(right)) < 0);
			} else if (left is int) {
				PushValue(left.CompareTo(ConvertToInteger(right)) < 0);
			} else if (left is long) {
				PushValue(left.CompareTo(ConvertToLong(right)) < 0);
			} else if (left is double) {
				PushValue(left.CompareTo(ConvertToDouble(right)) < 0);
			} else if (left is DateTime) {
				PushValue(left.CompareTo(ConvertToDateTime(right)) < 0);
			} else {
				PushValue(left.ToString().CompareTo(right.ToString()) < 0);
			}
		}

		private void InternalLessThanEqualTo(List<Object> values) {
			IComparable right = (IComparable)PopValue();
			IComparable left = (IComparable)PopValue();
			if (left == null && right == null) {
				PushValue(true);
			} else if (left == null || right == null) {
				PushValue(left == null);
			} else if (left.GetType().IsAssignableFrom(right.GetType())) {
				PushValue(left.CompareTo(right) <= 0);
			} else if (left is bool) {
				PushValue(left.CompareTo(ConvertToBoolean(right)) <= 0);
			} else if (left is int) {
				PushValue(left.CompareTo(ConvertToInteger(right)) <= 0);
			} else if (left is long) {
				PushValue(left.CompareTo(ConvertToLong(right)) <= 0);
			} else if (left is double) {
				PushValue(left.CompareTo(ConvertToDouble(right)) <= 0);
			} else if (left is DateTime) {
				PushValue(left.CompareTo(ConvertToDateTime(right)) <= 0);
			} else {
				PushValue(left.ToString().CompareTo(right.ToString()) <= 0);
			}
		}

		private void InternalGreaterThan(List<Object> values) {
			IComparable right = (IComparable)PopValue();
			IComparable left = (IComparable)PopValue();
			if (left == null && right == null) {
				PushValue(false);
			} else if (left == null || right == null) {
				PushValue(right == null);
			} else if (left.GetType().IsAssignableFrom(right.GetType())) {
				PushValue(left.CompareTo(right) > 0);
			} else if (left is bool) {
				PushValue(left.CompareTo(ConvertToBoolean(right)) > 0);
			} else if (left is int) {
				PushValue(left.CompareTo(ConvertToInteger(right)) > 0);
			} else if (left is long) {
				PushValue(left.CompareTo(ConvertToLong(right)) > 0);
			} else if (left is double) {
				PushValue(left.CompareTo(ConvertToDouble(right)) > 0);
			} else if (left is DateTime) {
				PushValue(left.CompareTo(ConvertToDateTime(right)) > 0);
			} else {
				PushValue(left.ToString().CompareTo(right.ToString()) > 0);
			}
		}

		private void InternalGreaterThanEqualTo(List<Object> values) {
			IComparable right = (IComparable)PopValue();
			IComparable left = (IComparable)PopValue();
			if (left == null && right == null) {
				PushValue(true);
			} else if (left == null || right == null) {
				PushValue(right == null);
			} else if (left.GetType().IsAssignableFrom(right.GetType())) {
				PushValue(left.CompareTo(right) >= 0);
			} else if (left is bool) {
				PushValue(left.CompareTo(ConvertToBoolean(right)) >= 0);
			} else if (left is int) {
				PushValue(left.CompareTo(ConvertToInteger(right)) >= 0);
			} else if (left is long) {
				PushValue(left.CompareTo(ConvertToLong(right)) >= 0);
			} else if (left is double) {
				PushValue(left.CompareTo(ConvertToDouble(right)) >= 0);
			} else if (left is DateTime) {
				PushValue(left.CompareTo(ConvertToDateTime(right)) >= 0);
			} else {
				PushValue(left.ToString().CompareTo(right.ToString()) >= 0);
			}
		}

		private void InternalLogicalEquals(List<Object> values) {
			IComparable right = (IComparable)PopValue();
			IComparable left = (IComparable)PopValue();
			if (left == null && right == null) {
				PushValue(true);
			} else if (left == null || right == null) {
				PushValue(false);
			} else if (left.GetType().IsAssignableFrom(right.GetType())) {
				PushValue(left.CompareTo(right) == 0);
			} else if (left is bool) {
				PushValue(left.CompareTo(ConvertToBoolean(right)) == 0);
			} else if (left is int) {
				PushValue(left.CompareTo(ConvertToInteger(right)) == 0);
			} else if (left is long) {
				PushValue(left.CompareTo(ConvertToLong(right)) == 0);
			} else if (left is double) {
				PushValue(left.CompareTo(ConvertToDouble(right)) == 0);
			} else if (left is DateTime) {
				PushValue(left.CompareTo(ConvertToDateTime(right)) == 0);
			} else {
				PushValue(left.ToString().CompareTo(right.ToString()) == 0);
			}
		}

		private void InternalLogicalNotEquals(List<Object> values) {
			IComparable right = (IComparable)PopValue();
			IComparable left = (IComparable)PopValue();
			if (left == null && right == null) {
				PushValue(false);
			} else if (left == null || right == null) {
				PushValue(true);
			} else if (left.GetType().IsAssignableFrom(right.GetType())) {
				PushValue(left.CompareTo(right) != 0);
			} else if (left is bool) {
				PushValue(left.CompareTo(ConvertToBoolean(right)) != 0);
			} else if (left is int) {
				PushValue(left.CompareTo(ConvertToInteger(right)) != 0);
			} else if (left is long) {
				PushValue(left.CompareTo(ConvertToLong(right)) != 0);
			} else if (left is double) {
				PushValue(left.CompareTo(ConvertToDouble(right)) != 0);
			} else if (left is DateTime) {
				PushValue(left.CompareTo(ConvertToDateTime(right)) != 0);
			} else {
				PushValue(left.ToString().CompareTo(right.ToString()) != 0);
			}
		}

		private void InternalLogicalNot(List<Object> values) {
			Object value = PopValue();
			if (value != null) value = !ConvertToBoolean(value);
			PushValue(value);
		}

		private void InternalLogicalOr(List<Object> values) {
			Object right = PopValue();
			Object left = PopValue();
			PushValue((left != null && ConvertToBoolean(left)) || (right != null && ConvertToBoolean(right)));
		}

		private void InternalLogicalAnd(List<Object> values) {
			Object right = PopValue();
			Object left = PopValue();
			PushValue((left != null && ConvertToBoolean(left)) && (right != null && ConvertToBoolean(right)));
		}

		private void InternalPlus(List<Object> values) {
			Object right = PopValue();
			Object left = PopValue();
			if (left is string || left is String || right is string || right is String) {
				PushValue(left.ToString() + right.ToString());
			} else if (left is Single || left is double || right is Single || right is double) {
				PushValue(ConvertToDouble(left) + ConvertToDouble(right));
			} else if (left is long || left is Int64 || right is long || right is Int64) {
				PushValue(ConvertToLong(left) + ConvertToLong(right));
			} else {
				PushValue(ConvertToInteger(left) + ConvertToInteger(right));
			}
		}

		private void InternalMinus(List<Object> values) {
			Object right = PopValue();
			Object left = PopValue();
			if (left is Single || left is double || right is Single || right is double) {
				PushValue(ConvertToDouble(left) - ConvertToDouble(right));
			} else if (left is long || left is Int64 || right is long || right is Int64) {
				PushValue(ConvertToLong(left) - ConvertToLong(right));
			} else {
				PushValue(ConvertToInteger(left) - ConvertToInteger(right));
			}
		}

		private void InternalUnaryMinus(List<Object> values) {
			Object value = PopValue();
			if (value is Single || value is double) {
				PushValue(-ConvertToDouble(value));
			} else if (value is long || value is Int64) {
				PushValue(-ConvertToLong(value));
			} else {
				PushValue(-ConvertToInteger(value));
			}
		}

		private void InternalMult(List<Object> values) {
			Object right = PopValue();
			Object left = PopValue();
			if (left is Single || left is double || right is Single || right is double) {
				PushValue(ConvertToDouble(left) * ConvertToDouble(right));
			} else if (left is long || left is Int64 || right is long || right is Int64) {
				PushValue(ConvertToLong(left) * ConvertToLong(right));
			} else {
				PushValue(ConvertToInteger(left) * ConvertToInteger(right));
			}
		}

		private void InternalDiv(List<Object> values) {
			Object right = PopValue();
			Object left = PopValue();
			if (left is Single || left is double || right is Single || right is double) {
				PushValue(ConvertToDouble(left) / ConvertToDouble(right));
			} else if (left is long || left is Int64 || right is long || right is Int64) {
				PushValue(ConvertToLong(left) / ConvertToLong(right));
			} else {
				PushValue(ConvertToInteger(left) / ConvertToInteger(right));
			}
		}

		private void InternalMod(List<Object> values) {
			Object right = PopValue();
			Object left = PopValue();
			if (left is Single || left is double || right is Single || right is double) {
				PushValue(ConvertToDouble(left) % ConvertToDouble(right));
			} else if (left is long || left is Int64 || right is long || right is Int64) {
				PushValue(ConvertToLong(left) % ConvertToLong(right));
			} else {
				PushValue(ConvertToInteger(left) % ConvertToInteger(right));
			}
		}

		private OperatorToken ExecuteOperation() {
			OperatorToken command = PopOperator();
			switch (command) {
				case OperatorToken.UnaryMinus:
					InternalUnaryMinus(values);
					break;
				case OperatorToken.UnaryPlus:
					// no action needed
					break;
				case OperatorToken.Plus:
					InternalPlus(values);
					break;
				case OperatorToken.Minus:
					InternalMinus(values);
					break;
				case OperatorToken.Multiply:
					InternalMult(values);
					break;
				case OperatorToken.Divide:
					InternalDiv(values);
					break;
				case OperatorToken.Modulus:
					InternalMod(values);
					break;
				case OperatorToken.LogicalOr:
					InternalLogicalOr(values);
					break;
				case OperatorToken.LogicalAnd:
					InternalLogicalAnd(values);
					break;
				case OperatorToken.LogicalEqual:
					InternalLogicalEquals(values);
					break;
				case OperatorToken.LogicalNotEqual:
					InternalLogicalNotEquals(values);
					break;
				case OperatorToken.LogicalNot:
					InternalLogicalNot(values);
					break;
				case OperatorToken.LessThan:
					InternalLessThan(values);
					break;
				case OperatorToken.GreaterThan:
					InternalGreaterThan(values);
					break;
				case OperatorToken.LessThanOrEqual:
					InternalLessThanEqualTo(values);
					break;
				case OperatorToken.GreaterThanOrEqual:
					InternalGreaterThanEqualTo(values);
					break;
				case OperatorToken.CloseParen:
					// Do nothing
					break;
				default:
					throw new Exception("Invalid token processed: " + command);
			}
			return command;
		}

		private Object ExecuteFunction(string name, int argumentCount, List<Object> values, Object[] arguments) {
			if (name.ToLower() == "rand") {
				return ExecuteRandom(argumentCount, values);
			} else {
				MethodInfo info = new MethodInfo(name, argumentCount, values, arguments);
				if (ExecuteMethod != null) {
					ExecuteMethod(this, info);
				}
				return info.ReturnValue;
			}
		}

		private int PriorityLevel(OperatorToken command) {
			switch (command) {
				case OperatorToken.OpenParen:
					return 1;
				case OperatorToken.LogicalAnd:
				case OperatorToken.LogicalOr:
				case OperatorToken.LogicalNot:
					return 2;
				case OperatorToken.LogicalEqual:
				case OperatorToken.LogicalNotEqual:
				case OperatorToken.GreaterThan:
				case OperatorToken.LessThan:
				case OperatorToken.GreaterThanOrEqual:
				case OperatorToken.LessThanOrEqual:
					return 3;
				case OperatorToken.Plus:
				case OperatorToken.Minus:
					return 4;
				case OperatorToken.Multiply:
				case OperatorToken.Divide:
				case OperatorToken.Modulus:
					return 5;
				case OperatorToken.CloseParen:
					return 6;
				default:
					return 0;
			}
		}

		private bool IsLowerPrecedence(OperatorToken command) {
			if (functions.Count == 0) return false;
			return PriorityLevel(command) < PriorityLevel(functions[0]);
		}

		private void SkipToToken(char token) {
			bool isInQuote = false;
			bool isEscape = false;
			int level = 0;
			while (Offset < sourceLength && (isInQuote || Source[Offset] != token)) {
				switch (Source[Offset]) {
					case '(': 
						if (!isInQuote) level++;
						isEscape = false;
						break;
					case ')':
						if (!isInQuote) {
							if (level == 0) throw new Exception("Expression not balanced!");
							level--;
						}
						isEscape = false;
						break;
					case QUOTECHAR:
						if (!isEscape) {
							isInQuote = !isInQuote;
						}
						isEscape = false;
						break;
					case '\\':
						if (isInQuote) isEscape = !isEscape;
						break;
				}
				Offset++;
			}
		}

		private void SkipWhitespace() {
			while (Offset < Source.Length && char.IsWhiteSpace(Source[Offset])) {
				Offset++;
			}
		}

		private void SkipPastToken(char token) {
			SkipToToken(token);
			if (Offset < sourceLength) Offset++;
		}

		private void EvaluateNumber() {
			int startIndex = Offset;
			while (Offset < sourceLength && (char.IsDigit(Source[Offset]) || Source[Offset] == '.')) {
				Offset++;
			}
			string numberValue = Source.Substring(startIndex, Offset - startIndex);
			if (numberValue.Contains(".")) {
				PushValue(double.Parse(numberValue));
			} else {
				long test = long.Parse(numberValue);
				if (test <= int.MaxValue && test >= int.MinValue) {
					PushValue((int)test);
				} else {
					PushValue(test);
				}
			}
		}

		private void EvaluateString() {

			bool isEscape = false;

			Offset++;
			StringBuilder builder = new StringBuilder();
			if (Offset < sourceLength) {
				char glyph = Source[Offset];
				while (Offset < sourceLength && (isEscape || glyph != QUOTECHAR)) {
					if (glyph == '\\') {
						isEscape = !isEscape;
						if (!isEscape) builder.Append(glyph);
					} else {
						builder.Append(glyph);
						isEscape = false;
					}
					Offset++;
					glyph = Source[Offset];
				}
			}
			if (Offset < sourceLength) Offset++;
			PushValue(builder.ToString());
		}

		private void EvaluateFunction(string name) {
			int argCount = 0;
			Offset++;
			SkipWhitespace();
			List<Object> args = new List<Object>();
			List<Object> savedValues = values;
			List<OperatorToken> savedTokens = functions;
			while (Offset < sourceLength && Source[Offset] != ')') {
				argCount++;
				args.Add(Evaluate());
				SkipWhitespace();
				if (Offset < sourceLength && Source[Offset] == ',') {
					Offset++;
				}
			}
			functions = savedTokens;
			values = savedValues;
			if (Offset < sourceLength && Source[Offset] == ')') {
				Offset++;
				MethodInfo info = new MethodInfo(name, argCount, values, args.ToArray());
				ExecuteMethod(this, info);
				PushValue(info.ReturnValue);
			}
		}

		private void EvaluateVariableOrFunction() {
			int startIndex = Offset;
			while (Offset < sourceLength && (char.IsLetterOrDigit(Source[Offset]) || Source[Offset] == '_')) {
				Offset++;
			}
			string name = Source.Substring(startIndex, Offset - startIndex);
			switch (name) {
				case "now":
					PushValue(DateTime.Now);
					break;
				case "null":
					PushValue(null);
					break;
				case "true":
					PushValue(true);
					break;
				case "false":
					PushValue(false);
					break;
				default:
					SkipWhitespace();
					if (Offset < sourceLength && Source[Offset] == '(') {
						EvaluateFunction(name);
					} else {
						VariableInfo info = new VariableInfo(name);
						FindVariable(this, info);
						PushValue(info.Result);
					}
					break;

			}

		}

		private void EvaluateOperator(ref bool isUnary, ref int parenLevel, ref bool isFinished) {
			OperatorToken token;
			switch (Source[Offset]) {
				case '?': // Conditional
					token = OperatorToken.Conditional;
					break;
				case ':': // End conditional
					token = OperatorToken.EndConditional;
					break;
				case '-': // Minus or negative
					if (isUnary) {
						token = OperatorToken.UnaryMinus;
					} else {
						token = OperatorToken.Minus;
					}
					break;
				case '+':
					if (isUnary) {
						token = OperatorToken.UnaryPlus;
					} else {
						token = OperatorToken.Plus;
					}
					break;
				case '*':
					token = OperatorToken.Multiply;
					break;
				case '/':
					token = OperatorToken.Divide;
					break;
				case '%':
					token = OperatorToken.Modulus;
					break;
				case '|': // | or ||
					token = OperatorToken.LogicalOr;
					if (Offset < sourceLength - 2 && Source[Offset + 1] == '|') Offset++;
					break;
				case '&': // & or &&
					token = OperatorToken.LogicalAnd;
					if (Offset < sourceLength - 2 && Source[Offset + 1] == '&') Offset++;
					break;
				case '=': // = or ==
					token = OperatorToken.LogicalEqual;
					if (Offset < sourceLength - 2 && Source[Offset + 1] == '=') Offset++;
					break;
				case '<': // < or <=
					token = OperatorToken.LessThan;
					if (Offset < sourceLength - 2 && Source[Offset + 1] == '=') {
						token = OperatorToken.LessThanOrEqual;
						Offset++;
					}
					break;
				case '>': // > or >=
					token = OperatorToken.GreaterThan;
					if (Offset < sourceLength - 2 && Source[Offset + 1] == '=') {
						token = OperatorToken.GreaterThanOrEqual;
						Offset++;
					}
					break;
				case '!': // ! or !=
					token = OperatorToken.LogicalNot;
					if (Offset < sourceLength - 2 && Source[Offset + 1] == '=') {
						token = OperatorToken.LogicalNotEqual;
						Offset++;
					}
					break;
				case '(': 
					token = OperatorToken.OpenParen;
					parenLevel++;
					break;
				case ',': // argument seperator - stop processing
					token = OperatorToken.FinishExpression;
					isFinished = true;
					break;
				case ')':
					token = OperatorToken.CloseParen;
					if (parenLevel == 0) {
						isFinished = true;
						break;
					}
					parenLevel--;
					while (functions.Count > 0) {
						if (ExecuteOperation() == OperatorToken.OpenParen) {
							break;
						}
					}
					break;
				default:
					throw new Exception("Evaluate operator faled at token " + Source[Offset]);

			}
			isUnary = true;
			if (isFinished) return;
			Offset++;
			while (IsLowerPrecedence(token)) {
				ExecuteOperation();
			}

			switch (token) {
				case OperatorToken.Conditional:
					bool test = ConvertToBoolean(PopValue());
					if (!test) {
						SkipPastToken(':');
					}
					break;
				case OperatorToken.EndConditional:
					SkipToToken(')');
					break;
				default:
					PushOperator(token);
					break;
			}
		}

		public Object Evaluate() {

			values = new List<object>();
			functions = new List<OperatorToken>();
			bool isUnary = true;
			bool isFinished = false;
			int parenLevel = 0;

			while (Offset < sourceLength && !isFinished) {
				char glyph = Source[Offset];
				if (char.IsWhiteSpace(glyph)) {
					Offset++;
				} else {
					if (char.IsDigit(glyph)) {
						// number constant
						EvaluateNumber();
						isUnary = false;
					} else if (glyph == QUOTECHAR) {
						// string constant
						EvaluateString();
						isUnary = false;
					} else if (char.IsLetter(glyph) || glyph == '_') {
						EvaluateVariableOrFunction();
						isUnary = false;
					} else {
						EvaluateOperator(ref isUnary, ref parenLevel, ref isFinished);
					}
				}
			}
			while (functions.Count > 0) {
				ExecuteOperation();
			}
			return values[0];
		}
	}
}


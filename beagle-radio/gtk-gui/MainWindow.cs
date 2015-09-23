
// This file has been generated by the GUI designer. Do not modify.

public partial class MainWindow
{
	private global::Gtk.Table table1;
	
	private global::Gtk.Table table3;
	
	private global::Gtk.Table table4;
	
	private global::Gtk.Table tableTabs;
	
	private global::Gtk.Notebook notebook;
	
	private global::Gtk.Notebook notebookWiFi;
	
	private global::Gtk.Table tableWiFi;
	
	private global::Gtk.DrawingArea drawWiFi;
	
	private global::Gtk.VButtonBox vbuttonbox3;
	
	private global::Gtk.Button buttonConnect;
	
	private global::Gtk.Button buttonDisconnect;
	
	private global::Gtk.Button buttonTest;
	
	private global::Gtk.Button buttonWeather;
	
	private global::Gtk.Button buttonEvents;
	
	private global::Gtk.Label label6;
	
	private global::beagleradio.SelectAccessPoint selectaccesspoint;
	
	private global::Gtk.Label label7;
	
	private global::beagleradio.SelectUserPass selectuserpass;
	
	private global::Gtk.Label label8;
	
	private global::Gtk.Label label1;
	
	private global::Gtk.Table table11;
	
	private global::Gtk.Label labelClockInfo;
	
	private global::Gtk.Table table17;
	
	private global::beagleradio.AnalogClock analogclock;
	
	private global::beagleradio.TimedImage timedimage;
	
	private global::Gtk.Label label2;
	
	private global::Gtk.Table table12;
	
	private global::Gtk.Table table15;
	
	private global::Gtk.Image ctlArtwork;
	
	private global::Gtk.DrawingArea ctlDrawingMeta;
	
	private global::Gtk.Table table16;
	
	private global::Gtk.EventBox eventLeft;
	
	private global::Gtk.Arrow arrowLeft;
	
	private global::Gtk.EventBox eventRight;
	
	private global::Gtk.Arrow arrowRight;
	
	private global::Gtk.Label label3;
	
	private global::beagleradio.WeatherView weatherview;
	
	private global::Gtk.Label label5;
	
	private global::beagleradio.Intercom intercomView;
	
	private global::Gtk.Label label4;
	
	private global::Gtk.Table table10;
	
	private global::Gtk.EventBox eventTabClock;
	
	private global::Gtk.Image imageTabClock;
	
	private global::Gtk.EventBox eventTabIntercom;
	
	private global::Gtk.Image imageTabIntercom;
	
	private global::Gtk.EventBox eventTabRadio;
	
	private global::Gtk.Image imageTabRadio;
	
	private global::Gtk.EventBox eventTabWeather;
	
	private global::Gtk.Image imageTabWeather;
	
	private global::Gtk.EventBox eventTabWiFi;
	
	private global::Gtk.Image imageTabWiFi;
	
	private global::beagleradio.Volume volume;

	protected virtual void Build ()
	{
		global::Stetic.Gui.Initialize (this);
		// Widget MainWindow
		this.WidthRequest = 480;
		this.HeightRequest = 272;
		this.Name = "MainWindow";
		this.Title = global::Mono.Unix.Catalog.GetString ("Beagle Radio");
		this.Modal = true;
		this.Resizable = false;
		this.AllowGrow = false;
		this.DefaultWidth = 480;
		this.DefaultHeight = 272;
		this.Decorated = false;
		// Container child MainWindow.Gtk.Container+ContainerChild
		this.table1 = new global::Gtk.Table (((uint)(2)), ((uint)(1)), false);
		this.table1.Name = "table1";
		this.table1.RowSpacing = ((uint)(6));
		this.table1.ColumnSpacing = ((uint)(6));
		// Container child table1.Gtk.Table+TableChild
		this.table3 = new global::Gtk.Table (((uint)(1)), ((uint)(1)), false);
		this.table3.Name = "table3";
		this.table3.RowSpacing = ((uint)(6));
		this.table3.ColumnSpacing = ((uint)(6));
		// Container child table3.Gtk.Table+TableChild
		this.table4 = new global::Gtk.Table (((uint)(1)), ((uint)(1)), false);
		this.table4.Name = "table4";
		this.table4.RowSpacing = ((uint)(6));
		this.table4.ColumnSpacing = ((uint)(6));
		// Container child table4.Gtk.Table+TableChild
		this.tableTabs = new global::Gtk.Table (((uint)(1)), ((uint)(2)), false);
		this.tableTabs.Name = "tableTabs";
		this.tableTabs.RowSpacing = ((uint)(6));
		this.tableTabs.ColumnSpacing = ((uint)(6));
		this.tableTabs.BorderWidth = ((uint)(6));
		// Container child tableTabs.Gtk.Table+TableChild
		this.notebook = new global::Gtk.Notebook ();
		this.notebook.CanFocus = true;
		this.notebook.Name = "notebook";
		this.notebook.CurrentPage = 1;
		this.notebook.TabPos = ((global::Gtk.PositionType)(1));
		this.notebook.ShowBorder = false;
		this.notebook.ShowTabs = false;
		// Container child notebook.Gtk.Notebook+NotebookChild
		this.notebookWiFi = new global::Gtk.Notebook ();
		this.notebookWiFi.CanFocus = true;
		this.notebookWiFi.Name = "notebookWiFi";
		this.notebookWiFi.CurrentPage = 0;
		this.notebookWiFi.ShowTabs = false;
		// Container child notebookWiFi.Gtk.Notebook+NotebookChild
		this.tableWiFi = new global::Gtk.Table (((uint)(1)), ((uint)(2)), false);
		this.tableWiFi.Name = "tableWiFi";
		this.tableWiFi.RowSpacing = ((uint)(6));
		this.tableWiFi.ColumnSpacing = ((uint)(6));
		this.tableWiFi.BorderWidth = ((uint)(4));
		// Container child tableWiFi.Gtk.Table+TableChild
		this.drawWiFi = new global::Gtk.DrawingArea ();
		this.drawWiFi.Events = ((global::Gdk.EventMask)(1020));
		this.drawWiFi.Name = "drawWiFi";
		this.tableWiFi.Add (this.drawWiFi);
		global::Gtk.Table.TableChild w1 = ((global::Gtk.Table.TableChild)(this.tableWiFi [this.drawWiFi]));
		w1.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child tableWiFi.Gtk.Table+TableChild
		this.vbuttonbox3 = new global::Gtk.VButtonBox ();
		this.vbuttonbox3.Name = "vbuttonbox3";
		this.vbuttonbox3.Spacing = 4;
		this.vbuttonbox3.LayoutStyle = ((global::Gtk.ButtonBoxStyle)(3));
		// Container child vbuttonbox3.Gtk.ButtonBox+ButtonBoxChild
		this.buttonConnect = new global::Gtk.Button ();
		this.buttonConnect.CanFocus = true;
		this.buttonConnect.Name = "buttonConnect";
		this.buttonConnect.UseUnderline = true;
		this.buttonConnect.Label = global::Mono.Unix.Catalog.GetString ("Connect");
		this.vbuttonbox3.Add (this.buttonConnect);
		global::Gtk.ButtonBox.ButtonBoxChild w2 = ((global::Gtk.ButtonBox.ButtonBoxChild)(this.vbuttonbox3 [this.buttonConnect]));
		w2.Expand = false;
		w2.Fill = false;
		// Container child vbuttonbox3.Gtk.ButtonBox+ButtonBoxChild
		this.buttonDisconnect = new global::Gtk.Button ();
		this.buttonDisconnect.CanFocus = true;
		this.buttonDisconnect.Name = "buttonDisconnect";
		this.buttonDisconnect.UseUnderline = true;
		this.buttonDisconnect.Label = global::Mono.Unix.Catalog.GetString ("Disconnect");
		this.vbuttonbox3.Add (this.buttonDisconnect);
		global::Gtk.ButtonBox.ButtonBoxChild w3 = ((global::Gtk.ButtonBox.ButtonBoxChild)(this.vbuttonbox3 [this.buttonDisconnect]));
		w3.Position = 1;
		w3.Expand = false;
		w3.Fill = false;
		// Container child vbuttonbox3.Gtk.ButtonBox+ButtonBoxChild
		this.buttonTest = new global::Gtk.Button ();
		this.buttonTest.CanFocus = true;
		this.buttonTest.Name = "buttonTest";
		this.buttonTest.UseUnderline = true;
		this.buttonTest.Label = global::Mono.Unix.Catalog.GetString ("Night");
		this.vbuttonbox3.Add (this.buttonTest);
		global::Gtk.ButtonBox.ButtonBoxChild w4 = ((global::Gtk.ButtonBox.ButtonBoxChild)(this.vbuttonbox3 [this.buttonTest]));
		w4.Position = 2;
		w4.Expand = false;
		w4.Fill = false;
		// Container child vbuttonbox3.Gtk.ButtonBox+ButtonBoxChild
		this.buttonWeather = new global::Gtk.Button ();
		this.buttonWeather.CanFocus = true;
		this.buttonWeather.Name = "buttonWeather";
		this.buttonWeather.UseUnderline = true;
		this.buttonWeather.Label = global::Mono.Unix.Catalog.GetString ("Weather");
		this.vbuttonbox3.Add (this.buttonWeather);
		global::Gtk.ButtonBox.ButtonBoxChild w5 = ((global::Gtk.ButtonBox.ButtonBoxChild)(this.vbuttonbox3 [this.buttonWeather]));
		w5.Position = 3;
		w5.Expand = false;
		w5.Fill = false;
		// Container child vbuttonbox3.Gtk.ButtonBox+ButtonBoxChild
		this.buttonEvents = new global::Gtk.Button ();
		this.buttonEvents.CanFocus = true;
		this.buttonEvents.Name = "buttonEvents";
		this.buttonEvents.UseUnderline = true;
		this.buttonEvents.Label = global::Mono.Unix.Catalog.GetString ("Events");
		this.vbuttonbox3.Add (this.buttonEvents);
		global::Gtk.ButtonBox.ButtonBoxChild w6 = ((global::Gtk.ButtonBox.ButtonBoxChild)(this.vbuttonbox3 [this.buttonEvents]));
		w6.Position = 4;
		w6.Expand = false;
		w6.Fill = false;
		this.tableWiFi.Add (this.vbuttonbox3);
		global::Gtk.Table.TableChild w7 = ((global::Gtk.Table.TableChild)(this.tableWiFi [this.vbuttonbox3]));
		w7.LeftAttach = ((uint)(1));
		w7.RightAttach = ((uint)(2));
		w7.XOptions = ((global::Gtk.AttachOptions)(2));
		this.notebookWiFi.Add (this.tableWiFi);
		// Notebook tab
		this.label6 = new global::Gtk.Label ();
		this.label6.Name = "label6";
		this.label6.LabelProp = global::Mono.Unix.Catalog.GetString ("page1");
		this.notebookWiFi.SetTabLabel (this.tableWiFi, this.label6);
		this.label6.ShowAll ();
		// Container child notebookWiFi.Gtk.Notebook+NotebookChild
		this.selectaccesspoint = new global::beagleradio.SelectAccessPoint ();
		this.selectaccesspoint.Events = ((global::Gdk.EventMask)(256));
		this.selectaccesspoint.Name = "selectaccesspoint";
		this.notebookWiFi.Add (this.selectaccesspoint);
		global::Gtk.Notebook.NotebookChild w9 = ((global::Gtk.Notebook.NotebookChild)(this.notebookWiFi [this.selectaccesspoint]));
		w9.Position = 1;
		// Notebook tab
		this.label7 = new global::Gtk.Label ();
		this.label7.Name = "label7";
		this.label7.LabelProp = global::Mono.Unix.Catalog.GetString ("page2");
		this.notebookWiFi.SetTabLabel (this.selectaccesspoint, this.label7);
		this.label7.ShowAll ();
		// Container child notebookWiFi.Gtk.Notebook+NotebookChild
		this.selectuserpass = new global::beagleradio.SelectUserPass ();
		this.selectuserpass.Events = ((global::Gdk.EventMask)(256));
		this.selectuserpass.Name = "selectuserpass";
		this.notebookWiFi.Add (this.selectuserpass);
		global::Gtk.Notebook.NotebookChild w10 = ((global::Gtk.Notebook.NotebookChild)(this.notebookWiFi [this.selectuserpass]));
		w10.Position = 2;
		// Notebook tab
		this.label8 = new global::Gtk.Label ();
		this.label8.Name = "label8";
		this.label8.LabelProp = global::Mono.Unix.Catalog.GetString ("page3");
		this.notebookWiFi.SetTabLabel (this.selectuserpass, this.label8);
		this.label8.ShowAll ();
		this.notebook.Add (this.notebookWiFi);
		// Notebook tab
		this.label1 = new global::Gtk.Label ();
		this.label1.HeightRequest = 32;
		this.label1.Name = "label1";
		this.label1.LabelProp = global::Mono.Unix.Catalog.GetString ("WiFi");
		this.notebook.SetTabLabel (this.notebookWiFi, this.label1);
		this.label1.ShowAll ();
		// Container child notebook.Gtk.Notebook+NotebookChild
		this.table11 = new global::Gtk.Table (((uint)(2)), ((uint)(1)), false);
		this.table11.Name = "table11";
		this.table11.RowSpacing = ((uint)(6));
		this.table11.ColumnSpacing = ((uint)(6));
		// Container child table11.Gtk.Table+TableChild
		this.labelClockInfo = new global::Gtk.Label ();
		this.labelClockInfo.Name = "labelClockInfo";
		this.labelClockInfo.LabelProp = global::Mono.Unix.Catalog.GetString ("Not connected to the internet");
		this.table11.Add (this.labelClockInfo);
		global::Gtk.Table.TableChild w12 = ((global::Gtk.Table.TableChild)(this.table11 [this.labelClockInfo]));
		w12.XOptions = ((global::Gtk.AttachOptions)(4));
		w12.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child table11.Gtk.Table+TableChild
		this.table17 = new global::Gtk.Table (((uint)(1)), ((uint)(2)), false);
		this.table17.Name = "table17";
		this.table17.RowSpacing = ((uint)(6));
		this.table17.ColumnSpacing = ((uint)(6));
		// Container child table17.Gtk.Table+TableChild
		this.analogclock = new global::beagleradio.AnalogClock ();
		this.analogclock.WidthRequest = 192;
		this.analogclock.Events = ((global::Gdk.EventMask)(256));
		this.analogclock.Name = "analogclock";
		this.analogclock.CurrentTime = new global::System.DateTime (0);
		this.analogclock.AnalogMovement = false;
		this.analogclock.Spacing = 24;
		this.analogclock.AutoUpdate = false;
		this.table17.Add (this.analogclock);
		global::Gtk.Table.TableChild w13 = ((global::Gtk.Table.TableChild)(this.table17 [this.analogclock]));
		w13.XOptions = ((global::Gtk.AttachOptions)(4));
		w13.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child table17.Gtk.Table+TableChild
		this.timedimage = new global::beagleradio.TimedImage ();
		this.timedimage.Events = ((global::Gdk.EventMask)(256));
		this.timedimage.Name = "timedimage";
		this.table17.Add (this.timedimage);
		global::Gtk.Table.TableChild w14 = ((global::Gtk.Table.TableChild)(this.table17 [this.timedimage]));
		w14.LeftAttach = ((uint)(1));
		w14.RightAttach = ((uint)(2));
		this.table11.Add (this.table17);
		global::Gtk.Table.TableChild w15 = ((global::Gtk.Table.TableChild)(this.table11 [this.table17]));
		w15.TopAttach = ((uint)(1));
		w15.BottomAttach = ((uint)(2));
		this.notebook.Add (this.table11);
		global::Gtk.Notebook.NotebookChild w16 = ((global::Gtk.Notebook.NotebookChild)(this.notebook [this.table11]));
		w16.Position = 1;
		// Notebook tab
		this.label2 = new global::Gtk.Label ();
		this.label2.HeightRequest = 32;
		this.label2.Name = "label2";
		this.label2.LabelProp = global::Mono.Unix.Catalog.GetString ("Clock");
		this.notebook.SetTabLabel (this.table11, this.label2);
		this.label2.ShowAll ();
		// Container child notebook.Gtk.Notebook+NotebookChild
		this.table12 = new global::Gtk.Table (((uint)(2)), ((uint)(1)), false);
		this.table12.Name = "table12";
		this.table12.RowSpacing = ((uint)(6));
		this.table12.ColumnSpacing = ((uint)(6));
		// Container child table12.Gtk.Table+TableChild
		this.table15 = new global::Gtk.Table (((uint)(1)), ((uint)(2)), false);
		this.table15.Name = "table15";
		this.table15.RowSpacing = ((uint)(6));
		this.table15.ColumnSpacing = ((uint)(6));
		// Container child table15.Gtk.Table+TableChild
		this.ctlArtwork = new global::Gtk.Image ();
		this.ctlArtwork.WidthRequest = 150;
		this.ctlArtwork.HeightRequest = 150;
		this.ctlArtwork.Name = "ctlArtwork";
		this.ctlArtwork.Xpad = 2;
		this.ctlArtwork.Ypad = 2;
		this.ctlArtwork.Yalign = 0F;
		this.ctlArtwork.Pixbuf = global::Gdk.Pixbuf.LoadFromResource ("beagleradio.Properties.unknown.png");
		this.table15.Add (this.ctlArtwork);
		global::Gtk.Table.TableChild w17 = ((global::Gtk.Table.TableChild)(this.table15 [this.ctlArtwork]));
		w17.XOptions = ((global::Gtk.AttachOptions)(4));
		w17.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child table15.Gtk.Table+TableChild
		this.ctlDrawingMeta = new global::Gtk.DrawingArea ();
		this.ctlDrawingMeta.Name = "ctlDrawingMeta";
		this.table15.Add (this.ctlDrawingMeta);
		global::Gtk.Table.TableChild w18 = ((global::Gtk.Table.TableChild)(this.table15 [this.ctlDrawingMeta]));
		w18.LeftAttach = ((uint)(1));
		w18.RightAttach = ((uint)(2));
		w18.YOptions = ((global::Gtk.AttachOptions)(4));
		this.table12.Add (this.table15);
		// Container child table12.Gtk.Table+TableChild
		this.table16 = new global::Gtk.Table (((uint)(1)), ((uint)(2)), false);
		this.table16.Name = "table16";
		this.table16.RowSpacing = ((uint)(6));
		this.table16.ColumnSpacing = ((uint)(6));
		// Container child table16.Gtk.Table+TableChild
		this.eventLeft = new global::Gtk.EventBox ();
		this.eventLeft.Name = "eventLeft";
		this.eventLeft.VisibleWindow = false;
		// Container child eventLeft.Gtk.Container+ContainerChild
		this.arrowLeft = new global::Gtk.Arrow (((global::Gtk.ArrowType)(2)), ((global::Gtk.ShadowType)(2)));
		this.arrowLeft.HeightRequest = 63;
		this.arrowLeft.Name = "arrowLeft";
		this.eventLeft.Add (this.arrowLeft);
		this.table16.Add (this.eventLeft);
		global::Gtk.Table.TableChild w21 = ((global::Gtk.Table.TableChild)(this.table16 [this.eventLeft]));
		w21.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child table16.Gtk.Table+TableChild
		this.eventRight = new global::Gtk.EventBox ();
		this.eventRight.Name = "eventRight";
		this.eventRight.VisibleWindow = false;
		// Container child eventRight.Gtk.Container+ContainerChild
		this.arrowRight = new global::Gtk.Arrow (((global::Gtk.ArrowType)(3)), ((global::Gtk.ShadowType)(2)));
		this.arrowRight.WidthRequest = 32;
		this.arrowRight.Name = "arrowRight";
		this.eventRight.Add (this.arrowRight);
		this.table16.Add (this.eventRight);
		global::Gtk.Table.TableChild w23 = ((global::Gtk.Table.TableChild)(this.table16 [this.eventRight]));
		w23.LeftAttach = ((uint)(1));
		w23.RightAttach = ((uint)(2));
		w23.YOptions = ((global::Gtk.AttachOptions)(4));
		this.table12.Add (this.table16);
		global::Gtk.Table.TableChild w24 = ((global::Gtk.Table.TableChild)(this.table12 [this.table16]));
		w24.TopAttach = ((uint)(1));
		w24.BottomAttach = ((uint)(2));
		w24.XOptions = ((global::Gtk.AttachOptions)(4));
		w24.YOptions = ((global::Gtk.AttachOptions)(4));
		this.notebook.Add (this.table12);
		global::Gtk.Notebook.NotebookChild w25 = ((global::Gtk.Notebook.NotebookChild)(this.notebook [this.table12]));
		w25.Position = 2;
		// Notebook tab
		this.label3 = new global::Gtk.Label ();
		this.label3.HeightRequest = 32;
		this.label3.Name = "label3";
		this.label3.LabelProp = global::Mono.Unix.Catalog.GetString ("Radio");
		this.notebook.SetTabLabel (this.table12, this.label3);
		this.label3.ShowAll ();
		// Container child notebook.Gtk.Notebook+NotebookChild
		this.weatherview = new global::beagleradio.WeatherView ();
		this.weatherview.Events = ((global::Gdk.EventMask)(256));
		this.weatherview.Name = "weatherview";
		this.weatherview.SelectedForecast = 0;
		this.notebook.Add (this.weatherview);
		global::Gtk.Notebook.NotebookChild w26 = ((global::Gtk.Notebook.NotebookChild)(this.notebook [this.weatherview]));
		w26.Position = 3;
		// Notebook tab
		this.label5 = new global::Gtk.Label ();
		this.label5.HeightRequest = 32;
		this.label5.Name = "label5";
		this.label5.LabelProp = global::Mono.Unix.Catalog.GetString ("Weather");
		this.notebook.SetTabLabel (this.weatherview, this.label5);
		this.label5.ShowAll ();
		// Container child notebook.Gtk.Notebook+NotebookChild
		this.intercomView = new global::beagleradio.Intercom ();
		this.intercomView.Events = ((global::Gdk.EventMask)(256));
		this.intercomView.Name = "intercomView";
		this.intercomView.IsBusy = false;
		this.notebook.Add (this.intercomView);
		global::Gtk.Notebook.NotebookChild w27 = ((global::Gtk.Notebook.NotebookChild)(this.notebook [this.intercomView]));
		w27.Position = 4;
		// Notebook tab
		this.label4 = new global::Gtk.Label ();
		this.label4.Name = "label4";
		this.label4.LabelProp = global::Mono.Unix.Catalog.GetString ("Intercom");
		this.notebook.SetTabLabel (this.intercomView, this.label4);
		this.label4.ShowAll ();
		this.tableTabs.Add (this.notebook);
		// Container child tableTabs.Gtk.Table+TableChild
		this.table10 = new global::Gtk.Table (((uint)(5)), ((uint)(1)), false);
		this.table10.Name = "table10";
		this.table10.RowSpacing = ((uint)(6));
		this.table10.ColumnSpacing = ((uint)(6));
		// Container child table10.Gtk.Table+TableChild
		this.eventTabClock = new global::Gtk.EventBox ();
		this.eventTabClock.Name = "eventTabClock";
		this.eventTabClock.VisibleWindow = false;
		// Container child eventTabClock.Gtk.Container+ContainerChild
		this.imageTabClock = new global::Gtk.Image ();
		this.imageTabClock.Name = "imageTabClock";
		this.imageTabClock.Pixbuf = global::Gdk.Pixbuf.LoadFromResource ("beagleradio.Properties.clock.png");
		this.eventTabClock.Add (this.imageTabClock);
		this.table10.Add (this.eventTabClock);
		global::Gtk.Table.TableChild w30 = ((global::Gtk.Table.TableChild)(this.table10 [this.eventTabClock]));
		w30.TopAttach = ((uint)(1));
		w30.BottomAttach = ((uint)(2));
		w30.XOptions = ((global::Gtk.AttachOptions)(4));
		w30.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child table10.Gtk.Table+TableChild
		this.eventTabIntercom = new global::Gtk.EventBox ();
		this.eventTabIntercom.Name = "eventTabIntercom";
		this.eventTabIntercom.VisibleWindow = false;
		// Container child eventTabIntercom.Gtk.Container+ContainerChild
		this.imageTabIntercom = new global::Gtk.Image ();
		this.imageTabIntercom.Name = "imageTabIntercom";
		this.imageTabIntercom.Pixbuf = global::Gdk.Pixbuf.LoadFromResource ("beagleradio.Properties.intercom.png");
		this.eventTabIntercom.Add (this.imageTabIntercom);
		this.table10.Add (this.eventTabIntercom);
		global::Gtk.Table.TableChild w32 = ((global::Gtk.Table.TableChild)(this.table10 [this.eventTabIntercom]));
		w32.TopAttach = ((uint)(4));
		w32.BottomAttach = ((uint)(5));
		w32.XOptions = ((global::Gtk.AttachOptions)(4));
		w32.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child table10.Gtk.Table+TableChild
		this.eventTabRadio = new global::Gtk.EventBox ();
		this.eventTabRadio.Name = "eventTabRadio";
		this.eventTabRadio.VisibleWindow = false;
		// Container child eventTabRadio.Gtk.Container+ContainerChild
		this.imageTabRadio = new global::Gtk.Image ();
		this.imageTabRadio.Name = "imageTabRadio";
		this.imageTabRadio.Pixbuf = global::Gdk.Pixbuf.LoadFromResource ("beagleradio.Properties.radio.png");
		this.eventTabRadio.Add (this.imageTabRadio);
		this.table10.Add (this.eventTabRadio);
		global::Gtk.Table.TableChild w34 = ((global::Gtk.Table.TableChild)(this.table10 [this.eventTabRadio]));
		w34.TopAttach = ((uint)(2));
		w34.BottomAttach = ((uint)(3));
		w34.XOptions = ((global::Gtk.AttachOptions)(4));
		w34.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child table10.Gtk.Table+TableChild
		this.eventTabWeather = new global::Gtk.EventBox ();
		this.eventTabWeather.Name = "eventTabWeather";
		this.eventTabWeather.VisibleWindow = false;
		// Container child eventTabWeather.Gtk.Container+ContainerChild
		this.imageTabWeather = new global::Gtk.Image ();
		this.imageTabWeather.Name = "imageTabWeather";
		this.imageTabWeather.Pixbuf = global::Gdk.Pixbuf.LoadFromResource ("beagleradio.Properties.weather.png");
		this.eventTabWeather.Add (this.imageTabWeather);
		this.table10.Add (this.eventTabWeather);
		global::Gtk.Table.TableChild w36 = ((global::Gtk.Table.TableChild)(this.table10 [this.eventTabWeather]));
		w36.TopAttach = ((uint)(3));
		w36.BottomAttach = ((uint)(4));
		w36.XOptions = ((global::Gtk.AttachOptions)(4));
		w36.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child table10.Gtk.Table+TableChild
		this.eventTabWiFi = new global::Gtk.EventBox ();
		this.eventTabWiFi.Name = "eventTabWiFi";
		this.eventTabWiFi.VisibleWindow = false;
		// Container child eventTabWiFi.Gtk.Container+ContainerChild
		this.imageTabWiFi = new global::Gtk.Image ();
		this.imageTabWiFi.Name = "imageTabWiFi";
		this.imageTabWiFi.Pixbuf = global::Gdk.Pixbuf.LoadFromResource ("beagleradio.Properties.wifi-off.png");
		this.eventTabWiFi.Add (this.imageTabWiFi);
		this.table10.Add (this.eventTabWiFi);
		global::Gtk.Table.TableChild w38 = ((global::Gtk.Table.TableChild)(this.table10 [this.eventTabWiFi]));
		w38.XOptions = ((global::Gtk.AttachOptions)(4));
		w38.YOptions = ((global::Gtk.AttachOptions)(4));
		this.tableTabs.Add (this.table10);
		global::Gtk.Table.TableChild w39 = ((global::Gtk.Table.TableChild)(this.tableTabs [this.table10]));
		w39.LeftAttach = ((uint)(1));
		w39.RightAttach = ((uint)(2));
		w39.XOptions = ((global::Gtk.AttachOptions)(2));
		w39.YOptions = ((global::Gtk.AttachOptions)(2));
		this.table4.Add (this.tableTabs);
		this.table3.Add (this.table4);
		this.table1.Add (this.table3);
		global::Gtk.Table.TableChild w42 = ((global::Gtk.Table.TableChild)(this.table1 [this.table3]));
		w42.YOptions = ((global::Gtk.AttachOptions)(7));
		// Container child table1.Gtk.Table+TableChild
		this.volume = new global::beagleradio.Volume ();
		this.volume.HeightRequest = 32;
		this.volume.Events = ((global::Gdk.EventMask)(256));
		this.volume.Name = "volume";
		this.volume.Percentage = 0;
		this.table1.Add (this.volume);
		global::Gtk.Table.TableChild w43 = ((global::Gtk.Table.TableChild)(this.table1 [this.volume]));
		w43.TopAttach = ((uint)(1));
		w43.BottomAttach = ((uint)(2));
		w43.YOptions = ((global::Gtk.AttachOptions)(2));
		this.Add (this.table1);
		if ((this.Child != null)) {
			this.Child.ShowAll ();
		}
		this.Show ();
		this.DeleteEvent += new global::Gtk.DeleteEventHandler (this.OnDeleteEvent);
		this.DestroyEvent += new global::Gtk.DestroyEventHandler (this.OnDestroyEvent);
		this.tableTabs.ExposeEvent += new global::Gtk.ExposeEventHandler (this.OnTableTabsExposeEvent);
		this.eventTabWiFi.ButtonPressEvent += new global::Gtk.ButtonPressEventHandler (this.OnEventTabWiFiButtonPressEvent);
		this.eventTabWeather.ButtonPressEvent += new global::Gtk.ButtonPressEventHandler (this.OnEventTabWeatherButtonPressEvent);
		this.eventTabRadio.ButtonPressEvent += new global::Gtk.ButtonPressEventHandler (this.OnEventTabRadioButtonPressEvent);
		this.eventTabIntercom.ButtonPressEvent += new global::Gtk.ButtonPressEventHandler (this.OnEventTabIntercomButtonPressEvent);
		this.eventTabClock.ButtonPressEvent += new global::Gtk.ButtonPressEventHandler (this.OnEventTabClockButtonPressEvent);
		this.buttonConnect.Clicked += new global::System.EventHandler (this.OnButtonConnectClicked);
		this.buttonDisconnect.Clicked += new global::System.EventHandler (this.OnButtonDisconnectClicked);
		this.buttonTest.Clicked += new global::System.EventHandler (this.OnButtonTestClicked);
		this.buttonWeather.Clicked += new global::System.EventHandler (this.OnButtonWeatherClicked);
		this.buttonEvents.Clicked += new global::System.EventHandler (this.OnButtonEventsClicked);
		this.drawWiFi.ExposeEvent += new global::Gtk.ExposeEventHandler (this.OnDrawWiFiExposeEvent);
		this.selectaccesspoint.NextPage += new global::System.EventHandler (this.OnSelectaccesspointNextPage);
		this.selectaccesspoint.PrevPage += new global::System.EventHandler (this.OnSelectaccesspointPrevPage);
		this.selectuserpass.NextPage += new global::System.EventHandler (this.OnSelectuserpassNextPage);
		this.selectuserpass.PrevPage += new global::System.EventHandler (this.OnSelectuserpassPrevPage);
		this.eventRight.ButtonPressEvent += new global::Gtk.ButtonPressEventHandler (this.OnEventRightButtonPressEvent);
		this.eventLeft.ButtonPressEvent += new global::Gtk.ButtonPressEventHandler (this.OnEventLeftButtonPressEvent);
		this.ctlDrawingMeta.ExposeEvent += new global::Gtk.ExposeEventHandler (this.OnCtlDrawingMetaExposeEvent);
	}
}

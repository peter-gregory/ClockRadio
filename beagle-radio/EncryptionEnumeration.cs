using System;

/*
					 * 0	wpa                 	WPA 1/2 (Hex [0-9/A-F])
  Req: key (Key)
---
1	wpa-peap            	WPA-PEAP
  Req: identity (Username), domain (Domain), password (Password)
---
2	wpa-psk             	WPA 1/2 (Passphrase)
  Req: apsk (Preshared Key)
---
3	wpa2-leap           	WPA2-LEAP
  Req: username (Username), password (Password)
---
4	wpa2-peap           	WPA2-PEAP
  Req: identity (Username), domain (Domain), password (Password)
---
5	wep-hex             	WEP (Hex [0-9/A-F])
  Req: key (Key)
---
6	wep-passphrase      	WEP (Passphrase)
  Req: passphrase (Passphrase)
---
7	wep-shared          	WEP Shared/Restricted
  Req: key (Key)
---
8	leap                	LEAP with WEP
  Req: username (Username), password (Password)
---
9	ttls                	TTLS with WEP
  Req: identity (Identity), password (Password), auth (Authentication)
---
10	eap                 	EAP-FAST
  Req: username (Username), password (Password)
---
11	peap                	PEAP with GTC
  Req: identity (Identity), password (Password)
---
12	peap-tkip           	PEAP with TKIP/MSCHAPV2
  Req: identity (Identity), password (Password)
---
13	eap-tls             	EAP-TLS
  Req: identity (Identity), private_key (Private Key), private_key_passwd (Private Key Password)
---
14	psu                 	PSU
  Req: identity (Identity), password (Password)

					 */

namespace beagleradio {
	public enum EncryptionEnumeration {
		wpa, //key (Key)
		wpa_peap, //Req: identity (Username), domain (Domain), password (Password)
		wpa_psk, // Req: apsk (Preshared Key)
		wpa2_leap, // Req: username (Username), password (Password)
		wpa2_peap, // Req: identity (Username), domain (Domain), password (Password)
		wep_hex, // Req: key (Key)
		wep_passphrase, // Req: passphrase (Passphrase)
		wep_shared, // Req: key (Key)
		leap , // Req: username (Username), password (Password)
		ttls, // Req: identity (Identity), password (Password), auth (Authentication)
		eap, // Req: username (Username), password (Password)
		peap, // Req: identity (Identity), password (Password)
		peap_tkip, // Req: identity (Identity), password (Password)
		eap_tls, // Req: identity (Identity), private_key (Private Key), private_key_passwd (Private Key Password)
		psu // Req: identity (Identity), password (Password)
	}
}


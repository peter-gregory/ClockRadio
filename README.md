# ClockRadio
Beagle bone based smart clock radio

Purchase the following:

Beaglebone Black Rev. C or greater
4DCAPE-43T 4.3 inch touchscreen
Powered 4 port mini hub (I used Pluggable 2.0 4 port hub with power supply)
WiFi pigtail
USB sound card (I used Plugable USB audio adapter)
Small Amplified speakers

Go to http://www.wunderground.com/weather/api
Register for a free account and put the account key in weather.txt

Install Debian 7.8 or greater

Change the default password

#passwd

Disable hdmi

#sudo nano /boot/uEnv.txt

Upgrade the image to latest repositories

#sudo apt-get update

#sudo apt-get upgrade

Set up your time zone information

#sudo dpkg-reconfigure tzdata

add debian user for access to network functions

#sudo adduser debian netdev

Configure wireless to start when system starts
by uncommenting wireless entries

#sudo nano /etc/network/interfaces
auto eth0
allow-hotplug eth0
iface eth0 inet dhcp
auto wlan0
iface wlan0 inet dhcp

Modify the ALSA config to allow the USB sound card to be the primary sound card

#sudo nano /etc/modprobe.d/alsa-base.conf
options snd-usb-audio index=0

Install mplayer
Install pulseaudio

#sudo apt-get install pulseaudio pavucontrol mplayer

Install mono

#sudo apt-get install build-essential automake checkinstall intltool git

#sudo apt-key adv --keyserver keyserver.ubuntu.com --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF

#echo "deb http://download.mono-project.com/repo/debian wheezy main" | sudo tee /etc/apt/sources.list.d/mono-xamarin.list

#sudo apt-get update

#sudo apt-get install mono-runtime

#sudo apt-get install mono-complete libtool

Install PoxketSphinx

wget http://downloads.sourceforge.net/project/cmusphinx/sphinxbase/5prealpha/sphinxbase-5prealpha.tar.gz?r=&ts=1442606775&use_mirror=iweb


#sudo apt-get install vim git-core python-dev python-pip bison
#sudo apt-get install swig libasound2-dev libportaudio-dev python-pyaudio
#sudo apt-get install libiw-dev

download
sphinxbase-5prealpha.tar.gz
pocketsphinx-5prealpha.tar.gz


#tar -xvf sphinxbase-5prealpha.tar
#tar -xvf pocketsphinx-5prealpha.tar


#cd sphinxbase-5prealpha
#./configure
#make
#sudo make install
#cd ~

#cd pocketsphinx-5prealpha
#./configure
#make
#sudo make install

#sudo nano /etc/profile
export LD_LIBRARY_PATH=/usr/local/lib
export PKG_CONFIG_PATH=/usr/local/lib/pkgconfig

#sudo apt-get install linphonec

copy the clock-radio directory to the beagle bone in directory clock-radio

#sudo apt-get install unclutter
#sudo nano /etc/xdg/lxsession/LXDE/autostart
@xset s off
@xset -dpms
@xset s noblank
sudo sh -c "echo none > /sys/class/leds/lcd4\:green\:usr0/trigger"
sudo sh -c "echo none > /sys/class/leds/beaglebone\:green\:usr0/trigger"
sudo sh -c "echo none > /sys/class/leds/beaglebone\:green\:usr1/trigger"
sudo sh -c "echo none > /sys/class/leds/beaglebone\:green\:usr2/trigger"
sudo sh -c "echo none > /sys/class/leds/beaglebone\:green\:usr3/trigger"
sudo sh -c "amixer set "Input Gain" 100; amixer set Mic 100 capture mute"
sudo sh -c "cd /home/debian/clock-radio; ./beagle-radio.exe > clock.log"


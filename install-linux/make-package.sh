#/bin/bash

#clean
sudo rm ./infinite-diagram.deb
sudo rm -R ./infinite-diagram 

#make dir structure
mkdir ./infinite-diagram
mkdir ./infinite-diagram/DEBIAN
cp ./files/control.txt ./infinite-diagram/DEBIAN/control
mkdir ./infinite-diagram/usr
mkdir ./infinite-diagram/usr/bin
cp ./files/diagram.exec ./infinite-diagram/usr/bin/diagram
mkdir ./infinite-diagram/usr/lib
mkdir ./infinite-diagram/usr/lib/infinite-diagram
mkdir ./infinite-diagram/usr/share
mkdir ./infinite-diagram/usr/share/applications
cp ./files/infinite-diagram.desktop ./infinite-diagram/usr/share/applications/infinite-diagram.desktop
mkdir ./infinite-diagram/usr/share/doc
mkdir ./infinite-diagram/usr/share/doc/infinite-diagram/
cp ./files/copyright ./infinite-diagram/usr/share/doc/infinite-diagram/copyright
mkdir ./infinite-diagram/usr/share/icons
mkdir ./infinite-diagram/usr/share/icons/gnome
mkdir ./infinite-diagram/usr/share/icons/gnome/256x256
mkdir ./infinite-diagram/usr/share/icons/gnome/256x256/mimetypes
cp ./files/application-diagram.png ./infinite-diagram/usr/share/icons/gnome/256x256/mimetypes/application-diagram.png
mkdir ./infinite-diagram/usr/share/man
mkdir ./infinite-diagram/usr/share/man/man1
mkdir ./infinite-diagram/usr/share/mime
mkdir ./infinite-diagram/usr/share/mime/packages
cp ./files/infinitediagram.xml ./infinite-diagram/usr/share/mime/packages/infinitediagram.xml

sudo cp ./files/diagram.1 ./infinite-diagram/usr/share/man/man1/
sudo gzip -9 ./infinite-diagram/usr/share/man/man1/diagram.1

sudo cp ./files/changelog ./infinite-diagram/usr/share/doc/infinite-diagram/
sudo gzip -9 ./infinite-diagram/usr/share/doc/infinite-diagram/changelog


#sudo cp ../Diagram.SRC/Diagram/bin/Release/Ciloci.Flee.dll ./infinite-diagram/usr/lib/infinite-diagram/
sudo cp ../Diagram.SRC/Diagram/bin/Release/Diagram.exe ./infinite-diagram/usr/lib/infinite-diagram/
#sudo cp ../Diagram.SRC/Diagram/bin/Release/Diagram.exe.config ./infinite-diagram/usr/lib/infinite-diagram/
#sudo cp ../Diagram.SRC/Diagram/bin/Release/Diagram.exe.mdb ./infinite-diagram/usr/lib/infinite-diagram/
sudo cp ../Diagram.SRC/Diagram/bin/Release/IronPython.dll ./infinite-diagram/usr/lib/infinite-diagram/
sudo cp ../Diagram.SRC/Diagram/bin/Release/IronPython.Modules.dll ./infinite-diagram/usr/lib/infinite-diagram/
sudo cp ../Diagram.SRC/Diagram/bin/Release/IronPython.SQLite.dll ./infinite-diagram/usr/lib/infinite-diagram/
sudo cp ../Diagram.SRC/Diagram/bin/Release/IronPython.Wpf.dll ./infinite-diagram/usr/lib/infinite-diagram/
sudo cp ../Diagram.SRC/Diagram/bin/Release/Microsoft.Dynamic.dll ./infinite-diagram/usr/lib/infinite-diagram/
sudo cp ../Diagram.SRC/Diagram/bin/Release/Microsoft.Scripting.AspNet.dll ./infinite-diagram/usr/lib/infinite-diagram/
sudo cp ../Diagram.SRC/Diagram/bin/Release/Microsoft.Scripting.dll ./infinite-diagram/usr/lib/infinite-diagram/
sudo cp ../Diagram.SRC/Diagram/bin/Release/Microsoft.Scripting.Metadata.dll ./infinite-diagram/usr/lib/infinite-diagram/
sudo cp ../Diagram.SRC/Diagram/bin/Release/Newtonsoft.Json.dll ./infinite-diagram/usr/lib/infinite-diagram/
sudo cp ../Diagram.SRC/Diagram/bin/Release/NCalc.dll ./infinite-diagram/usr/lib/infinite-diagram/


#permissions
sudo chown -R root:root ./infinite-diagram
sudo find ./infinite-diagram -type d -exec chmod 755 {} \;
sudo find ./infinite-diagram -type f -exec chmod 644 {} \;
sudo chmod 755 ./infinite-diagram/usr/bin/diagram
sudo chmod 755 ./infinite-diagram/usr/lib/infinite-diagram/Diagram.exe

#build
sudo dpkg-deb --build infinite-diagram
lintian infinite-diagram.deb

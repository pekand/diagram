#/bin/bash

mkdir ./turbo-diagram
mkdir ./turbo-diagram/DEBIAN
cp ./files/control.txt ./turbo-diagram/DEBIAN/control
mkdir ./turbo-diagram/usr
mkdir ./turbo-diagram/usr/bin
cp ./files/diagram.exec ./turbo-diagram/usr/bin/diagram
mkdir ./turbo-diagram/usr/lib
mkdir ./turbo-diagram/usr/lib/turbo-diagram
mkdir ./turbo-diagram/usr/share
mkdir ./turbo-diagram/usr/share/applications
cp ./files/turbo-diagram.desktop ./turbo-diagram/usr/share/applications/turbo-diagram.desktop
mkdir ./turbo-diagram/usr/share/doc
mkdir ./turbo-diagram/usr/share/doc/turbo-diagram/
cp ./files/copyright ./turbo-diagram/usr/share/doc/turbo-diagram/copyright
mkdir ./turbo-diagram/usr/share/icons
mkdir ./turbo-diagram/usr/share/icons/gnome
mkdir ./turbo-diagram/usr/share/icons/gnome/256x256
mkdir ./turbo-diagram/usr/share/icons/gnome/256x256/mimetypes
cp ./files/application-diagram.png ./turbo-diagram/usr/share/icons/gnome/256x256/mimetypes/application-diagram.png
mkdir ./turbo-diagram/usr/share/man
mkdir ./turbo-diagram/usr/share/man/man1
mkdir ./turbo-diagram/usr/share/mime
mkdir ./turbo-diagram/usr/share/mime/packages
mkdir ./turbo-diagram/usr/share/mime/packages
cp ./files/turbodiagram.xml ./turbo-diagram/usr/share/mime/packages/turbodiagram.xml



#sudo cp ../Diagram.SRC/Diagram/bin/Release/Ciloci.Flee.dll ./turbo-diagram/usr/lib/turbo-diagram/
sudo cp ../Diagram.SRC/Diagram/bin/Release/Diagram.exe ./turbo-diagram/usr/lib/turbo-diagram/
#sudo cp ../Diagram.SRC/Diagram/bin/Release/Diagram.exe.config ./turbo-diagram/usr/lib/turbo-diagram/
#sudo cp ../Diagram.SRC/Diagram/bin/Release/Diagram.exe.mdb ./turbo-diagram/usr/lib/turbo-diagram/
sudo cp ../Diagram.SRC/Diagram/bin/Release/IronPython.dll ./turbo-diagram/usr/lib/turbo-diagram/
sudo cp ../Diagram.SRC/Diagram/bin/Release/IronPython.Modules.dll ./turbo-diagram/usr/lib/turbo-diagram/
sudo cp ../Diagram.SRC/Diagram/bin/Release/IronPython.SQLite.dll ./turbo-diagram/usr/lib/turbo-diagram/
sudo cp ../Diagram.SRC/Diagram/bin/Release/IronPython.Wpf.dll ./turbo-diagram/usr/lib/turbo-diagram/
sudo cp ../Diagram.SRC/Diagram/bin/Release/Microsoft.Dynamic.dll ./turbo-diagram/usr/lib/turbo-diagram/
sudo cp ../Diagram.SRC/Diagram/bin/Release/Microsoft.Scripting.AspNet.dll ./turbo-diagram/usr/lib/turbo-diagram/
sudo cp ../Diagram.SRC/Diagram/bin/Release/Microsoft.Scripting.dll ./turbo-diagram/usr/lib/turbo-diagram/
sudo cp ../Diagram.SRC/Diagram/bin/Release/Microsoft.Scripting.Metadata.dll ./turbo-diagram/usr/lib/turbo-diagram/
#sudo cp ../Diagram.SRC/Diagram/bin/Release/ScintillaNET.dll ./turbo-diagram/usr/lib/turbo-diagram/
sudo cp ../Diagram.SRC/Diagram/bin/Release/NCalc.dll ./turbo-diagram/usr/lib/turbo-diagram/

sudo rm ./turbo-diagram.deb

sudo chown -R root:root ./turbo-diagram

sudo find ./turbo-diagram -type d -exec chmod 755 {} \;
sudo find ./turbo-diagram -type f -exec chmod 644 {} \;
sudo chmod 755 ./turbo-diagram/usr/bin/diagram
sudo chmod 755 ./turbo-diagram/usr/lib/turbo-diagram/Diagram.exe

sudo rm ./turbo-diagram/usr/share/man/man1/diagram.1.gz
sudo cp ./files/diagram.1 ./turbo-diagram/usr/share/man/man1/
sudo gzip -9 ./turbo-diagram/usr/share/man/man1/diagram.1

sudo rm ./turbo-diagram/usr/share/doc/turbo-diagram/changelog.gz
sudo cp ./files/changelog ./turbo-diagram/usr/share/doc/turbo-diagram/
sudo gzip -9 ./turbo-diagram/usr/share/doc/turbo-diagram/changelog

sudo dpkg-deb --build turbo-diagram
lintian turbo-diagram.deb

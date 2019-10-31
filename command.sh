#!/bin/bash
for var in "$@"
do
    if [ "$var" = "-x" ]; then
        set -x
    fi
done

if [ "$1" = "debug" ]; then
    cd ./Diagram.SRC/
    xbuild /p:Configuration=Debug Diagram.Mono.sln

elif [ "$1" = "build" ]; then
	cd ./Diagram.SRC/
        xbuild /p:Configuration=Release Diagram.Mono.sln

elif [ "$1" = "install" ]; then
	cd ./install-linux/
	chmod 775 make-package.sh
    ./make-package.sh
    chmod 775 install-package.sh
    ./install-package.sh

elif [ "$1" = "clean" ]; then

    if [ "$2" = "project" ]; then
        echo -e "\e[31m Clean project  \e[0m"

        sudo chown kerberos:kerberos -R ./
        sudo find "./" -type d -exec chmod 775 {} \;
        sudo find "./" -type f -exec chmod 664 {} \;
        chmod 755 ./install-linux/make-package.sh
        chmod 755 ./install-linux/install-package.sh
        rm -R ./Diagram.SRC/Diagram/bin/Debug/
        rm -R ./Diagram.SRC/Diagram/bin/Release/
        rm ./install-linux/turbo-diagram.deb
    fi

    if [ "$2" = "" ]; then
        echo "clean"
        echo "  project"
    fi
else
    echo "debug"
    echo "build"
    echo "install"
    echo "clean project"
fi

#!/bin/bash
for var in "$@"
do
    if [ "$var" = "-x" ]; then
        set -x
    fi
done

if [ "$1" = "svn" ]; then

    if [ "$2" = "commit" ]; then
        echo -e "\e[31m Svn commit  \e[0m"

        MESSAGE=`date +%Y-%m-%d`
        if [ "$3" != "" ]; then
            MESSAGE=$3
        fi

        svn add --force .
        svn commit -m "$MESSAGE"
    fi

    if [ "$2" = "update" ]; then
        echo -e "\e[31m Svn update  \e[0m"

        svn update
    fi

    if [ "$2" = "status" ]; then
        echo -e "\e[31m Svn update  \e[0m"

        svn status
    fi

    if [ "$2" = "ignore" ]; then
        echo -e "\e[31m Svn set ignore list  \e[0m"
        svn propset svn:ignore -F ignorelist.txt .
    fi

    if [ "$2" = "" ]; then
        echo "svn commit {message}"
        echo "svn update"
        echo "svn status"
        echo "svn ignore - set ignore list"
    fi
elif [ "$1" = "debug" ]; then
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
    echo "svn"
    echo "debug"
    echo "build"
    echo "install"
    echo "clean project"
fi

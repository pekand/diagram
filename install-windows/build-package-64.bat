rmdir /S /Q .\files
rmdir /S /Q .\plugins
mkdir .\files
mkdir .\plugins

copy ..\Diagram.SRC\Diagram\diagram.ico .\files\
copy ..\Diagram.SRC\Diagram\bin\Release\x64\Diagram.exe .\files\

xcopy /s ..\Diagram.SRC\Diagram\bin\Release\x64\plugins\*.* .\plugins\

call subscribe "files\Diagram.exe"
call subscribe "plugins\CreateDirectoryPlugin\CreateDirectoryPlugin.dll"
call subscribe "plugins\DropPlugin\DropPlugin.dll"
call subscribe "plugins\FindUidPlugin\FindUidPlugin.dll"
call subscribe "plugins\NcalcPlugin\NcalcPlugin.dll"
call subscribe "plugins\ScriptingPlugin\ScriptingPlugin.dll"

iscc /q create-installation-package-64.iss

call subscribe "output\infinite-diagram-install.exe"

pause
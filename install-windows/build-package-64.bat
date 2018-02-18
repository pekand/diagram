rmdir /S /Q .\files
mkdir .\files
mkdir .\plugins

copy ..\Diagram.SRC\Diagram\diagram.ico .\files\
copy ..\Diagram.SRC\Diagram\bin\Release\x64\Diagram.exe .\files\

xcopy /s ..\Diagram.SRC\Diagram\bin\Release\x64\plugins\*.* .\plugins\

pause
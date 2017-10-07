rmdir /S /Q .\files
mkdir .\files

copy ..\Diagram.SRC\Diagram\diagram.ico .\files\
copy ..\Diagram.SRC\Diagram\bin\Release\x64\Diagram.exe .\files\
copy ..\Diagram.SRC\Diagram\bin\Release\x64\NCalc.dll .\files\
copy ..\Diagram.SRC\Diagram\bin\Release\x64\Newtonsoft.Json.dll .\files\

pause
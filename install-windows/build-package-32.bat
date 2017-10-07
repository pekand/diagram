rmdir /S /Q .\files
mkdir .\files

copy ..\Diagram.SRC\Diagram\diagram.ico .\files\
copy ..\Diagram.SRC\Diagram\bin\Release\x86\Diagram.exe .\files\
copy ..\Diagram.SRC\Diagram\bin\Release\x86\NCalc.dll .\files\
copy ..\Diagram.SRC\Diagram\bin\Release\x86\Newtonsoft.Json.dll .\files\

pause
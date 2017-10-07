rmdir /S /Q .\files
mkdir .\files

copy ..\Diagram.SRC\Diagram\diagram.ico .\files\
copy ..\Diagram.SRC\Diagram\bin\Release\Diagram.exe .\files\
copy ..\Diagram.SRC\Diagram\bin\Release\NCalc.dll .\files\
copy ..\Diagram.SRC\Diagram\bin\Release\Newtonsoft.Json.dll .\files\

pause
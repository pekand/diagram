rmdir /S /Q .\files
mkdir .\files

copy ..\Diagram.SRC\Diagram\diagram.ico .\files\
copy ..\Diagram.SRC\Diagram\bin\Release\Diagram.exe .\files\
copy ..\Diagram.SRC\Diagram\bin\Release\IronPython.dll .\files\
copy ..\Diagram.SRC\Diagram\bin\Release\IronPython.Modules.dll .\files\
copy ..\Diagram.SRC\Diagram\bin\Release\IronPython.SQLite.dll .\files\
copy ..\Diagram.SRC\Diagram\bin\Release\IronPython.Wpf.dll .\files\
copy ..\Diagram.SRC\Diagram\bin\Release\Microsoft.Dynamic.dll .\files\
copy ..\Diagram.SRC\Diagram\bin\Release\Microsoft.Scripting.AspNet.dll .\files\
copy ..\Diagram.SRC\Diagram\bin\Release\Microsoft.Scripting.dll .\files\
copy ..\Diagram.SRC\Diagram\bin\Release\Microsoft.Scripting.Metadata.dll .\files\
copy ..\Diagram.SRC\Diagram\bin\Release\NCalc.dll .\files\
copy ..\Diagram.SRC\Diagram\bin\Release\Newtonsoft.Json.dll .\files\
copy ..\Documentation\ReleaseNote\ReleaseNote.html .\files\

pause
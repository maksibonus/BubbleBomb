ECHO "### DELETING OLD ARCHIVE"
DEL "RamGec XNA Controls.zip"

ECHO "### REMOVING OLD FILES"
RMDIR "RamGec XNA Controls" /s /q

ECHO "### CREATING BINARIES DIRECTORY"
MKDIR "RamGec XNA Controls"


ECHO "### THEMES"
XCOPY "..\RamGec XNA Controls\bin\x86\Release\Themes" "RamGec XNA Controls\Themes" /E /Y /Q /I

ECHO "### RAMGEC XNA CONTROLS DLLS"
COPY "..\RamGec XNA Controls\bin\x86\Release\RamGec XNA Controls.dll" "RamGec XNA Controls\" /Y
COPY "..\RamGec XNA Controls\bin\x86\Release\RamGec XNA Controls.xml" "RamGec XNA Controls\" /Y

ECHO "### DEMO"
XCOPY "..\Demo\Demo\bin\x86\Release\Content" "RamGec XNA Controls\Content" /E /Y /Q /I
COPY "..\Demo\Demo\bin\x86\Release\Demo.exe" "RamGec XNA Controls\" /Y

ECHO "### WINDOW DESIGNER"
XCOPY "..\Window Designer\Window Designer\bin\x86\Release\Content" "RamGec XNA Controls\Content" /E /Y /Q /I
COPY "..\Window Designer\Window Designer\bin\x86\Release\Window Designer.exe" "RamGec XNA Controls\" /Y

ECHO "### ADDING HELP FILE"
MKDIR "RamGec XNA Controls\Help"
COPY "Help\Help\RamGec XNA Controls Help.chm" "RamGec XNA Controls\Help\" /Y


ECHO "### CREATING BINARIES ARCHIVE"
"C:\Program Files\7-Zip\7z.exe" a "RamGec XNA Controls.zip" "RamGec XNA Controls\"

ECHO "### ALL DONE"

pause
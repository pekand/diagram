import os.path

# write message to release note
# update version in about windows
# updte version in installation script

def getFirstLine(path):
    if os.path.exists(path):
        with open(path, 'r') as f:
            return f.readline()

def confirmAction():
    yes = set(['yes','y', 'ye', ''])
    choice = input("Confirm update (y/n):").lower()
    if not choice in yes:
       exit("Update cancled.")

def prependToFile(path, message):
    if os.path.exists(path):
        f = open(path, 'r')
        content = f.read()
        f = open(path, "w")
        f.write(message + "\n")
        f.write(content)
        f.close()

def replaceInFile(path, oldString, newString):
    if os.path.exists(path) and oldString.strip() != '' and newString.strip() != '':
        f = open(path, 'r')
        content = f.read()
        content = content.replace(oldString, newString)
        f = open(path, "w")
        f.write(content)
        f.close()    

first_line = getFirstLine('./ReleaseNote.txt');
oldVersion = first_line.split()
oldVersion = oldVersion[0]
print("Old verssion: " + oldVersion)
parts = oldVersion.split('.')
parts[len(parts)-1] = str(int(parts[len(parts)-1])+1)
newVersionGuest = '.'.join(parts)
newVersion = input("New varsion ("+newVersionGuest+"): ").strip()
if newVersion == '':
    newVersion = newVersionGuest
description = input("Description: ").strip()

#confirm changes
confirmAction()
prependToFile("./ReleaseNote.txt", newVersion + " " + description)
replaceInFile("./Diagram.SRC/Diagram/Src/Program.cs", oldVersion, newVersion)
replaceInFile("./Diagram.SRC/Diagram/Diagram.csproj", oldVersion, newVersion)
replaceInFile("./install-windows/create-installation-package-32.iss", oldVersion, newVersion)
replaceInFile("./install-windows/create-installation-package-64.iss", oldVersion, newVersion)


strPath = Wscript.Arguments.Item(0)
strExtension = Wscript.Arguments.Item(1)
intOlderThanDays = Wscript.Arguments.Item(2)

DeleteOldFiles strPath, strExtension, intOlderThanDays
 
Function DeleteOldFiles(strPath, strExtension, intOlderThanDays)

    Dim fso
    Set fso = CreateObject("Scripting.FileSystemObject")
    Dim myFolder
    Dim myFiles
    Dim myFile
    Dim dtmdate
    Const intArchiveAttrib = 32
    If fso.FolderExists(strPath) Then
        
        dtmdate = Date - intOlderThanDays
        Set myFolder = fso.GetFolder(strPath)
        Set myFiles = myFolder.Files
        For Each myFile In myFiles
            If myFile.DateLastModified < dtmdate _
                    And fso.GetExtensionName(myFile) = strExtension Then
                    myFile.Delete
            End If
        Next
    End If
    
End Function

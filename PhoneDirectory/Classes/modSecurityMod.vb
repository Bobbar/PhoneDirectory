Option Explicit On
Imports MySql.Data.MySqlClient
Imports System.IO
Imports System.Security.Cryptography
Imports System.Text
Imports System.Runtime.Serialization
Module modSecurityMod
    Public AccessLevels() As Access_Info
    Private Const CryptKey As String = "%hERg9@w7ELp=Y9S"
    Public Function DecodePassword(strCypher As String) As String
        Dim wrapper As New Simple3Des(CryptKey)
        Return wrapper.DecryptData(strCypher)
    End Function
    Public Function GetHashOfTable(Table As DataTable) As String
        Dim serializer = New DataContractSerializer(GetType(DataTable))
        Dim memoryStream = New MemoryStream()
        serializer.WriteObject(memoryStream, Table)
        Dim serializedData As Byte() = memoryStream.ToArray()
        Dim SHA = New SHA1CryptoServiceProvider()
        Dim hash As Byte() = SHA.ComputeHash(serializedData)
        Return Convert.ToBase64String(hash)
    End Function
    Public Function GetHashOfFile(Path As String) As String
        Dim hash As MD5
        hash = MD5.Create
        Dim hashValue() As Byte
        Dim fileStream As FileStream = File.OpenRead(Path)
        fileStream.Position = 0
        hashValue = hash.ComputeHash(fileStream)
        Dim sBuilder As New StringBuilder
        Dim i As Integer
        For i = 0 To hashValue.Length - 1
            sBuilder.Append(hashValue(i).ToString("x2"))
        Next
        fileStream.Close()
        Return sBuilder.ToString
    End Function
    Public Function GetHashOfFileStream(ByRef MemStream As IO.FileStream) As String
        Dim md5Hash As MD5 = MD5.Create
        MemStream.Position = 0
        Dim hash As Byte() = md5Hash.ComputeHash(MemStream)
        Dim sBuilder As New StringBuilder
        Dim i As Integer
        For i = 0 To hash.Length - 1
            sBuilder.Append(hash(i).ToString("x2"))
        Next
        MemStream.Position = 0
        Return sBuilder.ToString
    End Function
    Public Function GetHashOfIOStream(ByVal MemStream As IO.MemoryStream) As String
        Dim md5Hash As MD5 = MD5.Create
        MemStream.Position = 0
        Dim hash As Byte() = md5Hash.ComputeHash(MemStream)
        Dim sBuilder As New StringBuilder
        Dim i As Integer
        For i = 0 To hash.Length - 1
            sBuilder.Append(hash(i).ToString("x2"))
        Next
        MemStream.Position = 0
        Return sBuilder.ToString
    End Function
    Public Function GetSecGroupValue(SecModule As String) As Integer
        For Each Group As Access_Info In AccessLevels
            If Group.strModule = SecModule Then Return Group.intLevel
        Next
    End Function
    Public Function CanAccess(recModule As String, intAccessLevel As Integer) As Boolean 'bitwise access levels
        Dim mask As Integer = 1
        Dim calc_level As Integer
        Dim UsrLevel As Integer = intAccessLevel 'UserAccess.intAccessLevel
        Dim levels As Integer
        For levels = 0 To UBound(AccessLevels)
            calc_level = UsrLevel And mask
            If calc_level <> 0 Then
                If AccessLevels(levels).strModule = recModule Then
                    Return True
                End If
            End If
            mask = mask << 1
        Next
        Return False
    End Function
    Public Function CheckForAccess(recModule As String) As Boolean
        If Not OfflineMode Then
            If Not CanAccess(recModule, CurrentUser.AccessLevel) Then
                Dim blah = Message("You do not have the required rights for this function. Must have access to '" & recModule & "'.", vbOKOnly + vbExclamation, "Access Denied", PhoneDirectory)
                Return False
            Else
                Return True
            End If
        Else
            Dim blah = Message("Edit functions disabled. Application is currently in offline mode.", vbOKOnly + vbExclamation, "Offline Mode", PhoneDirectory)
            Return False
        End If

    End Function
    Public Sub GetUserAccess()
        Try
            Dim strQRY = "SELECT * FROM " & User_Columns.TableName & " WHERE " & User_Columns.Username & "='" & strLocalUser & "'"
            Using SQLComms As New clsMySQL_Comms, results As DataTable = SQLComms.Return_SQLTable(strQRY)
                If results.Rows.Count > 0 Then
                    Dim r As DataRow = results.Rows(0)
                    CurrentUser = New User(r.Item(User_Columns.Username).ToString, r.Item(User_Columns.Fullname).ToString, DirectCast(r.Item(User_Columns.AccessLevel), Integer), r.Item(User_Columns.UID).ToString)
                Else
                    CurrentUser = New User(0)
                End If
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub
    Public Sub GetAccessLevels()
        Try
            Dim strQRY = "SELECT * FROM " & Security_Table.TableName & " ORDER BY " & Security_Table.AccessLevel & "" ' WHERE usr_username='" & strLocalUser & "'"
            Dim rows As Integer
            Using SQLComms As New clsMySQL_Comms, results As DataTable = SQLComms.Return_SQLTable(strQRY)
                ReDim AccessLevels(0)
                rows = -1
                For Each r As DataRow In results.Rows
                    rows += 1
                    ReDim Preserve AccessLevels(rows)
                    AccessLevels(rows).intLevel = r.Item(Security_Table.AccessLevel)
                    AccessLevels(rows).strModule = r.Item(Security_Table.SecModule)
                    AccessLevels(rows).strDesc = r.Item(Security_Table.Description)
                Next
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub
End Module

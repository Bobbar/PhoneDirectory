﻿Option Explicit On
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
    'Public Function GetHashOfDevice(Device As Device_Info) As String
    '    Dim serializer = New DataContractSerializer(GetType(Device_Info))
    '    Dim memoryStream = New MemoryStream()
    '    serializer.WriteObject(memoryStream, Device)
    '    Dim serializedData As Byte() = memoryStream.ToArray()
    '    Dim SHA = New SHA1CryptoServiceProvider()
    '    Dim hash As Byte() = SHA.ComputeHash(serializedData)
    '    Return Convert.ToBase64String(hash)
    'End Function
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
        If Not CanAccess(recModule, UserAccess.intAccessLevel.Value) Then
            Dim blah = Message("You do not have the required rights for this function. Must have access to '" & recModule & "'.", vbOKOnly + vbExclamation, "Access Denied")
            Return False
        Else
            Return True
        End If
    End Function
    Public Sub GetUserAccess()
        Try
            Dim strQRY = "SELECT * FROM " & UserAccess.TableName & " WHERE " & UserAccess.strUsername.ColumnName & "='" & strLocalUser & "'"
            Using SQLComms As New clsMySQL_Comms, results As DataTable = SQLComms.Return_SQLTable(strQRY)
                If results.Rows.Count > 0 Then
                    For Each r As DataRow In results.Rows
                        UserAccess.strUsername.Value = r.Item(UserAccess.strUsername.ColumnName)
                        UserAccess.strFullname.Value = r.Item(UserAccess.strFullname.ColumnName)
                        UserAccess.intAccessLevel.Value = r.Item(UserAccess.intAccessLevel.ColumnName)
                        UserAccess.strUID.Value = r.Item(UserAccess.strUID.ColumnName)
                    Next
                Else
                    UserAccess.intAccessLevel.Value = 0
                End If
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
        End Try
    End Sub
End Module

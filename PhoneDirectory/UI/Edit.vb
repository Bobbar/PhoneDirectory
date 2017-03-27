Imports System.ComponentModel
Imports MySql.Data.MySqlClient
Public Class Edit
    Private MyExtension As New Extension_Info
    Sub New(ExtID As Integer)
        ' This call is required by the designer.
        InitializeComponent()
        GetExtension(ExtID)
        FillBoxes()
        Show()
    End Sub
    Private Sub SaveChanges()
        If ValidateData() Then
            Dim NewData As Extension_Info = CollectData()
            If UpdateExtension(NewData) Then
                PhoneDirectory.RefreshCurrent()
                Me.Dispose()
                Message("Save successful.", vbOKOnly + vbInformation, "Success", PhoneDirectory)
            End If
        End If
    End Sub
    Private Sub FillBoxes()
        txtID.Text = MyExtension.ID.Value.ToString
        txtExtension.Text = MyExtension.Extension.Value
        txtExtensionName.Text = MyExtension.ExtensionName.Value
    End Sub
    Private Function ValidateData() As Boolean
        Dim bolValid As Boolean = False
        txtExtension.Text = Trim(txtExtension.Text)
        If txtExtension.Text <> "" Then
            bolValid = True
        Else
            Return False
        End If
        txtExtensionName.Text = Trim(txtExtensionName.Text)
        If txtExtensionName.Text <> "" Then
            bolValid = True
        Else
            Return False
        End If
        Return bolValid
    End Function
    Private Function CollectData() As Extension_Info
        Dim tmpExtInfo As New Extension_Info
        tmpExtInfo.ID.Value = CInt(txtID.Text)
        tmpExtInfo.Extension.Value = txtExtension.Text
        tmpExtInfo.ExtensionName.Value = txtExtensionName.Text
        Return tmpExtInfo
    End Function
    Private Sub GetExtension(ExtID As Integer)
        Try
            Dim strQRY As String = "SELECT * FROM " & MyExtension.TableName & " WHERE " & MyExtension.ID.ColumnName & "='" & ExtID & "'"
            Using SQLComms As New clsMySQL_Comms,
                results As DataTable = SQLComms.Return_SQLTable(strQRY)
                With results.Rows(0)
                    MyExtension.ID.Value = .Item(MyExtension.ID.ColumnName)
                    MyExtension.Extension.Value = .Item(MyExtension.Extension.ColumnName)
                    MyExtension.ExtensionName.Value = .Item(MyExtension.ExtensionName.ColumnName)
                End With
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
        End Try
    End Sub
    Private Function UpdateExtension(ExtInfo As Extension_Info) As Boolean
        Try
            Dim strQRY As String = "UPDATE " & Extension_Info.TableName & " SET " & ExtInfo.Extension.ColumnName & "=@" & ExtInfo.Extension.ColumnName &
                "," & ExtInfo.ExtensionName.ColumnName & "=@" & ExtInfo.ExtensionName.ColumnName &
                " WHERE " & ExtInfo.ID.ColumnName & "='" & ExtInfo.ID.Value & "'"
            Dim rows As Integer = 0
            Using SQLComms As New clsMySQL_Comms,
                    cmd As MySqlCommand = SQLComms.Return_SQLCommand(strQRY)
                With cmd.Parameters
                    .AddWithValue("@" & ExtInfo.Extension.ColumnName, ExtInfo.Extension.Value)
                    .AddWithValue("@" & ExtInfo.ExtensionName.ColumnName, ExtInfo.ExtensionName.Value)
                End With
                rows = cmd.ExecuteNonQuery()
            End Using
            If rows > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
            Return False
        End Try
    End Function
    Private Sub cmdSave_Click(sender As Object, e As EventArgs) Handles cmdSave.Click
        SaveChanges()
    End Sub
    Private Sub cmdCancel_Click(sender As Object, e As EventArgs) Handles cmdCancel.Click
        Me.Dispose()
    End Sub
    Private Sub Edit_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        Me.Dispose()
    End Sub
End Class
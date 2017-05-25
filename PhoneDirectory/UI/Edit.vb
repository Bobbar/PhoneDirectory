Imports System.ComponentModel
Imports MySql.Data.MySqlClient
Public Class Edit
    Private MyExtension As New Extension
    Private DataParser As New DBControlParser
    Sub New(ExtID As Integer)
        ' This call is required by the designer.
        InitializeComponent()
        InitDBControls()
        GetExtension(ExtID)
        ' FillBoxes()
        Show()
    End Sub
    Private Sub InitDBControls()
        txtID.Tag = New DBControlInfo(Extension_Columns.ID)
        txtExtension.Tag = New DBControlInfo(Extension_Columns.Extension)
        txtExtensionName.Tag = New DBControlInfo(Extension_Columns.Name)
        txtDepartment.Tag = New DBControlInfo(Extension_Columns.Department)
    End Sub
    Private Sub SaveChanges()
        If ValidateData() Then
            Dim NewData As Extension = CollectData()
            If UpdateExtension(NewData) Then
                PhoneDirectory.RefreshCurrent()
                Me.Dispose()
                Message("Save successful.", vbOKOnly + vbInformation, "Success", PhoneDirectory)
            End If
        End If
    End Sub
    'Private Sub FillBoxes()
    '    txtID.Text = MyExtension.ID.Value.ToString
    '    txtExtension.Text = MyExtension.Extension.Value
    '    txtExtensionName.Text = MyExtension.ExtensionName.Value
    'End Sub
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
    Private Function CollectData() As Extension
        ' Dim tmpExtInfo As 
        'tmpExtInfo.ID.Value = CInt(Trim(txtID.Text))
        'tmpExtInfo.Extension.Value = txtExtension.Text
        'tmpExtInfo.ExtensionName.Value = txtExtensionName.Text
        Return New Extension(CInt(txtID.Text), Trim(txtExtension.Text), Trim(txtExtensionName.Text), Trim(txtDepartment.Text))
    End Function
    Private Sub GetExtension(ExtID As Integer)
        Try
            Dim strQRY As String = "SELECT * FROM " & Extension_Columns.TableName & " WHERE " & MyExtension.Columns.ID & "='" & ExtID & "'"
            Using SQLComms As New clsMySQL_Comms,
                results As DataTable = SQLComms.Return_SQLTable(strQRY)
                With results.Rows(0)
                    MyExtension.ID = .Item(MyExtension.Columns.ID)
                    MyExtension.Extension = .Item(MyExtension.Columns.Extension)
                    MyExtension.Name = .Item(MyExtension.Columns.Name)
                End With
                DataParser.FillDBFields(Me, results)
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub
    Private Function GetUpdateTable(Adapter As MySqlDataAdapter) As DataTable
        Return DataParser.ReturnUpdateTable(Me, Adapter.SelectCommand.CommandText)
        '  Dim DBRow = tmpTable.Rows(0)


    End Function
    Private Function UpdateExtension(ExtInfo As Extension) As Boolean
        Try
            'Dim strQRY As String = "UPDATE " & Extension_Info.TableName & " SET " & ExtInfo.Extension.ColumnName & "=@" & ExtInfo.Extension.ColumnName &
            '    "," & ExtInfo.ExtensionName.ColumnName & "=@" & ExtInfo.ExtensionName.ColumnName &
            '    " WHERE " & ExtInfo.ID.ColumnName & "='" & ExtInfo.ID.Value & "'"
            Dim UpdateQry As String = "SELECT * FROM " & ExtInfo.Columns.TableName & " WHERE " & ExtInfo.Columns.ID & "='" & ExtInfo.ID & "'"
            Dim rows As Integer = 0
            Using SQLComms As New clsMySQL_Comms,
                    UpdateAdapter = SQLComms.Return_Adapter(UpdateQry)
                ' cmd As MySqlCommand = SQLComms.Return_SQLCommand(strQRY)
                'With cmd.Parameters
                '    .AddWithValue("@" & ExtInfo.Extension.ColumnName, ExtInfo.Extension.Value)
                '    .AddWithValue("@" & ExtInfo.ExtensionName.ColumnName, ExtInfo.ExtensionName.Value)
                'End With
                rows = UpdateAdapter.Update(GetUpdateTable(UpdateAdapter)) 'cmd.ExecuteNonQuery()
            End Using
            If rows > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
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
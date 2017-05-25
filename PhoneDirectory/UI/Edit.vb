Imports System.ComponentModel
Imports MySql.Data.MySqlClient
Public Class Edit
    Private MyExtension As New Extension
    Private DataParser As New DBControlParser
    Private bolAddNew As Boolean = False
    Sub New(ExtID As Integer)
        ' This call is required by the designer.
        InitializeComponent()
        InitDBControls()
        GetExtension(ExtID)
        Show()
    End Sub
    Sub New()
        ' This call is required by the designer.
        InitializeComponent()
        InitDBControls()
        bolAddNew = True
        cmdSave.Visible = False
        cmdAdd.Visible = True
        Dim MyLiveBox As New clsLiveBox(Me)
        MyLiveBox.AttachToControl(txtDepartment, LiveBoxType.SelectValue, Extension_Columns.Department)
        MyLiveBox.AttachToControl(txtExtensionName, LiveBoxType.SelectValue, Extension_Columns.Name)
        Me.Text = "Add New Extension"
        Show()
    End Sub
    Private Sub InitDBControls()
        txtID.Tag = New DBControlInfo(Extension_Columns.ID)
        txtExtension.Tag = New DBControlInfo(Extension_Columns.Extension, True)
        txtExtensionName.Tag = New DBControlInfo(Extension_Columns.Name, True)
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
        Else
            Message("One or more required fields are empty.", vbExclamation + vbOKOnly, "Missing Info", Me)
        End If
    End Sub
    Private Sub AddNew()
        If ValidateData() Then
            Dim NewData As Extension = CollectData()
            If InsertExtension(NewData) Then
                Me.Dispose()
                Message("New extension added.", vbOKOnly + vbInformation, "Success", PhoneDirectory)
            End If
        Else
            Message("One or more required fields are empty.", vbExclamation + vbOKOnly, "Missing Info", Me)
        End If
    End Sub
    Private Function ValidateData() As Boolean
        Dim bolValid As Boolean = False
        Dim DBControls As New List(Of Control)
        DataParser.GetDBControls(Me, DBControls)
        For Each ctl As Control In DBControls
            Dim DBInfo As DBControlInfo = DirectCast(ctl.Tag, DBControlInfo)
            If DBInfo.Required Then
                Select Case True
                    Case TypeOf ctl Is TextBox
                        Dim txt As TextBox = ctl
                        txt.Text = Trim(txt.Text)
                        If txt.Text <> String.Empty Then
                            bolValid = True
                        Else
                            Return False
                        End If
                End Select
            End If
        Next
        Return bolValid
    End Function
    Private Function CollectData() As Extension
        If bolAddNew Then
            Return New Extension(Trim(txtExtension.Text), Trim(txtExtensionName.Text), Trim(txtDepartment.Text))
        Else
            Return New Extension(CInt(txtID.Text), Trim(txtExtension.Text), Trim(txtExtensionName.Text), Trim(txtDepartment.Text))
        End If
    End Function
    Private Sub GetExtension(ExtID As Integer)
        Try
            Dim strQRY As String = "SELECT * FROM " & Extension_Columns.TableName & " WHERE " & Extension_Columns.ID & "='" & ExtID & "'"
            Using SQLComms As New clsMySQL_Comms,
                results As DataTable = SQLComms.Return_SQLTable(strQRY)
                With results.Rows(0)
                    MyExtension = New Extension(.Item(Extension_Columns.ID), .Item(Extension_Columns.Extension), .Item(Extension_Columns.Name), .Item(Extension_Columns.Department))
                End With
                SetTitle(MyExtension)
                DataParser.FillDBFields(Me, results)
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub
    Private Sub SetTitle(Ext As Extension)
        Me.Text += " - " & Ext.Extension & " - " & Ext.Name
    End Sub
    Private Function GetUpdateTable(Adapter As MySqlDataAdapter) As DataTable
        Return DataParser.ReturnUpdateTable(Me, Adapter.SelectCommand.CommandText)
    End Function
    Private Function UpdateExtension(ExtInfo As Extension) As Boolean
        Try
            Dim UpdateQry As String = "SELECT * FROM " & ExtInfo.Columns.TableName & " WHERE " & ExtInfo.Columns.ID & "='" & ExtInfo.ID & "'"
            Dim rows As Integer = 0
            Using SQLComms As New clsMySQL_Comms,
                    UpdateAdapter = SQLComms.Return_Adapter(UpdateQry)
                rows = UpdateAdapter.Update(GetUpdateTable(UpdateAdapter))
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
    Private Function InsertExtension(ExtInfo As Extension) As Boolean
        Try
            Dim InsertQry As String = "SELECT * FROM " & ExtInfo.Columns.TableName & " LIMIT 0"
            Dim rows As Integer = 0
            Using SQLComms As New clsMySQL_Comms,
                    InsertAdapter = SQLComms.Return_Adapter(InsertQry)
                rows = InsertAdapter.Update(DataParser.ReturnInsertTable(Me, InsertQry)) 'GetinsertTable(InsertAdapter))
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
    Private Sub cmdAdd_Click(sender As Object, e As EventArgs) Handles cmdAdd.Click
        AddNew()
    End Sub
End Class
Option Explicit On
Option Strict On
Imports MySql.Data.MySqlClient
Public Class PhoneDirectory
    Private LastCommand As MySqlCommand
    Private DataParser As New DBControlParser
    Private Sub LoadProgram()
        SetDBColumns()
        GetUserAccess()
        GetAccessLevels()
        ExtendedMethods.DoubleBuffered(ExtensionGrid, True)
    End Sub
    Private Sub SetDBColumns()
        txtExtension.Tag = New DBControlInfo(Extension_Columns.Extension)
        txtExtName.Tag = New DBControlInfo(Extension_Columns.Name)
        txtDepartment.Tag = New DBControlInfo(Extension_Columns.Department)
    End Sub
    Public Sub RefreshCurrent()
        StartQuery(LastCommand)
    End Sub
    Public Sub SearchExtension()
        Try
            Dim cmd As New MySqlCommand
            Dim strStartQry As String = "SELECT * FROM " & Extension_Columns.TableName & " WHERE"
            Dim strDynaQry As String
            Dim SearchValCol As List(Of SearchVal) = BuildSearchListNew()
            For Each fld As SearchVal In SearchValCol
                If Not IsNothing(fld.Value) Then
                    If fld.Value.ToString <> "" Then
                        strDynaQry = strDynaQry + " " + fld.FieldName + " LIKE CONCAT('%', @" + fld.FieldName + ", '%') AND"
                        cmd.Parameters.AddWithValue("@" & fld.FieldName, fld.Value) 'If TypeOf fld.Value Is Boolean Then  'trackable boolean. if false, dont add it.
                    End If
                End If
            Next
            If strDynaQry = "" Then
                Clear()
                '  Dim blah = Message("Please add some filter data.", vbOKOnly + vbInformation, "Fields Missing", Me)
                Exit Sub
            End If
            Dim strQry = strStartQry & strDynaQry
            If Strings.Right(strQry, 3) = "AND" Then 'remove trailing AND from dynamic query
                strQry = Strings.Left(strQry, Strings.Len(strQry) - 3)
            End If
            strQry += "ORDER BY " & Extension_Columns.Name & " LIMIT 30"
            cmd.CommandText = strQry
            LastCommand = cmd
            StartQuery(cmd)
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub
    Private Async Sub StartQuery(QryCommand As MySqlCommand)
        Try
            Dim Results = Await Task.Run(Function()
                                             Using LocalSQLComm As New clsMySQL_Comms,
                    tmpResults As New DataTable,
                    da As New MySqlDataAdapter,
                    QryComm As MySqlCommand = QryCommand
                                                 QryComm.Connection = LocalSQLComm.Connection
                                                 da.SelectCommand = QryComm
                                                 da.Fill(tmpResults)
                                                 Return tmpResults
                                             End Using
                                         End Function)
            SendToGrid(Results)
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub
    Private Function BuildSearchListNew() As List(Of SearchVal)
        Try
            Dim tmpList As New List(Of SearchVal)
            Dim DBCtl As New List(Of Control)
            DataParser.GetDBControls(Me, DBCtl)
            For Each ctl As Control In DBCtl
                Select Case True
                    Case TypeOf ctl Is TextBox
                        Dim DBInfo As DBControlInfo = DirectCast(ctl.Tag, DBControlInfo)
                        Dim txt As TextBox = DirectCast(ctl, TextBox)
                        tmpList.Add(New SearchVal(DBInfo.DataColumn, Trim(txt.Text)))
                End Select

            Next
            Return tmpList
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            Return Nothing
        End Try
    End Function
    Private Sub SendToGrid(Results As DataTable)
        Try
            If Results Is Nothing Then Exit Sub
            SetupGrid()
            ExtensionGrid.Visible = False
            For Each r As DataRow In Results.Rows
                With ExtensionGrid.Rows
                    .Add(r.Item(Extension_Columns.Extension),
                              r.Item(Extension_Columns.Name),
                         r.Item(Extension_Columns.Department))
                    ExtensionGrid.Rows(ExtensionGrid.Rows.Count - 1).HeaderCell.Value = r.Item(Extension_Columns.ID).ToString
                    ExtensionGrid.Rows(ExtensionGrid.Rows.Count - 1).ContextMenuStrip = ContextMenuStrip
                End With
            Next
            ExtensionGrid.ClearSelection()
            ExtensionGrid.Visible = True
            ExtensionGrid.ResumeLayout()
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub
    Private Sub SetupGrid()
        ExtensionGrid.DataSource = Nothing
        ExtensionGrid.Rows.Clear()
        ExtensionGrid.Columns.Clear()
        With ExtensionGrid.Columns
            .Add("Extension", "Extension")
            .Add("Extension Name", "Extension Name")
            .Add("Department", "Department")
        End With
        ExtensionGrid.TopLeftHeaderCell.Value = "ID"
        ExtensionGrid.RowHeadersWidth = 100
        ExtensionGrid.RowHeadersVisible = False
    End Sub
    Private Sub Clear()
        txtExtension.Clear()
        txtExtName.Clear()
        txtDepartment.Clear()
        ExtensionGrid.Rows.Clear()
    End Sub
    Private Sub cmdSearch_Click(sender As Object, e As EventArgs) Handles cmdSearch.Click
        SearchExtension()
    End Sub
    Private Sub cmdClear_Click(sender As Object, e As EventArgs) Handles cmdClear.Click
        Clear()
    End Sub
    Private Sub PhoneDirectory_Load(sender As Object, e As EventArgs) Handles Me.Load
        LoadProgram()
    End Sub
    Private Sub EditSelectedExtension()
        Try
            If ExtensionGrid.SelectedCells(0) IsNot Nothing Then
                If CheckForAccess(AccessGroup.Modify) Then Dim nEdit As New Edit(CInt(ExtensionGrid.SelectedCells(0).OwningRow.HeaderCell.Value))
            End If
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub
    Private Sub ExtensionGrid_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles ExtensionGrid.CellDoubleClick
        EditSelectedExtension()
    End Sub
    Private Sub txtExtension_KeyUp(sender As Object, e As KeyEventArgs) Handles txtExtension.KeyUp
        SearchExtension()
    End Sub
    Private Sub txtExtName_KeyUp(sender As Object, e As KeyEventArgs) Handles txtExtName.KeyUp
        SearchExtension()
    End Sub
    Private Sub txtDepartment_KeyUp(sender As Object, e As KeyEventArgs) Handles txtDepartment.KeyUp
        SearchExtension()
    End Sub
    Private Sub cmdNew_Click(sender As Object, e As EventArgs) Handles cmdNew.Click
        If CheckForAccess(AccessGroup.Add) Then Dim AddNew As New Edit()
    End Sub
    Private Sub tsmEdit_Click(sender As Object, e As EventArgs) Handles tsmEdit.Click
        EditSelectedExtension()
    End Sub
    Private Sub ExtensionGrid_CellMouseDown(sender As Object, e As DataGridViewCellMouseEventArgs) Handles ExtensionGrid.CellMouseDown
        Try
            If e.ColumnIndex >= 0 And e.RowIndex >= 0 Then
                If e.Button = MouseButtons.Right And Not ExtensionGrid.Item(e.ColumnIndex, e.RowIndex).Selected Then
                    ExtensionGrid.Rows(e.RowIndex).Selected = True
                    ExtensionGrid.CurrentCell = ExtensionGrid(e.ColumnIndex, e.RowIndex)
                End If
            End If
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub
End Class

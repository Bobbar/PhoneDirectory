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
        'txtExtension.DataColumn = Extension_Table.Extension
        'txtExtName.DataColumn = Extension_Table.ExtensionName
        txtExtension.Tag = New DBControlInfo(Extension_Columns.Extension)
        txtExtName.Tag = New DBControlInfo(Extension_Columns.Name)
        txtDepartment.Tag = New DBControlInfo(Extension_Columns.Department)


    End Sub
    Public Sub RefreshCurrent()
        StartQuery(LastCommand)
    End Sub
    Public Sub SearchExtension()

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
        strQry += " LIMIT 30"
        cmd.CommandText = strQry
        LastCommand = cmd
        StartQuery(cmd)
    End Sub
    Private Async Sub StartQuery(QryCommand As MySqlCommand)
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
    End Sub
    'Private Function BuildSearchListNew() As List(Of SearchVal)
    '    Dim MyExtInfo As New Extension_Info
    '    Dim tmpList As New List(Of SearchVal)
    '    Dim DBCtl As New List(Of Control)
    '    DBCtl = GetDataControls(Me, DBCtl)
    '    For Each ctl As Control In DBCtl
    '        Select Case True
    '            Case TypeOf ctl Is MyTextBox
    '                Dim txt As MyTextBox = DirectCast(ctl, MyTextBox)
    '                tmpList.Add(New SearchVal(txt.DataColumn, Trim(txt.Text)))
    '        End Select

    '    Next

    '    'tmpList.Add(New SearchVal(MyExtInfo.Extension.ColumnName, Trim(txtExtension.Text)))
    '    'tmpList.Add(New SearchVal(MyExtInfo.ExtensionName.ColumnName, Trim(txtExtName.Text)))
    '    Return tmpList
    'End Function
    Private Function BuildSearchListNew() As List(Of SearchVal)
        Dim tmpList As New List(Of SearchVal)
        Dim DBCtl As New List(Of Control)
        DataParser.GetDBControls(Me, DBCtl) 'GetDataControls(Me, DBCtl)
        For Each ctl As Control In DBCtl
            Select Case True
                Case TypeOf ctl Is TextBox
                    Dim DBInfo As DBControlInfo = DirectCast(ctl.Tag, DBControlInfo)
                    Dim txt As TextBox = DirectCast(ctl, TextBox)
                    tmpList.Add(New SearchVal(DBInfo.DataColumn, Trim(txt.Text)))
            End Select

        Next

        'tmpList.Add(New SearchVal(MyExtInfo.Extension.ColumnName, Trim(txtExtension.Text)))
        'tmpList.Add(New SearchVal(MyExtInfo.ExtensionName.ColumnName, Trim(txtExtName.Text)))
        Return tmpList
    End Function
    'Private Function GetDataControls(ParentControl As Control, NewList As List(Of Control)) As List(Of Control)
    '    For Each ctl As Control In ParentControl.Controls
    '        If TypeOf ctl Is MyTextBox Then
    '            NewList.Add(ctl)
    '        Else
    '            If ctl.HasChildren Then
    '                GetDataControls(ctl, NewList)
    '            End If
    '        End If
    '    Next
    '    Return NewList
    'End Function
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
        ExtensionGrid.RowHeadersWidth = 70
    End Sub
    Private Sub Clear()
        txtExtension.Clear()
        txtExtName.Clear()
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
    'Private Sub txtExtName_KeyDown(sender As Object, e As KeyEventArgs) Handles txtExtName.KeyDown
    '    If e.KeyCode = Keys.Return Then
    '        SearchExtension()
    '    End If
    'End Sub
    'Private Sub txtExtension_KeyDown(sender As Object, e As KeyEventArgs) Handles txtExtension.KeyDown
    '    If e.KeyCode = Keys.Return Then
    '        SearchExtension()
    '    End If
    'End Sub
    Private Sub ExtensionGrid_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles ExtensionGrid.CellDoubleClick
        If CheckForAccess(AccessGroup.Modify) Then Dim nEdit As New Edit(CInt(ExtensionGrid.SelectedCells(0).OwningRow.HeaderCell.Value))
    End Sub

    Private Sub txtExtension_KeyUp(sender As Object, e As KeyEventArgs) Handles txtExtension.KeyUp
        SearchExtension()
    End Sub

    Private Sub txtExtName_KeyUp(sender As Object, e As KeyEventArgs) Handles txtExtName.KeyUp
        SearchExtension()
    End Sub
End Class

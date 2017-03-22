﻿Option Explicit On
Option Strict On
Imports MySql.Data.MySqlClient
Public Class PhoneDirectory
    Private LastCommand As MySqlCommand
    Private Sub LoadProgram()
        GetUserAccess()
        GetAccessLevels()
        ExtendedMethods.DoubleBuffered(ExtensionGrid, True)
    End Sub
    Public Sub RefreshCurrent()
        StartQuery(LastCommand)
    End Sub
    Public Sub SearchExtension()
        Dim cmd As New MySqlCommand
        Dim MyExtInfo As New Extension_Info
        Dim strStartQry As String = "SELECT * FROM " & Extension_Info.TableName & " WHERE "
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
            Dim blah = Message("Please add some filter data.", vbOKOnly + vbInformation, "Fields Missing", Me)
            Exit Sub
        End If
        Dim strQry = strStartQry & strDynaQry
        If Strings.Right(strQry, 3) = "AND" Then 'remove trailing AND from dynamic query
            strQry = Strings.Left(strQry, Strings.Len(strQry) - 3)
        End If
        cmd.CommandText = strQry
        LastCommand = cmd
        StartQuery(cmd)
    End Sub
    Private Sub StartQuery(ByRef QryCommand As MySqlCommand)
        Using LocalSQLComm As New clsMySQL_Comms,
                ds As New DataSet,
                da As New MySqlDataAdapter,
                QryComm As MySqlCommand = QryCommand
            QryComm.Connection = LocalSQLComm.Connection
            da.SelectCommand = QryComm
            da.Fill(ds)
            SendToGrid(ds.Tables(0))
        End Using
    End Sub
    Private Function BuildSearchListNew() As List(Of SearchVal)
        Dim MyExtInfo As New Extension_Info
        Dim tmpList As New List(Of SearchVal)
        tmpList.Add(New SearchVal(MyExtInfo.Extension.ColumnName, Trim(txtExtension.Text)))
        tmpList.Add(New SearchVal(MyExtInfo.ExtensionName.ColumnName, Trim(txtExtName.Text)))
        Return tmpList
    End Function
    Private Sub SendToGrid(Results As DataTable)
        Try
            Dim MyExtInfo As New Extension_Info
            If Results Is Nothing Then Exit Sub
            SetupGrid()
            ExtensionGrid.Visible = False
            For Each r As DataRow In Results.Rows
                With ExtensionGrid.Rows
                    .Add(r.Item(MyExtInfo.Extension.ColumnName),
                              r.Item(MyExtInfo.ExtensionName.ColumnName))
                    ExtensionGrid.Rows(ExtensionGrid.Rows.Count - 1).HeaderCell.Value = r.Item(MyExtInfo.ID.ColumnName).ToString
                End With
            Next
            ExtensionGrid.ClearSelection()
            ExtensionGrid.Visible = True
            ExtensionGrid.ResumeLayout()
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
        End Try
    End Sub
    Private Sub SetupGrid()
        ExtensionGrid.DataSource = Nothing
        ExtensionGrid.Rows.Clear()
        ExtensionGrid.Columns.Clear()
        With ExtensionGrid.Columns
            .Add("Extension", "Extension")
            .Add("Extension Name", "Extension Name")
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
    Private Sub txtExtName_KeyDown(sender As Object, e As KeyEventArgs) Handles txtExtName.KeyDown
        If e.KeyCode = Keys.Return Then
            SearchExtension()
        End If
    End Sub
    Private Sub txtExtension_KeyDown(sender As Object, e As KeyEventArgs) Handles txtExtension.KeyDown
        If e.KeyCode = Keys.Return Then
            SearchExtension()
        End If
    End Sub
    Private Sub ExtensionGrid_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles ExtensionGrid.CellDoubleClick
        If CheckForAccess(AccessGroup.Modify) Then Dim nEdit As New Edit(CInt(ExtensionGrid.SelectedCells(0).OwningRow.HeaderCell.Value))
    End Sub

End Class

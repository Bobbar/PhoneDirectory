Imports MySql.Data.MySqlClient
Public Class PhoneDirectory


    Private Sub LoadProgram()
        GetUserAccess()
        ExtendedMethods.DoubleBuffered(ExtensionGrid, True)
    End Sub
    Public Sub SearchExtension()
        Dim cmd As New MySqlCommand
        Dim table As DataTable
        Dim MyExtInfo As New Extension_Info
        Dim strStartQry As String = "SELECT * FROM " & Extension_Info.TableName & " WHERE "
        Dim strDynaQry As String
        Dim SearchValCol As List(Of SearchVal) = BuildSearchListNew()
        For Each fld As SearchVal In SearchValCol
            If Not IsNothing(fld.Value) Then
                If fld.Value.ToString <> "" Then



                    strDynaQry = strDynaQry + " " + fld.FieldName + " LIKE CONCAT('%', @" + fld.FieldName + ", '%') AND"
                    cmd.Parameters.AddWithValue("@" & fld.FieldName, fld.Value) 'If TypeOf fld.Value Is Boolean Then  'trackable boolean. if false, dont add it.
                    '    Dim bolTrackable As Boolean = CType(fld.Value, Boolean)
                    '    If bolTrackable Then
                    '        strDynaQry = strDynaQry + " " + fld.FieldName + " LIKE CONCAT('%', @" + fld.FieldName + ", '%') AND"
                    '        cmd.Parameters.AddWithValue("@" & fld.FieldName, Convert.ToInt32(fld.Value))
                    '    End If
                    'Else
                    'Select Case fld.FieldName 'use the fixed fields with EQUALS operator instead of LIKE
                    '    Case MyExtInfo.Extension.ColumnName
                    '        strDynaQry = strDynaQry + " " + fld.FieldName + "=@" + fld.FieldName + " AND"
                    '        cmd.Parameters.AddWithValue("@" & fld.FieldName, fld.Value)
                    '        Case Devices.EQType
                    '            strDynaQry = strDynaQry + " " + fld.FieldName + "=@" + fld.FieldName + " AND"
                    '            cmd.Parameters.AddWithValue("@" & fld.FieldName, fld.Value)
                    '        Case Devices.Location
                    '            strDynaQry = strDynaQry + " " + fld.FieldName + "=@" + fld.FieldName + " AND"
                    '            cmd.Parameters.AddWithValue("@" & fld.FieldName, fld.Value)
                    '        Case Devices.Status
                    '            strDynaQry = strDynaQry + " " + fld.FieldName + "=@" + fld.FieldName + " AND"
                    '            cmd.Parameters.AddWithValue("@" & fld.FieldName, fld.Value)
                    '        Case Else
                    '            strDynaQry = strDynaQry + " " + fld.FieldName + " LIKE CONCAT('%', @" + fld.FieldName + ", '%') AND"
                    '            cmd.Parameters.AddWithValue("@" & fld.FieldName, fld.Value)
                    '    End Select
                    'End If
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
        'tmpList.Add(New SearchVal(Devices.Description, Trim(txtDescription.Text)))
        'tmpList.Add(New SearchVal(Devices.EQType, GetDBValue(DeviceIndex.EquipType, cmbEquipType.SelectedIndex)))
        'tmpList.Add(New SearchVal(Devices.ReplacementYear, Trim(txtReplaceYear.Text)))
        'tmpList.Add(New SearchVal(Devices.OSVersion, GetDBValue(DeviceIndex.OSType, cmbOSType.SelectedIndex)))
        'tmpList.Add(New SearchVal(Devices.Location, GetDBValue(DeviceIndex.Locations, cmbLocation.SelectedIndex)))
        'tmpList.Add(New SearchVal(Devices.CurrentUser, Trim(txtCurUser.Text)))
        'tmpList.Add(New SearchVal(Devices.Status, GetDBValue(DeviceIndex.StatusType, cmbStatus.SelectedIndex)))
        'tmpList.Add(New SearchVal(Devices.Trackable, chkTrackables.Checked))
        Return tmpList
    End Function

    Private Sub SendToGrid(Results As DataTable) ' Data() As Device_Info)
        Try
            Dim MyExtInfo As New Extension_Info
            If Results Is Nothing Then Exit Sub
            SetupGrid()
            ' StatusBar(strLoadingGridMessage)
            ' Application.DoEvents()
            ' Dim table As New DataTable
            'table.Columns.Add("Extension", GetType(String))
            'table.Columns.Add("Name", GetType(String))
            'table.Columns.Add("ID", GetType(String))
            'table.Columns.Add("Device Type", GetType(String))
            'table.Columns.Add("Description", GetType(String))
            'table.Columns.Add("OS Version", GetType(String))
            'table.Columns.Add("Location", GetType(String))
            'table.Columns.Add("PO Number", GetType(String))
            'table.Columns.Add("Purchase Date", GetType(Date))
            'table.Columns.Add("Replace Year", GetType(String))
            'table.Columns.Add("Modified", GetType(Date))
            'table.Columns.Add("GUID", GetType(String))
            ' ExtensionGrid.SuspendLayout()
            ExtensionGrid.Visible = False
            For Each r As DataRow In Results.Rows

                With ExtensionGrid.Rows
                    .Add(r.Item(MyExtInfo.Extension.ColumnName),
                              r.Item(MyExtInfo.ExtensionName.ColumnName))

                    ExtensionGrid.Rows(ExtensionGrid.Rows.Count - 1).HeaderCell.Value = r.Item(MyExtInfo.ID.ColumnName).ToString

                End With
                'table.Rows.Add(r.Item(MyExtInfo.Extension.ColumnName),
                '              r.Item(MyExtInfo.ExtensionName.ColumnName))

                ' GetHumanValue(DeviceIndex.EquipType, r.Item(Devices.EQType)),
                ' r.Item(Devices.Description),
                ' GetHumanValue(DeviceIndex.OSType, r.Item(Devices.OSVersion)),
                ' GetHumanValue(DeviceIndex.Locations, r.Item(Devices.Location)),
                ' r.Item(Devices.PO),
                ' r.Item(Devices.PurchaseDate),
                'r.Item(Devices.ReplacementYear),
                'r.Item(Devices.LastMod_Date),
                'r.Item(Devices.DeviceUID))
            Next
            ' bolGridFilling = True
            '  ExtensionGrid.DataSource = table
            ExtensionGrid.ClearSelection()
            ExtensionGrid.Visible = True
            ExtensionGrid.ResumeLayout()
            ' bolGridFilling = False
            ' DisplayRecords(table.Rows.Count)
            '   table.Dispose()
            'DoneWaiting()
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
        End Try
    End Sub
    Private Sub SetupGrid()
        ExtensionGrid.DataSource = Nothing
        ExtensionGrid.Rows.Clear()
        ExtensionGrid.Columns.Clear()
        'Dim intQty As New DataGridViewColumn
        'intQty.Name = "Qty"
        'intQty.HeaderText = "Qty"
        'intQty.ValueType = GetType(Integer)
        'intQty.CellTemplate = New DataGridViewTextBoxCell
        With ExtensionGrid.Columns
            .Add("Extension", "Extension")
            .Add("Extension Name", "Extension Name")
            '.Add(intQty) '"Qty", "Qty")
            '.Add(DataGridCombo(DeviceIndex.Locations, "Location", Attrib_Type.Location)) '.Add("Location")
            '.Add(DataGridCombo(SibiIndex.ItemStatusType, "Status", Attrib_Type.SibiItemStatusType))
            '.Add("Replace Asset", "Replace Asset")
            '.Add("Replace Serial", "Replace Serial")
            '.Add("New Asset", "New Asset")
            '.Add("New Serial", "New Serial")
            '.Add("Org Code", "Org Code")
            '.Add("Object Code", "Object Code")
            '.Add("Item UID", "Item UID")
        End With
        ExtensionGrid.TopLeftHeaderCell.Value = "ID"
        ExtensionGrid.RowHeadersWidth = 70
        ' SetColumnWidths()
        'RequestItemsGrid.Columns.Item("Item UID").ReadOnly = True
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
End Class

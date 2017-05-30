Public Class PopulateDB
    Public Sub PopulateNames()
        Dim ExtensionsTable As New DataTable
        Using Comms As New clsMySQL_Comms
            ExtensionsTable = Comms.Return_SQLTable("SELECT * FROM " & Extension_Columns.TableName)

            For Each r As DataRow In ExtensionsTable.Rows

                Dim NameString = r.Item(Extension_Columns.Name)
                Dim ID = r.Item(Extension_Columns.ID)

                Debug.Print(ID & " - " & NameString)

                Dim FirstName As String = ""
                Dim LastName As String = ""
                ' Dim FormattedName = txtExtensionName.Text
                Dim NameArray() = Split(NameString, ",")
                If NameArray.Length > 0 AndAlso NameArray.Length = 2 Then
                    LastName = Trim(NameArray(0))
                    FirstName = Trim(NameArray(1))
                Else
                    ' Debugger.Break()
                    FirstName = Trim(NameArray(0))
                End If


                Update_SQLValue(Extension_Columns.TableName, Extension_Columns.FirstName, FirstName, Extension_Columns.ID, ID)
                Update_SQLValue(Extension_Columns.TableName, Extension_Columns.LastName, LastName, Extension_Columns.ID, ID)


            Next

        End Using



    End Sub




End Class

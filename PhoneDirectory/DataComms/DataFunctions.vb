Imports MySql.Data.MySqlClient
Module DataFunctions
    Public Function Update_SQLValue(table As String, fieldIN As String, valueIN As String, idField As String, idValue As String) As Integer
        Try
            Dim sqlUpdateQry As String = "UPDATE " & table & " SET " & fieldIN & "=@ValueIN  WHERE " & idField & "='" & idValue & "'"
            Using SQLComms As New clsMySQL_Comms, cmd As MySqlCommand = SQLComms.Return_SQLCommand(sqlUpdateQry)
                cmd.Parameters.AddWithValue("@ValueIN", valueIN)
                Return cmd.ExecuteNonQuery()
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            Return Nothing
        End Try
    End Function
    Public Sub RefreshLocalDBCache()
        Try
            Dim tsk = Task.Run(Sub()
                                   BuildingCache = True
                                   Using conn As New SQLite_Comms(False)
                                       conn.RefreshSQLCache()
                                   End Using
                                   BuildingCache = False
                               End Sub)
        Catch ex As Exception
            BuildingCache = False
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub
End Module

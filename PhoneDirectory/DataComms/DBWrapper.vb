Imports MySql.Data.MySqlClient
Imports System.Data.SQLite
Imports System.Data.Common
Public Class DBWrapper
    Public Function GetCommand() As DbCommand
        If OfflineMode Then
            Return New SQLiteCommand
        Else
            Return New MySqlCommand
        End If
    End Function
    Public Function DataTableFromQueryString(Query As String) As DataTable
        Using conn = GetConnection(), results As New DataTable, da As DbDataAdapter = GetAdapter()
            da.SelectCommand = GetCommand(Query)
            da.Fill(results)
            Return results
        End Using
    End Function
    Public Function DataTableFromCommand(ByRef Command As DbCommand) As DataTable
        Using conn = GetConnection(), results As New DataTable, da As DbDataAdapter = GetAdapter()
            Command.Connection = conn
            da.SelectCommand = Command
            da.Fill(results)
            da.Dispose()
            Command.Dispose()
            Return results
        End Using
    End Function
    Public Function GetCommand(QryString As String) As DbCommand
        If OfflineMode Then
            Return New SQLiteCommand(QryString, GetConnection)
        Else
            Return New MySqlCommand(QryString, GetConnection)
        End If
    End Function
    Public Function GetAdapter() As DbDataAdapter
        If OfflineMode Then
            Return New SQLiteDataAdapter
        Else
            Return New MySqlDataAdapter
        End If
    End Function
    Public Function GetConnection() As DbConnection
        If OfflineMode Then
            Dim SQLiteComms As New SQLite_Comms
            Return SQLiteComms.Connection
        Else
            Dim MySQLComms As New clsMySQL_Comms
            Return MySQLComms.Connection
        End If
    End Function
End Class

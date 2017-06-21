Imports System.Data.SQLite
Imports System.IO
Public Class SQLite_Comms : Implements IDisposable
    Private SQLiteConnectString As String = "Data Source=" & strSQLitePath
    Private ConnectionException As Exception
    Public Connection As SQLiteConnection = NewConnection()
    Sub New(Optional OpenConnectionOnCall As Boolean = True)
        If OpenConnectionOnCall Then
            If Not OpenConnection() Then
                Throw ConnectionException 'If cannot connect, collect the exact exception and pass it to the referencing object
                Dispose()
            End If
        End If
    End Sub
    Private Function GetRemoteDBTable() As DataTable
        Dim qry As String = "SELECT * FROM extensions"
        Using conn As New clsMySQL_Comms, results As New DataTable, adapter = conn.Return_Adapter(qry)
            adapter.AcceptChangesDuringFill = False
            adapter.Fill(results)
            results.TableName = "extensions"
            Return results
        End Using

    End Function
    Public Sub RefreshSQLCache()
        CloseConnection()
        If File.Exists(strSQLiteDir) Then
            File.Delete(strSQLitePath)
        Else
            Dim di As DirectoryInfo = Directory.CreateDirectory(strSQLiteDir)
        End If
        SQLiteConnection.CreateFile(strSQLitePath)
        Connection = NewConnection()
        OpenConnection()
        CreateCacheTables()
        ImportDatabase()
    End Sub
    Private Sub CreateCacheTables()
        Dim qry As String = "CREATE TABLE `extensions` (
  `id` INTEGER PRIMARY KEY,
  `extension` varchar(13) NOT NULL,
  `user` varchar(60) NOT NULL,
  `department` varchar(100) DEFAULT NULL,
  `firstname` varchar(45) NOT NULL,
  `lastname` varchar(45) DEFAULT NULL)"
        Using cmd As New SQLiteCommand(qry, Connection)
            cmd.ExecuteNonQuery()
        End Using
    End Sub
    Private Sub ImportDatabase()
        OpenConnection()
        Using cmd = Connection.CreateCommand, adapter = New SQLiteDataAdapter(cmd), builder As New SQLiteCommandBuilder(adapter)
            cmd.CommandText = "SELECT * FROM extensions"
            adapter.Update(GetRemoteDBTable)
        End Using
    End Sub
    Public Function OpenConnection() As Boolean
        Try
            If Connection.State <> ConnectionState.Open Then
                CloseConnection()
                Connection = NewConnection()
                Connection.Open()
            End If
            If Connection.State = ConnectionState.Open Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Debug.Print(ex.Message)
            Return False
        End Try
    End Function
    Public Sub CloseConnection()
        Connection.Close()
        Connection.Dispose()
    End Sub
    Private Function NewConnection() As SQLiteConnection
        Return New SQLiteConnection(SQLiteConnectString)
    End Function
#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
            End If
            CloseConnection()

            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
        End If
        disposedValue = True
    End Sub

    ' TODO: override Finalize() only if Dispose(disposing As Boolean) above has code to free unmanaged resources.
    'Protected Overrides Sub Finalize()
    '    ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(True)
        ' TODO: uncomment the following line if Finalize() is overridden above.
        ' GC.SuppressFinalize(Me)
    End Sub
#End Region
End Class

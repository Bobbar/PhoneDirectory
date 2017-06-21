Imports System.Environment
Module Paths
    Public ReadOnly strAppDir As String = GetFolderPath(SpecialFolder.ApplicationData) & "\PhoneDirectory\"
    Public ReadOnly strLogName As String = "log.log"
    Public ReadOnly strLogPath As String = strAppDir & strLogName
    Public ReadOnly strTempPath As String = strAppDir & "temp\"

    Public ReadOnly strSQLiteDBName As String = "cache.db"
    Public ReadOnly strSQLitePath As String = strAppDir & "SQLiteCache\" & strSQLiteDBName
    Public ReadOnly strSQLiteDir As String = strAppDir & "SQLiteCache\"

End Module

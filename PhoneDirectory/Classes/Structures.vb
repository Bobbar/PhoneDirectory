Public Structure Access_Info
    Public strModule As String
    Public intLevel As Integer
    Public strDesc As String
End Structure
Public Class User_Info
    Public Const TableName As String = "users"
    Public strUsername As New DBColumn("usr_username")
    Public strFullname As New DBColumn("usr_fullname")
    Public intAccessLevel As New DBColumn("usr_access_level")
    Public strUID As New DBColumn("usr_UID")
End Class
Public Class DBColumn
    Public Value As Object
    Public ColumnName As String
    Sub New(nColumnName As String)
        ' Value = Value
        ColumnName = nColumnName
    End Sub
    Sub New(nValue As Object, nColumnName As String)

        Value = nValue
        ColumnName = nColumnName
    End Sub
End Class
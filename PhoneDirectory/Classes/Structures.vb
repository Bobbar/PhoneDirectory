Public Structure Access_Info
    Public strModule As String
    Public intLevel As Integer
    Public strDesc As String
End Structure
Public Class User_Info
    Public Const TableName As String = "users"
    Public strUsername As New DBColumn_String("usr_username")
    Public strFullname As New DBColumn_String("usr_fullname")
    Public intAccessLevel As New DBColumn_Integer("usr_access_level")
    Public strUID As New DBColumn_String("usr_UID")
End Class
Public Class Extension_Info
    Public Const TableName As String = "extensions"
    Public ID As New DBColumn_Integer("id")
    Public Extension As New DBColumn_String("extension")
    Public ExtensionName As New DBColumn_String("user")
End Class
Public Class Extension_Table
    Public Const TableName As String = "extensions"
    Public Const ID As String = "id"
    Public Const Extension As String = "extension"
    Public Const ExtensionName As String = "user"
End Class
Public Class DBColumn_String
    Public Value As String
    Public ColumnName As String
    Sub New(nColumnName As String)
        ' Value = Value
        ColumnName = nColumnName
    End Sub
    Sub New(nValue As String, nColumnName As String)

        Value = nValue
        ColumnName = nColumnName
    End Sub
    Public ReadOnly Property cName As String
        Get
            Return ColumnName
        End Get
    End Property
End Class
Public Class DBColumn_Integer
    Public Value As Integer
    Public ColumnName As String
    Sub New(nColumnName As String)
        ' Value = Value
        ColumnName = nColumnName
    End Sub
    Sub New(nValue As Integer, nColumnName As String)

        Value = nValue
        ColumnName = nColumnName
    End Sub

End Class
Public Class security
    Public Const TableName As String = "security"
    Public Const SecModule As String = "sec_module"
    Public Const AccessLevel As String = "sec_access_level"
    Public Const Description As String = "sec_desc"
End Class
Public Structure Access_Info
    Public strModule As String
    Public intLevel As Integer
    Public strDesc As String
End Structure
'Public Class OLD_User_Info
'    Public Const TableName As String = "users"
'    Public strUsername As New DBColumn_String("usr_username")
'    Public strFullname As New DBColumn_String("usr_fullname")
'    Public intAccessLevel As New DBColumn_Integer("usr_access_level")
'    Public strUID As New DBColumn_String("usr_UID")
'End Class

Public Class User
    Public Columns As User_Table

    Public Property Username As String
    Public Property Fullname As String
    Public Property AccessLevel As Integer
    Public Property UID As String

    Sub New(Username As String, Fullname As String, AccessLevel As Integer, UID As String)
        Me.Username = Username
        Me.Fullname = Fullname
        Me.AccessLevel = AccessLevel
        Me.UID = UID
    End Sub

    Sub New(AccessLevel As Integer)
        Me.AccessLevel = AccessLevel

    End Sub

End Class

Public Class User_Table
    Public ReadOnly TableName As String = "users"
    Public ReadOnly Username As String = "usr_username"
    Public ReadOnly Fullname As String = "usr_fullname"
    Public ReadOnly AccessLevel As String = "usr_access_level"
    Public ReadOnly UID As String = "usr_UID"



End Class
'Public Class OLD_Extension_Info
'    Public Const TableName As String = "extensions"
'    Public ID As New DBColumn_Integer("id")
'    Public Extension As New DBColumn_String("extension")
'    Public ExtensionName As New DBColumn_String("user")
'End Class
Public Class Extension
    Public Columns As Extension_Table
    Public Property ID As Object
    Public Property Extension As String
    Public Property Name As String
    Public Property Department As String
    Sub New(ID As Object, Extension As String, Name As String, Department As String)
        Me.ID = ID
        Me.Extension = Extension
        Me.Name = Name
        Me.Department = Department
    End Sub
    Sub New(Extension As String, Name As String, Department As String)
        Me.Extension = Extension
        Me.Name = Name
        Me.Department = Department
    End Sub
    Sub New()

    End Sub
End Class

Public Class Extension_Table

    Public ReadOnly TableName As String = "extensions"
    Public ReadOnly ID As String = "id"
    Public ReadOnly Extension As String = "extension"
    Public ReadOnly Name As String = "user"
    Public ReadOnly Department As String = "department"
End Class
'Public Class DBColumn_String
'    Public Value As String
'    Public ColumnName As String
'    Sub New(nColumnName As String)
'        ' Value = Value
'        ColumnName = nColumnName
'    End Sub
'    Sub New(nValue As String, nColumnName As String)

'        Value = nValue
'        ColumnName = nColumnName
'    End Sub
'    Public ReadOnly Property cName As String
'        Get
'            Return ColumnName
'        End Get
'    End Property
'End Class
'Public Class DBColumn_Integer
'    Public Value As Integer
'    Public ColumnName As String
'    Sub New(nColumnName As String)
'        ' Value = Value
'        ColumnName = nColumnName
'    End Sub
'    Sub New(nValue As Integer, nColumnName As String)

'        Value = nValue
'        ColumnName = nColumnName
'    End Sub

'End Class
Public Class Security_Table
    Public Const TableName As String = "security"
    Public Const SecModule As String = "sec_module"
    Public Const AccessLevel As String = "sec_access_level"
    Public Const Description As String = "sec_desc"
End Class
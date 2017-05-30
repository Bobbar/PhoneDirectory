Public Class Extension_Table
    Public ReadOnly TableName As String = "extensions"
    Public ReadOnly ID As String = "id"
    Public ReadOnly Extension As String = "extension"
    Public ReadOnly Name As String = "user"
    Public ReadOnly Department As String = "department"
    Public ReadOnly FirstName As String = "firstname"
    Public ReadOnly LastName As String = "lastname"
End Class
Public Class Security_Table
    Public Const TableName As String = "security"
    Public Const SecModule As String = "sec_module"
    Public Const AccessLevel As String = "sec_access_level"
    Public Const Description As String = "sec_desc"
End Class
Public Class User_Table
    Public ReadOnly TableName As String = "users"
    Public ReadOnly Username As String = "usr_username"
    Public ReadOnly Fullname As String = "usr_fullname"
    Public ReadOnly AccessLevel As String = "usr_access_level"
    Public ReadOnly UID As String = "usr_UID"
End Class

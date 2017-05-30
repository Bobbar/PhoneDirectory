Public Enum LiveBoxType
    DynamicSearch
    InstaLoad
    SelectValue
    UserSelect
End Enum
Public Class SearchVal
    Public Property FieldName As String
    Public Property Value As Object
    Public Sub New(ByVal strFieldName As String, ByVal obValue As Object)
        FieldName = strFieldName
        Value = obValue
    End Sub
End Class
Public NotInheritable Class AccessGroup
    Public Const Add As String = "add"
    Public Const CanRun As String = "can_run"
    Public Const Delete As String = "delete"
    Public Const Modify As String = "modify"
End Class
Public Class User
    Public Columns As New User_Table
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

Public Class Extension
    Public Columns As New Extension_Table
    Public Property ID As Object
    Public Property Extension As Object
    Public Property Name As Object
    Public Property Department As Object
    Sub New(ID As Object, Extension As Object, Name As Object, Department As Object)
        Me.ID = ID
        Me.Extension = Extension
        Me.Name = Name
        Me.Department = Department
    End Sub
    Sub New(Extension As Object, Name As Object, Department As Object)
        Me.Extension = Extension
        Me.Name = Name
        Me.Department = Department
    End Sub
    Sub New()

    End Sub
End Class

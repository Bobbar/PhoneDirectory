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


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


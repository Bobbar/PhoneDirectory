
Public Class MyTextBox
    Inherits TextBox
    Private DBColumn As String

    ''' <summary>
    ''' Stores Database Column ID
    ''' </summary>
    ''' <returns></returns>
    Public Property DataColumn As String
        Set(value As String)
            DBColumn = value
        End Set
        Get
            Return DBColumn
        End Get
    End Property
End Class

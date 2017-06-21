Module Global_Instances
    Public CurrentUser As User
    Public User_Columns As New User_Table
    Public Extension_Columns As New Extension_Table
    Public DBFunc As New DBWrapper
    Public OfflineMode As Boolean = False
    Public BuildingCache As Boolean = False
End Module

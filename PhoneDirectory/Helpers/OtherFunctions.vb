Imports System.Environment
Imports System.IO
Imports System.Runtime.InteropServices
Imports MyDialogLib
Imports System.Text.RegularExpressions
Module OtherFunctions
    Public stpw As New Stopwatch
    Public Sub EndProgram()
        'ProgramEnding = True
        If Not BuildingCache Then
            Logger("Ending Program...")
            'PurgeTempDir()
            Application.Exit()
        Else
            Message("DB Cache is still being built. Please wait an try again.", vbOKOnly + vbInformation, "Application Busy")
        End If
    End Sub
    Public Sub Logger(Message As String)
        Dim MaxLogSizeKiloBytes As Short = 100
        Dim DateStamp As String = DateTime.Now.ToString
        Dim infoReader As FileInfo
        infoReader = My.Computer.FileSystem.GetFileInfo(strLogPath)
        If Not File.Exists(strLogPath) Then
            Dim di As DirectoryInfo = Directory.CreateDirectory(strAppDir)
            Using sw As StreamWriter = File.CreateText(strLogPath)
                sw.WriteLine(DateStamp & ": Log Created...")
                sw.WriteLine(DateStamp & ": " & Message)
            End Using
        Else
            If (infoReader.Length / 1000) < MaxLogSizeKiloBytes Then
                Using sw As StreamWriter = File.AppendText(strLogPath)
                    sw.WriteLine(DateStamp & ": " & Message)
                End Using
            Else
                If RotateLogs() Then
                    Using sw As StreamWriter = File.AppendText(strLogPath)
                        sw.WriteLine(DateStamp & ": " & Message)
                    End Using
                End If
            End If
        End If
    End Sub
    Private Function RotateLogs() As Boolean
        Try
            File.Copy(strLogPath, strLogPath + ".old", True)
            File.Delete(strLogPath)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
    Public Function Message(ByVal Prompt As String, Optional ByVal Buttons As Integer = vbOKOnly + vbInformation, Optional ByVal Title As String = Nothing, Optional ByVal ParentFrm As Form = Nothing) As MsgBoxResult
        Dim NewMessage As New MyDialog(ParentFrm)
        Return NewMessage.DialogMessage(Prompt, Buttons, Title, ParentFrm)
    End Function
    Public Sub StartTimer()
        stpw.Stop()
        stpw.Reset()
        stpw.Start()
    End Sub
    Private intTimerHits As Integer = 0
    Public Function StopTimer()
        stpw.Stop()
        intTimerHits += 1
        Dim Results As String = intTimerHits & "  Stopwatch: MS:" & stpw.ElapsedMilliseconds & " Ticks: " & stpw.ElapsedTicks
        Debug.Print(Results)
        Return Results
    End Function
    Public Function CleanDBValue(Value As String) As Object
        Dim CleanString As String = Regex.Replace(Trim(Value), "[/\r?\n|\r]+", String.Empty)
        Return IIf(CleanString = String.Empty, DBNull.Value, CleanString)
    End Function
    Public Function NoNull(DBVal As Object) As String
        Try
            Return IIf(IsDBNull(DBVal), "", DBVal.ToString).ToString
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            Return ""
        End Try
    End Function
End Module

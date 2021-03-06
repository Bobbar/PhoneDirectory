﻿Imports System.ComponentModel
Imports MySql.Data.MySqlClient
Public Class clsLiveBox : Implements IDisposable
    Private RowLimit As Integer = 30
    Private WithEvents HideTimer As Timer
    Private LiveBox As ListBox
    Private strPrevSearchString As String
    Private LiveBoxResults As DataTable
    Private LiveBoxControls As New List(Of LiveBoxArgs)
    Private Structure LiveBoxArgs
        Public Control As TextBox
        Public Type As LiveBoxType
        Public ViewMember As String
        Public DataMember As String
    End Structure
    Private CurrentLiveBoxArgs As LiveBoxArgs
    Sub New(ParentForm As Form)
        InitializeControl(ParentForm)
        InitializeTimer()
    End Sub
    Private Sub InitializeControl(ParentForm As Form)
        LiveBox = New ListBox
        LiveBox.Parent = ParentForm
        LiveBox.BringToFront()
        'AddHandler LiveBox.MouseClick, AddressOf LiveBox_MouseClick
        AddHandler LiveBox.MouseDown, AddressOf LiveBox_MouseDown
        AddHandler LiveBox.MouseMove, AddressOf LiveBox_MouseMove
        AddHandler LiveBox.KeyDown, AddressOf LiveBox_KeyDown
        ExtendedMethods.DoubleBufferedListBox(LiveBox, True)
        LiveBox.Visible = False
        SetStyle()
    End Sub
    Private Sub InitializeTimer()
        HideTimer = New Timer
        HideTimer.Interval = 250
        HideTimer.Enabled = True
        AddHandler HideTimer.Tick, AddressOf HideTimer_Tick
    End Sub
    Private Sub LiveBoxSelect()
        Select Case CurrentLiveBoxArgs.Type
            Case LiveBoxType.SelectValue
                CurrentLiveBoxArgs.Control.Text = LiveBox.Text
        End Select
        HideLiveBox()
    End Sub
    ''' <summary>
    ''' Runs the DB query Asychronously.
    ''' </summary>
    ''' <param name="SearchString"></param>
    Private Async Sub ProcessSearch(SearchString As String)
        strPrevSearchString = SearchString
        Dim Results = Await Task.Run(Function()
                                         Dim strQry As String
                                         strQry = "SELECT * FROM " & Extension_Columns.TableName & " WHERE " & CurrentLiveBoxArgs.ViewMember & " LIKE CONCAT('%', @Search_Value, '%') GROUP BY " & CurrentLiveBoxArgs.ViewMember & " ORDER BY " & CurrentLiveBoxArgs.ViewMember & " LIMIT " & RowLimit
                                         Using MySQLComms As New clsMySQL_Comms, tmpResults As New DataTable, da As New MySqlDataAdapter, cmd As MySqlCommand = MySQLComms.Return_SQLCommand(strQry)
                                             cmd.Parameters.AddWithValue("@Search_Value", SearchString)
                                             da.SelectCommand = cmd
                                             da.Fill(tmpResults)
                                             Return tmpResults
                                         End Using
                                     End Function)
        DrawLiveBox(Results)
        LiveBoxResults = Results
    End Sub
    Private Sub DrawLiveBox(dtResults As DataTable)
        Try
            If dtResults.Rows.Count > 0 Then
                Dim strQryRow As String
                strQryRow = CurrentLiveBoxArgs.ViewMember
                LiveBox.Items.Clear()
                For Each dr As DataRow In dtResults.Rows
                    LiveBox.Items.Add(dr.Item(strQryRow))
                Next
                LiveBox.Visible = True
                PosistionLiveBox()
                'LiveBox.Visible = True
                If strPrevSearchString <> Trim(CurrentLiveBoxArgs.Control.Text) Then
                    StartLiveSearch(CurrentLiveBoxArgs) 'if search string has changed since last completion, run again.
                End If
            Else
                LiveBox.Visible = False
            End If
        Catch
            LiveBox.Visible = False
            LiveBox.Items.Clear()
        End Try
    End Sub
    Private Sub PosistionLiveBox()
        Dim ScreenPos As Point = LiveBox.Parent.PointToClient(CurrentLiveBoxArgs.Control.Parent.PointToScreen(CurrentLiveBoxArgs.Control.Location))
        ScreenPos.Y = ScreenPos.Y + CurrentLiveBoxArgs.Control.Height
        LiveBox.Location = ScreenPos
        LiveBox.Width = CurrentLiveBoxArgs.Control.Width
        Dim FormBounds As Rectangle = LiveBox.Parent.ClientRectangle
        If LiveBox.PreferredHeight + LiveBox.Top > FormBounds.Bottom Then
            LiveBox.Height = FormBounds.Bottom - LiveBox.Top - LiveBox.Padding.Bottom
        Else
            LiveBox.Height = LiveBox.PreferredHeight
        End If
    End Sub
    Public Sub GiveLiveBoxFocus()
        LiveBox.Focus()
        If LiveBox.SelectedIndex = -1 Then
            LiveBox.SelectedIndex = 0
        End If
    End Sub
    Private Sub StartLiveSearch(Args As LiveBoxArgs)
        CurrentLiveBoxArgs = Args
        Dim strSearchString As String = Trim(CurrentLiveBoxArgs.Control.Text)
        If strSearchString <> "" Then
            ProcessSearch(strSearchString)
        Else
            HideLiveBox()
        End If
    End Sub
    Public Sub HideLiveBox()
        Try
            LiveBox.Visible = False
            LiveBox.Items.Clear()
        Catch
        End Try
    End Sub
    Private Sub CollectLiveBoxArgs(Control As Control, Type As LiveBoxType, ViewMember As String, Optional DataMember As String = Nothing)
        With CurrentLiveBoxArgs
            .Control = Control
            .Type = Type
            .ViewMember = ViewMember
            If Not IsNothing(DataMember) Then .DataMember = DataMember
        End With
    End Sub
    Private Sub LiveBox_MouseMove(sender As Object, e As MouseEventArgs)
        LiveBox.SelectedIndex = LiveBox.IndexFromPoint(e.Location)
    End Sub
    Private Sub LiveBox_KeyDown(sender As Object, e As KeyEventArgs)
        Select Case e.KeyCode
            Case Keys.Enter
                LiveBoxSelect()
            Case Keys.Escape
                HideLiveBox()
        End Select
    End Sub
    Private Sub LiveBox_MouseDown(sender As Object, e As MouseEventArgs)
        Select Case e.Button
            Case MouseButtons.Left
                LiveBoxSelect()
            Case MouseButtons.Right
                HideLiveBox()
        End Select
    End Sub
    Private Sub SetStyle()
        Dim LiveBoxFont As Font = New Font("Consolas", 11.25, FontStyle.Bold)
        With LiveBox
            .BackColor = Color.FromArgb(135, 188, 252)
            .BorderStyle = BorderStyle.FixedSingle
            .Font = LiveBoxFont
            .ForeColor = Color.Black
            .Padding = New Padding(0, 0, 0, 10)
        End With
    End Sub
    Private Sub HideTimer_Tick(sender As Object, e As EventArgs) Handles HideTimer.Tick
        If Not IsNothing(CurrentLiveBoxArgs.Control) Then
            If Not CurrentLiveBoxArgs.Control.Focused And Not LiveBox.Focused Then
                If LiveBox.Visible Then HideLiveBox()
            End If
            If Not CurrentLiveBoxArgs.Control.Enabled Then
                If LiveBox.Visible Then HideLiveBox()
            End If
            If TypeOf CurrentLiveBoxArgs.Control Is TextBox Then
                Dim txt As TextBox = CurrentLiveBoxArgs.Control
                If txt.ReadOnly Then
                    If LiveBox.Visible Then HideLiveBox()
                End If
            End If
        End If
    End Sub
    Private Sub Control_KeyUp(sender As Object, e As KeyEventArgs)
        If e.KeyCode = Keys.Escape Then
            HideLiveBox()
        Else
            If e.KeyCode <> Keys.ShiftKey And e.KeyCode <> Keys.Tab Then
                For Each item As LiveBoxArgs In LiveBoxControls
                    If item.Control Is sender Then
                        If Not item.Control.ReadOnly Then
                            StartLiveSearch(item)
                        End If
                    End If
                Next
            End If
        End If
    End Sub
    Private Sub Control_KeyDown(sender As Object, e As KeyEventArgs)
        If e.KeyCode = Keys.Down Then
            GiveLiveBoxFocus()
        End If
    End Sub
    Public Sub AttachToControl(Control As Control, Type As LiveBoxType, ViewMember As String, Optional DataMember As String = Nothing)
        Dim ControlArgs As New LiveBoxArgs
        ControlArgs.Control = Control
        ControlArgs.Type = Type
        ControlArgs.ViewMember = ViewMember
        If Not IsNothing(DataMember) Then ControlArgs.DataMember = DataMember
        LiveBoxControls.Add(ControlArgs)
        AddHandler ControlArgs.Control.KeyUp, AddressOf Control_KeyUp
        AddHandler ControlArgs.Control.KeyDown, AddressOf Control_KeyDown
    End Sub
#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls
    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
                'LiveWorker.Dispose()
                HideTimer.Dispose()
                LiveBox.Dispose()

                LiveBoxControls.Clear()
            End If
            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
        End If
        disposedValue = True
    End Sub
    ' TODO: override Finalize() only if Dispose(disposing As Boolean) above has code to free unmanaged resources.
    'Protected Overrides Sub Finalize()
    '    ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub
    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(True)
        ' TODO: uncomment the following line if Finalize() is overridden above.
        ' GC.SuppressFinalize(Me)
    End Sub
#End Region
End Class

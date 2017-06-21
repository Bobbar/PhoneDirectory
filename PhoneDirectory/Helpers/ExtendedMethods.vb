﻿Imports System
Imports System.Reflection
Imports System.Windows.Forms
Imports System.Runtime.CompilerServices
Imports System.Data.Common
Module ExtendedMethods
    Public Sub DoubleBuffered(ByVal dgv As DataGridView, ByVal setting As Boolean)
        Dim dgvType As Type = dgv.[GetType]()
        Dim pi As PropertyInfo = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance Or BindingFlags.NonPublic)
        pi.SetValue(dgv, setting, Nothing)
    End Sub
    Public Sub DoubleBufferedListView(ByVal dgv As ListView, ByVal setting As Boolean)
        Dim dgvType As Type = dgv.[GetType]()
        Dim pi As PropertyInfo = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance Or BindingFlags.NonPublic)
        pi.SetValue(dgv, setting, Nothing)
    End Sub
    Public Sub DoubleBufferedListBox(ByVal dgv As ListBox, ByVal setting As Boolean)
        Dim dgvType As Type = dgv.[GetType]()
        Dim pi As PropertyInfo = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance Or BindingFlags.NonPublic)
        pi.SetValue(dgv, setting, Nothing)
    End Sub

    ''' <summary>
    ''' Adds a parameter to the command.
    ''' </summary>
    ''' <param name="command">
    ''' The command.
    ''' </param>
    ''' <param name="parameterName">
    ''' Name of the parameter.
    ''' </param>
    ''' <param name="parameterValue">
    ''' The parameter value.
    ''' </param>
    ''' <remarks>
    ''' </remarks>
    <System.Runtime.CompilerServices.Extension>
    Public Sub AddParameterWithValue(command As DbCommand, parameterName As String, parameterValue As Object)
        Dim parameter = command.CreateParameter()
        parameter.ParameterName = parameterName
        parameter.Value = parameterValue
        command.Parameters.Add(parameter)
    End Sub

End Module

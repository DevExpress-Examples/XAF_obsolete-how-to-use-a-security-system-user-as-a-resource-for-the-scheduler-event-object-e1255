Imports DevExpress.ExpressApp.Model
Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.Xpo
Imports System.Security
Imports System.Diagnostics
Imports System.ComponentModel
Imports DevExpress.Persistent.Base
Imports DevExpress.Persistent.Base.Security

Namespace WinWebSolution.Module
    <ImageName("BO_Security")> _
    Public Class PersistentPermissionObject
        Inherits BasePersistentObject
        Implements IPersistentPermission, IXpoCloneable
        Private _Group As Group
        Private _Permission As IPermission
        Public Sub New(ByVal session As Session)
            Me.New(session, Nothing)
        End Sub
        Public Sub New(ByVal session As Session, ByVal permission As IPermission)
            MyBase.New(session)
            _Permission = permission
        End Sub
        Public Overrides Function ToString() As String
            If _Permission IsNot Nothing Then
                Return CType(_Permission, Object).ToString()
            Else
                Return "Permission is null"
            End If
        End Function
        Public Shared Function GetPermissionFromXml(ByVal permissionXml As String) As IPermission
            Try
                If (Not String.IsNullOrEmpty(permissionXml)) Then
                    Dim securityElement As SecurityElement = SecurityElement.FromString(permissionXml)
                    Dim typeName As String = securityElement.Attribute("class")
                    'string assemblyName = securityElement.Attribute("assembly");
                    Dim result As IPermission = CType(ReflectionHelper.CreateObject(typeName), IPermission)
                    result.FromXml(securityElement)
                    Return result
                End If
            Catch e As Exception
                Tracing.Tracer.LogError(e)
            End Try
            Return Nothing
        End Function
        <Size(4000), Browsable(False)> _
        Public Property SerializedPermission() As String
            Get
                If _Permission IsNot Nothing Then
                    Return _Permission.ToXml().ToString()
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                Try
                    SetPropertyValue("Permission", _Permission, GetPermissionFromXml(value))
                Catch e As Exception
                    Trace.WriteLine(e.ToString())
                End Try
            End Set
        End Property
        <Association("Group-PersistentPermissionObjects")> _
        Public Property Group() As Group
            Get
                Return _Group
            End Get
            Set(ByVal value As Group)
                SetPropertyValue("Group", _Group, value)
            End Set
        End Property
        <ModelDefault("PropertyEditorType", "DevExpress.ExpressApp.Editors.DetailPropertyEditor")> _
        Public ReadOnly Property Permission() As IPermission
            Get
                Return _Permission
            End Get
        End Property
#Region "IPersistentPermission Members"
        Private Property IPersistentPermission_Permission() As IPermission Implements IPersistentPermission.Permission
            Get
                Return _Permission
            End Get
            Set(ByVal value As IPermission)
                _Permission = value
            End Set
        End Property
        Private Function CloneTo(ByVal targetType As Type) As IXPSimpleObject Implements IXpoCloneable.CloneTo
            If (Not GetType(PersistentPermissionObject).IsAssignableFrom(targetType)) Then
                Return Nothing
            End If
            Dim result As PersistentPermissionObject = CType(ReflectionHelper.CreateObject(Me.GetType(), Session), PersistentPermissionObject)
            result.Group = Group
            result.SerializedPermission = SerializedPermission
            Return result
        End Function
#End Region
    End Class
End Namespace
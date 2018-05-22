Imports System
Imports DevExpress.ExpressApp
Imports DevExpress.Xpo.Metadata
Imports DevExpress.Xpo
Imports DevExpress.Persistent.BaseImpl

Namespace WinWebSolution.Module
	Public NotInheritable Partial Class WinWebSolutionModule
		Inherits ModuleBase
		Public Sub New()
			InitializeComponent()
		End Sub
		Public Overrides Sub CustomizeXPDictionary(ByVal xpDictionary As XPDictionary)
			MyBase.CustomizeXPDictionary(xpDictionary)
			Dim personClassInfo As XPClassInfo = XafTypesInfo.XpoTypeInfoSource.XPDictionary.GetClassInfo(GetType(Person))
			Dim personFullNameMemberInfo As XPMemberInfo = personClassInfo.GetMember("FullName")
			Dim persistentAliasAttribute As PersistentAliasAttribute = TryCast(personFullNameMemberInfo.FindAttributeInfo(GetType(PersistentAliasAttribute)), PersistentAliasAttribute)
			If persistentAliasAttribute Is Nothing Then
				personFullNameMemberInfo.AddAttribute(New PersistentAliasAttribute("FirstName + MiddleName + LastName"))
			End If
		End Sub
	End Class
End Namespace
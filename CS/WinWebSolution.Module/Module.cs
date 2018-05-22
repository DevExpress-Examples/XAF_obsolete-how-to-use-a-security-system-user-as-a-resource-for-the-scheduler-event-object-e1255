using System;
using DevExpress.ExpressApp;
using DevExpress.Xpo.Metadata;
using DevExpress.Xpo;
using DevExpress.Persistent.BaseImpl;

namespace WinWebSolution.Module {
    public sealed partial class WinWebSolutionModule : ModuleBase {
        public WinWebSolutionModule() {
            InitializeComponent();
        }
        public override void CustomizeXPDictionary(XPDictionary xpDictionary) {
            base.CustomizeXPDictionary(xpDictionary);
            XPClassInfo personClassInfo = XafTypesInfo.XpoTypeInfoSource.XPDictionary.GetClassInfo(typeof(Person));
            XPMemberInfo personFullNameMemberInfo = personClassInfo.GetMember("FullName");
            PersistentAliasAttribute persistentAliasAttribute =
                personFullNameMemberInfo.FindAttributeInfo(typeof(PersistentAliasAttribute)) as PersistentAliasAttribute;
            if (persistentAliasAttribute == null) {
                personFullNameMemberInfo.AddAttribute(new PersistentAliasAttribute("FirstName + MiddleName + LastName"));
            }
        }
    }
}
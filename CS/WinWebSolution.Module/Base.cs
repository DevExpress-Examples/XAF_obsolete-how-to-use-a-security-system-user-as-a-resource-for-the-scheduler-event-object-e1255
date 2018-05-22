using System;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.Xpo.Metadata;

namespace WinWebSolution.Module {
    public interface IPerson {
        DateTime Birthday { get; set; }
        string Email { get; set; }
        string FirstName { get; set; }
        string FullName { get; }
        string LastName { get; set; }
        string MiddleName { get; set; }
    }
    [NonPersistent]
    public class BasePersistentObject : XPCustomObject {
        public BasePersistentObject(Session session) : base(session) { }
        private bool isDefaultPropertyAttributeInit;
        private XPMemberInfo defaultPropertyMemberInfo;
        [Persistent("Oid"), Key(true), Browsable(false), MemberDesignTimeVisibility(false)]
        private Guid _Oid = Guid.Empty;
        [PersistentAlias("_Oid"), Browsable(false)]
        public Guid Oid { get { return _Oid; } }
        protected override void OnSaving() {
            base.OnSaving();
            if (!(Session is NestedUnitOfWork) && Session.IsNewObject(this))
                _Oid = XpoDefault.NewGuid();
        }
        public override string ToString() {
            if (!isDefaultPropertyAttributeInit) {
                DefaultPropertyAttribute attrib = XafTypesInfo.Instance.FindTypeInfo(GetType()).FindAttribute<DefaultPropertyAttribute>();
                if (attrib != null)
                    defaultPropertyMemberInfo = ClassInfo.FindMember(attrib.Name);
                isDefaultPropertyAttributeInit = true;
            }
            if (defaultPropertyMemberInfo != null) {
                object obj = defaultPropertyMemberInfo.GetValue(this);
                if (obj != null)
                    return obj.ToString();
            }
            return base.ToString();
        }
    }
}
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.Security;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace WinWebSolution.Module {
    [DefaultProperty("Name")]
    public class Group : RoleBase, IRole {
        public const string DefaultAdministratorsGroupName = "Administrators";
        public const string DefaultUsersGroupName = "Users";

        public Group(Session session) : base(session) { }
        [Association("User-Role", UseAssociationNameAsIntermediateTableName = true)]
        public XPCollection<Employee> Employees {
            get { return GetCollection<Employee>("Employees"); }
        }
        IList<IUser> IRole.Users {
            get {
                return new ListConverter<IUser, Employee>(this.Employees);
            }
        }
    }
}

using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Security;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.Base.Security;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace WinWebSolution.Module {
    [NavigationItem]
    [DefaultProperty("UserName")]
    public class Employee : Person, IUserWithRoles, IAuthenticationActiveDirectoryUser, IAuthenticationStandardUser, IResource {
        private UserImpl user;
        private List<IPermission> permissions;
        [Persistent("Color")]
        private int _Color;
        private string _Caption;
        public Employee(Session session)
            : base(session) {
            this.permissions = new List<IPermission>();
            this.user = new UserImpl(this);
        }
        [Association("User-Role", UseAssociationNameAsIntermediateTableName = true)]
        [RuleRequiredField("RuleRequiredField for IUserWithRoles.Roles", DefaultContexts.Save, TargetCriteria="IsActive=True", CustomMessageTemplate="You cannot save an Active employee without Groups")]
        public XPCollection<Group> Groups {
            get { return GetCollection<Group>("Groups"); }
        }
        public string UserName {
            get {
                return this.user.UserName;
            }

            set {
                this.user.UserName = value;
                OnChanged("UserName");
            }
        }
        public bool ChangePasswordOnFirstLogon {
            get {
                return this.user.ChangePasswordAfterLogon;
            }

            set {
                this.user.ChangePasswordAfterLogon = value;
                OnChanged("ChangePasswordOnFirstLogon");
            }
        }
        public bool IsActive {
            get {
                return this.user.IsActive;
            }

            set {
                this.user.IsActive = value;
                OnChanged("IsActive");
            }
        }
        public IList<IPermission> Permissions {
            get {
                this.permissions.Clear();
                foreach (Group role in this.Groups) {
                    this.permissions.AddRange(role.Permissions);
                }

                return this.permissions.AsReadOnly();
            }
        }
        IList<IRole> IUserWithRoles.Roles {
            get {
                return new ListConverter<IRole, Group>(this.Groups);
            }
        }
        public string Caption {
            get {
                return this._Caption;
            }

            set {
                SetPropertyValue("Caption", ref this._Caption, value);
            }
        }
        [NonPersistent, Browsable(false)]
        public object Id {
            get { return this.Oid; }
        }
        [NonPersistent, Browsable(false)]
        public int OleColor {
            get { return ColorTranslator.ToOle(Color.FromArgb(this._Color)); }
        }
        [NonPersistent]
        public Color Color {
            get {
                return Color.FromArgb(this._Color);
            }

            set {
                SetPropertyValue("Color", ref this._Color, value.ToArgb());
            }
        }
        [Association("Activity-Employees", UseAssociationNameAsIntermediateTableName = true)]
        public XPCollection<Activity> Activities {
            get { return GetCollection<Activity>("Activities"); }
        }
        [Persistent]
        private string StoredPassword {
            get {
                return this.user.StoredPassword;
            }

            set {
                this.user.StoredPassword = value;
                OnChanged("StoredPassword");
            }
        }
        public void SetPassword(string password) {
            this.user.SetPassword(password);
        }
        public void ReloadPermissions() {
            this.Groups.Reload();
            foreach (Group role in this.Groups) {
                role.PersistentPermissions.Reload();
            }
        }
        public bool ComparePassword(string password) {
            return this.user.ComparePassword(password);
        }
        public override void AfterConstruction() {
            base.AfterConstruction();
            this._Color = Color.White.ToArgb();
        }
    }
}
using System;
using System.Xml;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using DevExpress.ExpressApp;
using System.ComponentModel;

namespace WinWebSolution.Module {
    [DefaultClassOptions]
    public class Activity : BaseObject, IEvent, ISupportRecurrences {
        private bool _AllDay;
        private string _Description;
        private DateTime _StartOn;
        private DateTime _EndOn;
        private int _Label;
        private string _Location;
        private int _Status;
        private string _Subject;
        private int _Type;
        [Persistent("ResourceIds"), Size(SizeAttribute.Unlimited), ObjectValidatorIgnoreIssue(typeof(ObjectValidatorLargeNonDelayedMember))]
        private string _EmployeeIds;
        public Activity(Session session)
            : base(session) {
        }
        public bool AllDay {
            get {
                return this._AllDay;
            }

            set {
                SetPropertyValue("AllDay", ref this._AllDay, value);
            }
        }
        [NonPersistent]
        public string AppointmentId {
            get { return this.Oid.ToString(); }
        }
        [Size(SizeAttribute.Unlimited), ObjectValidatorIgnoreIssue(typeof(ObjectValidatorLargeNonDelayedMember))]
        public string Description {
            get {
                return this._Description;
            }

            set {
                SetPropertyValue("Description", ref this._Description, value);
            }
        }
        public int Label {
            get {
                return this._Label;
            }

            set {
                SetPropertyValue("Label", ref this._Label, value);
            }
        }
        public string Location {
            get {
                return this._Location;
            }

            set {
                SetPropertyValue("Location", ref this._Location, value);
            }
        }
        [Browsable(false)]
        [PersistentAlias("_EmployeeIds")]
        public string ResourceId {
            get {
                if (this._EmployeeIds == null) {
                    this.UpdateEmployeeIds();
                }
                return this._EmployeeIds;
            }
            set {
                if (_EmployeeIds != value && value != null) {
                    this._EmployeeIds = value;
                    this.UpdateEmployees();
                }
            }
        }
        [Indexed]
        public DateTime StartOn {
            get {
                return this._StartOn;
            }

            set {
                SetPropertyValue("StartOn", ref this._StartOn, value);
            }
        }
        [Indexed]
        public DateTime EndOn {
            get {
                return this._EndOn;
            }

            set {
                SetPropertyValue("EndOn", ref this._EndOn, value);
            }
        }
        public int Status {
            get {
                return this._Status;
            }

            set {
                SetPropertyValue("Status", ref this._Status, value);
            }
        }
        [Size(250)]
        public string Subject {
            get {
                return this._Subject;
            }

            set {
                SetPropertyValue("Subject", ref this._Subject, value);
            }
        }
        [Browsable(false)]
        public int Type {
            get {
                return this._Type;
            }

            set {
                SetPropertyValue("Type", ref this._Type, value);
            }
        }
        [Association("Activity-Employees", typeof(Employee), UseAssociationNameAsIntermediateTableName = true)]
        public XPCollection Employees {
            get { return GetCollection("Employees"); }
        }
        public override void AfterConstruction() {
            base.AfterConstruction();
            this.StartOn = DateTime.Now;
            this.EndOn = this.StartOn.AddHours(1);
            this.Employees.Add(Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId));
        }
        public void UpdateEmployeeIds() {
            this._EmployeeIds = string.Empty;
            this.Employees.SuspendChangedEvents();
            try {
                foreach (Employee activityUser in this.Employees) {
                    this._EmployeeIds += String.Format(@"<ResourceId Type=""{0}"" Value=""{1}"" />", activityUser.Id.GetType().FullName, activityUser.Id);
                }
            } finally {
                this.Employees.ResumeChangedEvents();
            }
            this._EmployeeIds = "<ResourceIds>" + this._EmployeeIds + "</ResourceIds>";
        }
        protected override XPCollection CreateCollection(XPMemberInfo property) {
            XPCollection result = base.CreateCollection(property);
            if (property.Name == "Employees") {
                result.CollectionChanged += new XPCollectionChangedEventHandler(this.Employees_CollectionChanged);
            }

            return result;
        }
        private void UpdateEmployees() {
            this.Employees.SuspendChangedEvents();
            try {
                while (this.Employees.Count > 0) {
                    this.Employees.Remove(this.Employees[0]);
                }
                if (!String.IsNullOrEmpty(this._EmployeeIds)) {
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(this._EmployeeIds);
                    foreach (XmlNode xmlNode in xmlDocument.DocumentElement.ChildNodes) {
                        Employee activityUser = Session.GetObjectByKey<Employee>(new Guid(xmlNode.Attributes["Value"].Value));
                        if (activityUser != null) {
                            this.Employees.Add(activityUser);
                        }
                    }
                }
            } finally {
                this.Employees.ResumeChangedEvents();
            }
        }
        private void Employees_CollectionChanged(object sender, XPCollectionChangedEventArgs e) {
            if ((e.CollectionChangedType == XPCollectionChangedType.AfterAdd) || (e.CollectionChangedType == XPCollectionChangedType.AfterRemove)) {
                this.UpdateEmployeeIds();
                OnChanged("ResourceId");
            }
        }
        private string recurrenceInfoXml;
        [DevExpress.Xpo.DisplayName("Recurrence"), Size(SizeAttribute.Unlimited), ObjectValidatorIgnoreIssue(typeof(ObjectValidatorLargeNonDelayedMember))]
        public string RecurrenceInfoXml {
            get {
                return recurrenceInfoXml;
            }
            set {
                recurrenceInfoXml = value;
                OnChanged("RecurrenceInfoXml");
            }
        }
        [Persistent("RecurrencePattern")]
        private Activity recurrencePattern;
        public IEvent RecurrencePattern {
            get {
                return recurrencePattern;
            }
            set {
                recurrencePattern = (Activity)value;
                OnChanged("RecurrencePattern");
            }
        }
    }
}
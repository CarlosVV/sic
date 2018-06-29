namespace Nagnoi.SiC.Domain.Core.Model {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Serialization;

    public class User {
        public int UserId { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string LoginName { get; set; }
        public string DocumentNumber { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime RegistrationDate { get; set; }

        [XmlIgnore]
        public string FullName {
            get {
                if (!string.IsNullOrEmpty(this.GivenName) && !string.IsNullOrEmpty(this.FamilyName)) {
                    return string.Format("{0} {1}", this.GivenName, this.FamilyName);
                }
                else {
                    return this.LoginName;
                }
            }
        }

        public DateTime LastLoginDate { get; set; }
    }
}

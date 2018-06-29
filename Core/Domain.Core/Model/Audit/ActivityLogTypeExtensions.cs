using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nagnoi.SiC.Domain.Core.Model {
    public partial class ActivityLogType : ICloneable{

        public object Clone() {
            ActivityLogType clonedActivityLogType = new ActivityLogType();

            clonedActivityLogType.ActivityLogTypeId = this.ActivityLogTypeId;
            clonedActivityLogType.SystemKeyword = this.SystemKeyword;
            clonedActivityLogType.Name = this.Name;
            clonedActivityLogType.IsEnabled = this.IsEnabled;
            clonedActivityLogType.CreatedBy = this.CreatedBy;
            clonedActivityLogType.CreatedDateTime = this.CreatedDateTime;
            clonedActivityLogType.ModifiedBy = this.ModifiedBy;
            clonedActivityLogType.ModifiedDateTime = this.ModifiedDateTime;

            return clonedActivityLogType;
        }

       
    }

    public static class ActivityLogTypeExtensions {
        public static string BuildXml(this IEnumerable<ActivityLogType> activityLogTypes) {
               
            if (activityLogTypes == null || activityLogTypes.Count() == 0) {
                return string.Empty;
            }

            StringBuilder result = new StringBuilder();

            result.Append("<LogTypes>");
            foreach (ActivityLogType activityLogType in activityLogTypes) {
                result.AppendFormat("<LogType id='{0}'", activityLogType.ActivityLogTypeId);

                result.AppendFormat(" ie='{0}'", activityLogType.IsEnabled);

                result.Append("></LogType>");
            }
            result.Append("</LogTypes>");

            return result.ToString();
        }
    }
}

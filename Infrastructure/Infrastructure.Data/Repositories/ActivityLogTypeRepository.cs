namespace Nagnoi.SiC.Infrastructure.Data
{
    #region Referencias

    using System;
    using System.Collections;
    using System.Data.SqlTypes;
    using System.Xml;
    using Nagnoi.SiC.Domain.Core.Model;
    using Nagnoi.SiC.Domain.Core.Repositories;

    #endregion

    public sealed class ActivityLogTypeRepository : EfRepository<ActivityLogType>, IActivityLogTypeRepository
    {
        public void SaveAll(string xmlActivityLogTypes, string logHostName, DateTime logLastDate) {
            Hashtable parameters = new Hashtable();
            SqlXml activityLogTypesXml = new SqlXml(new XmlTextReader(xmlActivityLogTypes, XmlNodeType.Document, null));
            parameters.Add("ACTIVITYLOGTYPES", activityLogTypesXml);
            parameters.Add("LOGHOSTNAME", logHostName);
            parameters.Add("LOGLASTDATE", logLastDate);
            //TODO: Save XML OBJECT
            //executeNonQuery(null, "aud_ActivityLogType_SaveAll", false, parameters);
        }
    }
}
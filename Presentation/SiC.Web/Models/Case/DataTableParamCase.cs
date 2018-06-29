namespace Nagnoi.SiC.Web.Models.Case
{
    #region References

    using System;
    using Paging;
    using Infrastructure.Core.Helpers;

    #endregion

    public class DataTableParamCase : DataTableParam
    {
        #region Private Members

        private string caseNumber = null;

        #endregion

        #region Properties

        public string CaseNumber
        {
            get { return caseNumber; }
            set
            {
                caseNumber = value;

                EnsureCaseNumberAndCaseKey();
            }
        }

        public string CaseKey { get; set; }
        
        public string EntityName { get; set; }
        
        public string SocialSecurityNumber { get; set; }
        
        public DateTime? BirthDate { get; set; }
        
        public DateTime? FilingDate { get; set; }
        
        public string EBTNumber { get; set; }
        
        public int? RegionId { get; set; }
        
        public int? ClinicId { get; set; }

        #endregion

        #region Public Methods

        public void EnsureCaseNumberAndCaseKey()
        {
            if (string.IsNullOrEmpty(caseNumber))
            {
                return;
            }
            else if (caseNumber.Match(@"^[0-9]{1,11} [0-9]{2}$"))
            {
                string[] caseNumberIncludingCaseKey = caseNumber.Split(new char[] { ' ' });

                caseNumber = caseNumberIncludingCaseKey[0];
                CaseKey = caseNumberIncludingCaseKey[1];
            }
            else if (CaseNumber.Match(@"^[0-9]{1,11}$"))
            {
                CaseKey = null;
            }
            else
            {
                CaseKey = null;
            }
        }

        #endregion
    }
}
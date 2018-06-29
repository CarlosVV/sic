namespace Nagnoi.SiC.Web.Models.Approval
{
    #region References

    using System;

    #endregion

    public class SearchViewModel
    {
        #region Properties

        public string CaseNumber { get; set; }

        public string EntityName { get; set; }

        public string SocialSecurityNumber { get; set; }

        public DateTime? BirthDate { get; set; }

        public DateTime? FilingDate { get; set; }

        public int? RegionId { get; set; }

        public int? DispensaryId { get; set; }

        public DateTime? From { get; set; }

        public DateTime? To { get; set; }

        public int? DocumentType { get; set; }

        public int? StatusId { get; set; }

        public string EBTNumber { get; set; }

        #endregion
    }
}
namespace Nagnoi.SiC.Domain.Core.Repositories
{
    #region Referencias

    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Nagnoi.SiC.Domain.Core.Model;

    #endregion

    public interface ICaseRepository : IRepository<Case>
    {
        IEnumerable<ResumenPagosPorBeneficiario_Result> ResumenPagosPorBeneficiario(string caseNumber);
        IEnumerable<ResumenPagosPorConcepto_Result> ResumenPagosPorConcepto(string caseNumber);
        IEnumerable<BuscarCasos_Result> BuscarCasos(string nombre, string sSN, string eBT, string numeroCaso);
        IEnumerable<InformacionCaso_Result> InformacionCaso(int? caseId);

        IEnumerable<Case> SearchCases(string caseNumber, string injuredName, string ssn, DateTime? dateOfBirth, DateTime? caseDate, int? regionId, int? clinicId, string accountEbt);

        Task<List<Case>> SearchCasesAsync(string caseNumber, string injuredName, string ssn, DateTime? dateOfBirth, DateTime? caseDate, int? regionId, int? clinicId, string accountEbt);

        Case GetCaseByNumber(string caseNumber);

        Task<Case> GetCaseByNumberAsync(string caseNumber);
        
        decimal? GetBalanceByCase(int? caseId);
        PaymentBalance GetBalanceDetailByCase(int? caseDetailId);
        
        bool ActualizarEstadoPagos(int caseid, int transactionid, int estado, string razon);
        bool ActualizarEstadoDieta(int caseid, int estado, string razon);

        decimal? GetTotalAdjudicationByCase(int? caseId);
        decimal? GetTotalAdjudicationByOtherCases(int? caseId);
        IEnumerable<Case> GetRelatedCasesByCaseNumber(string caseNumber);
        IEnumerable<Case> GetOtherRelatedCasesByCaseNumber(string caseNumber);
        bool AddPreexistingCase(string CaseNumber, string PreexistingCaseNumber);
        bool RemovePreexistingCase(string CaseNumber, string PreexistingCaseNumber);
        IEnumerable<RelatedCasesByCompensationRegion> GetRelatedCasesByCompensationRegion(string caseNumber);
        IEnumerable<Case> GetRelatedCasesUsedInDecision(string caseNumber);
    }
}
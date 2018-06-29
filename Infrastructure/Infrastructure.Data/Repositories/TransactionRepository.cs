namespace Nagnoi.SiC.Infrastructure.Data
{
    #region References

    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using Domain.Core.Model;
    using Domain.Core.Repositories;
    using System.Data.SqlClient;

    #endregion

    public sealed class TransactionRepository : EfRepository<Transaction>, ITransactionRepository
    {
        public IEnumerable<Transaction> Find(int? caseId, int? transactionTypeId, string caseNumber)
        {
            return this.Find(t => (!caseId.HasValue || t.CaseDetail.CaseId == caseId.Value) &&
                                  (string.IsNullOrEmpty(caseNumber) || t.CaseReference.CaseNumber == caseNumber) &&
                                  (!transactionTypeId.HasValue || t.TransactionTypeId == transactionTypeId),
                             t => t.CaseDetail, t => t.CaseReference, t => t.TransactionType, t => t.CaseDetail.Case);
        }

        public Transaction GetByCaseId(int caseDetailId, string invoiceNumber)
        {
            return this.FindOne(t => t.CaseDetail.CaseDetailId == caseDetailId && t.InvoiceNumber == invoiceNumber,
                                t => t.CaseDetail, t => t.CaseReference, t => t.TransactionType, t => t.CaseDetail.Case);
        }

        public Transaction GetByCaseId(int caseDetailId, string invoiceNumber, int conceptId, int transactionTypeId)
        {
            return this.FindOne(t => t.CaseDetail.CaseDetailId == caseDetailId 
                                    && t.InvoiceNumber == invoiceNumber
                                    && t.ConceptId == conceptId
                                    && t.TransactionTypeId == transactionTypeId,
                                t => t.CaseDetail, t => t.CaseReference, t => t.TransactionType, t => t.CaseDetail.Case);
        }

        public Transaction GetById(int transactionId)
        {
            return this.FindOne(t => t.TransactionId == transactionId, t => t.CaseDetail, t => t.CaseReference, 
                                t => t.TransactionType, t => t.CaseDetail.Case, t => t.Payments);
        }

        public IEnumerable<Transaction> FindApprovals(string EBTNumber,
            string caseNumber, string entityName, string ssn,
            DateTime? birthDate, DateTime? filingDate, int? regionId,
            int? dispensaryId, DateTime? fromTransactionDate,
            DateTime? toTransactionDate, int? transactionTypeId, int? paymentStatusId, int? ConceptId)
        {
            var query = Entities.AsQueryable();

            if (!string.IsNullOrEmpty(caseNumber))
            {
                query = query.Where(t => t.CaseDetail.Case.CaseNumber == caseNumber);
            }

            if (!string.IsNullOrEmpty(entityName))
            {
                query = query.Where(t => t.CaseDetail.Entity.FullName.Contains(entityName));
            }

            if (!string.IsNullOrEmpty(ssn))
            {
                ssn = ssn.Replace("-", "");
                query = query.Where(t => t.CaseDetail.Entity.SSN == ssn);
            }

            if (birthDate.HasValue)
            {
                query = query.Where(t => t.CaseDetail.Entity.BirthDate == birthDate.Value);
            }

            if (filingDate.HasValue)
            {
                query = query.Where(t => t.CaseDetail.Case.CaseDate == filingDate.Value);
            }

            if (regionId.HasValue)
            {
                query = query.Where(t => t.CaseDetail.Case.RegionId == regionId.Value);
            }

            if (dispensaryId.HasValue)
            {
                query = query.Where(t => t.CaseDetail.Case.ClinicId == dispensaryId.Value);
            }

            if (fromTransactionDate.HasValue)
            {
                query = query.Where(t => t.CreatedDateTime.Value > fromTransactionDate.Value);
            }

            if (toTransactionDate.HasValue)
            {
                query = query.Where(t => t.CreatedDateTime.Value < toTransactionDate.Value);
            }

            if (transactionTypeId.HasValue)
            {
                if(transactionTypeId.Value == (int)TransactionTypeEnum.AdjudicacionAdicional)
                    query = query.Where(t => t.TransactionTypeId.Value == transactionTypeId.Value || t.TransactionTypeId.Value == (int)TransactionTypeEnum.AdjudicacionInicial);
                else
                    query = query.Where(t => t.TransactionTypeId.Value == transactionTypeId.Value);
            }

            if (paymentStatusId.HasValue)
            {
                query = query.Where(t => t.Payments.Any(p => p.StatusId == paymentStatusId.Value));
            }

            if (ConceptId.HasValue) {
                query = query.Where(t => t.Payments.Where(p => p.ConceptId == ConceptId.Value).Any());
            }

            if (!string.IsNullOrEmpty(EBTNumber)) {
                query = query.Where(t => t.CaseDetail.EBTAccount == EBTNumber);
            }

            return query.AsNoTracking()
                        .Include(t => t.CaseDetail)
                        .Include(t => t.CaseDetail.Entity)
                        .Include(t => t.CaseDetail.EntityLawyer)
                        .Include(t => t.CaseDetail.RelationshipType)
                        .Include(t => t.Payments)
                        .Include(t => t.Payments.Select(p => p.Status))
                        .Include(t => t.Concept)
                        .Include(t => t.TransactionType)
                        .Include(t => t.Payments.Select(p => p.Remitter))
                        .Include(t => t.Payments.Select(p => p.Transaction));                        
        }

        public IEnumerable<Transaction> GetTransactionByConceptAndCasefolder(int CaseDetailId, String CaseKey, int concept)
        {
            var query = Entities.AsQueryable();

           //Execute only if it is casekey 0 
            if (CaseKey == "00")
            {
                query = query.Where(c => c.CaseDetail.CaseDetailId == CaseDetailId);
                //If the concept id is IPP ADD the remanente and post mortem 
                if (concept == 3)
                {
                    query = query.Where(c => c.ConceptId == concept || c.ConceptId == 7 || c.ConceptId == 9).Where(c => (c.TransactionTypeId == 1 || c.TransactionTypeId == 2));
                }

                if (concept == 5)
                {
                    query = query.Where(c => c.ConceptId == concept || c.ConceptId == 8 || c.ConceptId == 10).Where(c => (c.TransactionTypeId == 1 || c.TransactionTypeId == 2));
                }
            }else
            {
                query = query.Where(c => c.CaseDetail.CaseDetailId == 0);
            }

            return query.Include(c => c.CaseDetail)
                           .Include(c => c.CaseDetail.Payments)
                           .Include(c => c.CaseDetail.Payments.Select(x => x.Status))
                           .Include(c => c.CaseDetail.Payments.Select(x => x.Concept))
                           .Include(c => c.CaseDetail.Transactions);
        }

        public IEnumerable<Transaction> GetTransactionTypeInversionByCase(int CaseDetailId, String CaseKey)
        {
            var query = Entities.AsQueryable();

            if (CaseKey == "00")
            {
                query = query.Where(c => c.CaseDetail.CaseDetailId == CaseDetailId).Where(c => c.TransactionTypeId == 3);
            }
            else
            {
                query = query.Where(c => c.CaseDetail.CaseDetailId == CaseDetailId).Where(c => c.TransactionTypeId == 3);
            }
            return query.Include(c => c.CaseDetail)
                        .Include(c => c.TransactionType);
        }

        public IEnumerable<Transaction> FindCaseBeneficiariesByCaseId(int CaseDetailId, String CaseKey, int CaseId)
        {
            var query = Entities.AsQueryable();
            
            //If it is is the leasionado, then bring everithing
            if (CaseKey == "00")
            {
                query = query.Where(c => c.CaseDetail.Case.CaseId == CaseId);
            }
            else
            {
                query = query.Where(c => c.CaseDetail.CaseDetailId == CaseDetailId);
            }
            query = query.Where(c => c.CaseDetail.Entity.ParticipantTypeId == 4);
            //query = query.Where(c => c.TransactionTypeId == 4);
            query = query.Where(c => c.CaseDetail.Case.ConceptId == 4 || c.CaseDetail.Case.ConceptId == 7 || c.CaseDetail.Case.ConceptId == 8 || c.CaseDetail.Case.ConceptId == 9 || c.CaseDetail.Case.ConceptId == 10);

            return query.Include(c => c.CaseDetail)
                        .Include(c => c.CaseDetail.Case)
                        .Include(c => c.CaseDetail.Case.Concept)
                        .Include(c => c.CaseDetail.Entity)
                        .Include(c => c.CaseDetail.RelationshipType)
                        .Include(c => c.CaseDetail.Payments)
                        .Include(c => c.CaseDetail.Payments.Select(x => x.Status))
                        .Include(c => c.CaseDetail.Payments.Select(x => x.Concept))
;
        }

        public IEnumerable<Transaction> GetTransactionByCaseId(int caseid)
        {
            var query = Entities.AsQueryable();

            query = query.Where(c => c.CaseDetail.CaseId == caseid);

            return query.Include(c => c.CaseDetail)
                        .Include(c => c.TransactionType);

        }

        public IEnumerable<Transaction> GetTransactionByCaseIdWithEffect(int caseid, string Effect)
        {
            var query = Entities.AsQueryable();

            query = query.Where(c => c.CaseDetail.CaseId == caseid);
            query = query.Where(c => c.TransactionType.Effect == Effect);

            var r = query.Include(c => c.CaseDetail)
                        .Include(c => c.TransactionType);

            return r;

        }

        public IEnumerable<CompensationRegion2> GetCompensationRegion()
        {

            List<CompensationRegion2> result = this.Context.SqlQuery<CompensationRegion2>(
                                            @"EXEC	[WebApp].[GetCompensationRegion]"
                                         ).ToList();

            return result;
        }

        public IEnumerable<TransactionDetail2> InsertTransactionDetail(IEnumerable<TransactionDetail2> trans)
        {
            int i = 0;
            var re = new List<TransactionDetail2>();
            foreach(var t in trans){

                var r = this.Context.SqlQuery<TransactionDetail2>(
                                                            @"EXEC [WebApp].[InsertTransactionDetail] 
                                                            @TransactionId
                                                            ,@CompensationRegionId
                                                            ,@Percent
                                                            ,@Amount
                                                            ,'Nagnoi'",
                                                            new SqlParameter("TransactionId", t.TransactionId),
                                                            new SqlParameter("CompensationRegionId", t.CompensationRegionId),
                                                            new SqlParameter("Percent", t.Percent),
                                                            new SqlParameter("Amount", t.Amount)
                                                         ).ToList();

                re.Add(r[0]);
                //trans.ElementAt<TransactionDetail2>(i).TransactionDetailId = r[0].TransactionDetailId;
                i++;
            }

            return re;
        }

        public IEnumerable<TransactionDetail2> UpdateTransactionDetail(IEnumerable<TransactionDetail2> trans)
        {
            int i = 0;
            var re = new List<TransactionDetail2>();
            foreach (var t in trans)
            {

                var r = this.Context.SqlQuery<TransactionDetail2>(
                                                            @"EXEC [WebApp].[UpdateTransactionDetail]
                                                            @TransactionDetailId
                                                            ,@TransactionId
                                                            ,@CompensationRegionId
                                                            ,@Percent
                                                            ,@Amount
                                                            ,@Hidden
                                                            ,'Nagnoi'",
                                                            new SqlParameter("TransactionDetailId", t.TransactionDetailId),
                                                            new SqlParameter("TransactionId", t.TransactionId),
                                                            new SqlParameter("CompensationRegionId", t.CompensationRegionId),
                                                            new SqlParameter("Percent", t.Percent),
                                                            new SqlParameter("Amount", t.Amount),
                                                            new SqlParameter("Hidden", t.Hidden)
                                                         ).ToList();

                re.Add(r[0]);
                //trans.ElementAt<TransactionDetail2>(i).TransactionDetailId = r[i].TransactionDetailId;
                i++;
            }

            return re;
        }

        public IEnumerable<TransactionDetail2> GetTransactionDetail(int TransactionId)
        {
            var r = this.Context.SqlQuery<TransactionDetail2>("EXEC [WebApp].[GetTransactionDetail] @TransactionId", new SqlParameter("TransactionId", TransactionId)).ToList();

            return r;
        }

        public IEnumerable<Transaction> GetBeneficiaryDetail(int CaseDetailId, String CaseKey, int CaseId)
        {
            var query = Entities.AsQueryable();

            if (CaseKey == "00")
            {
                query = query.Where(c => c.CaseDetail.Case.CaseId == CaseId);
            }
            else
            {
                query = query.Where(c => c.CaseDetail.CaseDetailId == CaseDetailId);
            }

            query = query.Where(c => c.CaseDetail.Entity.ParticipantTypeId == 4);
            query = query.Where(c => c.ConceptId == 4 || c.ConceptId == 7 || c.ConceptId == 8 || c.ConceptId == 9 || c.ConceptId == 10);
            //query = query.Where(c => c.TransactionTypeId != 1);

            return query.Include(c => c.CaseDetail)
                        .Include(c => c.CaseDetail.Entity)
                        .Include(c => c.TransactionType);
         
        }
    }
}
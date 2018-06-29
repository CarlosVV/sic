namespace Nagnoi.SiC.Infrastructure.Data
{
    #region References

    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using Domain.Core.Model;
    using Domain.Core.Repositories;

    #endregion

    public sealed class PaymentRepository : EfRepository<Payment>, IPaymentRepository
    {
        public IEnumerable<Payment> Find(int? caseId, string caseNumber, int? beneficiaryId, int? transferTypeId, int? classId, int? statusId)
        {
            var query = Entities.AsQueryable();

            if (caseId.HasValue)
            {
                query = query.Where(p => p.CaseId == caseId.Value);
            }

            if (!string.IsNullOrEmpty(caseNumber))
            {
                query = query.Where(p => p.CaseNumber == caseNumber);
            }

            if (beneficiaryId.HasValue)
            {
                query = query.Where(p => p.CaseDetail.EntityId == beneficiaryId.Value);
            }

            if (transferTypeId.HasValue)
            {
                query = query.Where(p => p.TransferTypeId == transferTypeId.Value);
            }

            if (classId.HasValue)
            {
                query = query.Where(p => p.ClassId == classId.Value);
            }

            if (statusId.HasValue)
            {
                query = query.Where(p => p.StatusId == statusId.Value);
            }

            return query.AsNoTracking()
                        .Include(p => p.Case)
                        .Include(p => p.Status)
                        .Include(p => p.Concept)
                        .Include(p => p.Transaction)
                        .Include(p => p.CaseDetail)
                        .Include(p => p.CaseDetail.RelationshipType)
                        .Include(p => p.CaseDetail.Entity);
        }

        public IEnumerable<Payment> FindApprovals(string EBTNumber,
            string caseNumber, string entityName, string ssn,
            DateTime? birthDate, DateTime? filingDate, int? regionId,
            int? dispensaryId, DateTime? fromIssueDate,
            DateTime? toIssueDate, int? paymentStatusId, int? ConceptId)
        {
            var query = Entities.AsQueryable();

            var caseKey = string.Empty;

            if (!string.IsNullOrEmpty(caseNumber))
            {
                //if (caseKey.Length == 13) {
                //    query = query.Where(t => t.CaseDetail.CaseNumber == caseNumber && t.CaseDetail.CaseKey == caseNumber.Split(' ')[1]);
                //} else if (caseKey.Length == 11) { } else { }

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

            if (fromIssueDate.HasValue)
            {
                query = query.Where(t => t.CreatedDateTime.Value > fromIssueDate.Value);
            }

            if (toIssueDate.HasValue)
            {
                query = query.Where(t => t.CreatedDateTime.Value < toIssueDate.Value);
            }

            if (paymentStatusId.HasValue)
            {
                query = query.Where(p => p.StatusId == paymentStatusId.Value);
            }

            if (ConceptId.HasValue) {
                query = query.Where(p => p.ConceptId == ConceptId.Value);
            }

            if (!string.IsNullOrEmpty(EBTNumber)) {
                query = query.Where(t => t.CaseDetail.EBTAccount == EBTNumber);
            }

            return query;
            //return query.AsNoTracking()
            //return query.Include(t => t.Case)
            //            .Include(t => t.CaseDetail)
            //            .Include(t => t.CaseDetail.Entity)
            //            .Include(t => t.CaseDetail.RelationshipType)
            //            .Include(t => t.Concept)
            //            .Include(t => t.Status)
            //            .Include(t => t.Transaction)
            //            .Include(t => t.TransferType);
        }

        public IEnumerable<Payment> Find(int? caseDetailId, int? transactionId, int? conceptId, int? paymentClassId)
        {
            var query = Entities.AsQueryable();

            if (caseDetailId.HasValue)
            {
                query = query.Where(p => p.CaseDetailId == caseDetailId.Value);
            }

            if (transactionId.HasValue)
            {
                query = query.Where(p => p.TransactionId == transactionId);
            }

            if (conceptId.HasValue)
            {
                query = query.Where(p => p.ConceptId == conceptId.Value);
            }

            if (paymentClassId.HasValue)
            {
                query = query.Where(p => p.ClassId == paymentClassId.Value);
            }

            return query.Include(p => p.CaseDetail)
                        .Include(p => p.Class)
                        .Include(p => p.Concept)
                        .Include(p => p.Region)
                        .Include(p => p.Status)
                        .Include(p => p.Transaction)
                        .Include(p => p.TransferType);
        }

        public IEnumerable<Payment> Find(string caseNumber, string injuredName, string ssn, DateTime? dateOfBirth, DateTime? caseDate, int? regionId, int? clinicId, string accountEbt, int? transferTypeId, int? statusId, int? adjustmentStatusId, string adjustmentTypeId)
        {
            var query = Entities.AsQueryable();

            if (!string.IsNullOrEmpty(caseNumber))
            {
                query = query.Where(p => p.CaseNumber == caseNumber);
            }

            if (!string.IsNullOrEmpty(injuredName))
            {
                query = query.Where(p => p.CaseDetail.Entity.FullName.Contains(injuredName));
            }

            if (!string.IsNullOrEmpty(ssn))
            {
                query = query.Where(p => p.CaseDetail.Entity.SSN == ssn);
            }

            if (dateOfBirth.HasValue)
            {
                query = query.Where(p => p.CaseDetail.Entity.BirthDate == dateOfBirth.Value);
            }

            if (caseDate.HasValue)
            {
                query = query.Where(p => p.Case.CaseDate == caseDate.Value);
            }

            if (regionId.HasValue)
            {
                query = query.Where(p => p.Case.RegionId == regionId.Value);
            }

            if (clinicId.HasValue)
            {
                query = query.Where(p => p.Case.ClinicId == clinicId.Value);
            }

            if (!string.IsNullOrEmpty(accountEbt))
            {
                query = query.Where(p => p.CaseDetail.EBTAccount == accountEbt);
            }

            if (transferTypeId.HasValue)
            {
                query = query.Where(p => p.TransferTypeId == transferTypeId.Value);
            }

            if (statusId.HasValue)
            {
                query = query.Where(p => p.StatusId == statusId.Value);
            }

            if (!string.IsNullOrEmpty(adjustmentTypeId))
            {
                query = query.Where(p => p.AdjustmentType.Equals(adjustmentTypeId, StringComparison.OrdinalIgnoreCase));
            }

            if (adjustmentStatusId.HasValue)
            {
                query = query.Where(p => p.AdjustmentStatusId == adjustmentStatusId.Value);
            }

            return query.AsNoTracking()
                        .Include(p => p.Case)
                        .Include(p => p.Status)
                        .Include(p => p.Concept)
                        .Include(p => p.Transaction)
                        .Include(p => p.AdjustmentStatus)
                        .Include(p => p.CaseDetail)
                        .Include(p => p.CaseDetail.Entity);
        }

        public IEnumerable<Payment> SearchPeremptories(int? caseId, int? caseDetailId, int[] statusIds)
        {
            var query = Entities.AsQueryable();

            if (caseId.HasValue)
            {
                query = query.Where(p => p.CaseId == caseId);
            }
            else if (caseDetailId.HasValue)
            {
                query = query.Where(p => p.CaseDetailId == caseDetailId);
            }

            query = query.Where(p => p.ClassId == 2 && (p.StatusId.HasValue && statusIds.Contains(p.StatusId.Value)));

            return query.AsNoTracking()
                        .Include(p => p.Case)
                        .Include(p => p.Status)
                        .Include(p => p.Concept)
                        .Include(p => p.Transaction)
                        .Include(p => p.CaseDetail)
                        .Include(p => p.CaseDetail.RelationshipType)
                        .Include(p => p.CaseDetail.Entity);
        }

        public IEnumerable<Payment> SearchInvestments(int? caseId, int? caseDetailId, int[] statusIds)
        {
            var query = Entities.AsQueryable();

            if (caseId.HasValue)
            {
                query = query.Where(p => p.CaseId == caseId);
            }
            else if (caseDetailId.HasValue)
            {
                query = query.Where(p => p.CaseDetailId == caseDetailId);
            }

            query = query.Where(p => p.ClassId == 3 && (p.StatusId.HasValue && statusIds.Contains(p.StatusId.Value)));

            return query.AsNoTracking()
                        .Include(p => p.Case)
                        .Include(p => p.Status)
                        .Include(p => p.Concept)
                        .Include(p => p.Transaction)
                        .Include(p => p.CaseDetail)
                        .Include(p => p.CaseDetail.RelationshipType)
                        .Include(p => p.CaseDetail.Entity);
        }

        public Payment GetById(int paymentId)
        {
            return Entities
                           .Include(c => c.CaseDetail)
                           .Include(c => c.CaseDetail.Entity)
                           .Include(c => c.Transaction)
                           .Include(c => c.Case)
                           .Include(c => c.Status)
                           .Include(c => c.Concept)
                           .Include(c => c.AdjustmentStatus)
                           .Where(c => c.PaymentId == paymentId)
                           .FirstOrDefault();
        }

        public override void Insert(Payment entity)
        {
            int nextPaymentId = EntitiesNoTracking.Max(p => p.PaymentId) + 1;

            entity.PaymentId = nextPaymentId;

            base.Insert(entity);
        }

        public IEnumerable<Payment> FindCertifications(string caseNumber, string caseKey = null)
        {
            if (string.IsNullOrEmpty(caseNumber))
            {
                return Enumerable.Empty<Payment>();
            }

            var exceptionConceptList = new List<string>(new string[] { "Dieta" });
            var exceptionStatusList = new List<string>(new string[] { "Certificado" });

            if (string.IsNullOrEmpty(caseKey))
            {
                var paymentCertification = (from i in this.Context.Payments
                                           .Include(u => u.Status)
                                           .Include(u => u.Concept)
                                            where (i.CaseNumber == caseNumber) && exceptionConceptList.Contains(i.Concept.Concept1) && i.Status.Status1.Equals("Generado")
                                            select new
                                            {
                                                PaymentId = i.PaymentId,
                                                CaseNumber = i.CaseNumber,
                                                CaseKey = i.CaseKey,
                                                Concept = i.Concept,
                                                Status = i.Status,
                                                FromDate = i.FromDate,
                                                ToDate = i.ToDate,
                                                TransactionNum = i.TransactionNum,
                                                CaseDetailId = i.CaseDetailId,
                                                Amount = i.Amount
                                            })
                                            .ToList()
                                            .Select(x => new Payment
                                            {
                                                PaymentId = x.PaymentId,
                                                CaseNumber = x.CaseNumber,
                                                CaseKey = x.CaseKey,
                                                Concept = x.Concept,
                                                Status = x.Status,
                                                FromDate = x.FromDate,
                                                ToDate = x.ToDate,
                                                TransactionNum = x.TransactionNum,
                                                CaseDetailId = x.CaseDetailId,
                                                Amount = x.Amount
                                            });

                return paymentCertification;
            }
            else
            {
                var paymentCertification = (from i in this.Context.Payments
                           .Include(u => u.Status)
                           .Include(u => u.Concept)
                                            where (i.CaseNumber == caseNumber && i.CaseKey == caseKey) && exceptionConceptList.Contains(i.Concept.Concept1) && i.Status.Status1 == "Generado"
                                            select new
                                            {
                                                PaymentId = i.PaymentId,
                                                CaseNumber = i.CaseNumber,
                                                CaseKey = i.CaseKey,
                                                Concept = i.Concept,
                                                Status = i.Status,
                                                FromDate = i.FromDate,
                                                ToDate = i.ToDate,
                                                TransactionNum = i.TransactionNum,
                                                CaseDetailId = i.CaseDetailId,
                                                Amount = i.Amount
                                            })
                            .ToList()
                            .Select(x => new Payment
                            {
                                PaymentId = x.PaymentId,
                                CaseNumber = x.CaseNumber,
                                CaseKey = x.CaseKey,
                                Concept = x.Concept,
                                Status = x.Status,
                                FromDate = x.FromDate,
                                ToDate = x.ToDate,
                                TransactionNum = x.TransactionNum,
                                CaseDetailId = x.CaseDetailId,
                                Amount = x.Amount
                            });

                return paymentCertification;
            }
        }

         public IEnumerable<Payment> PaymentQueries(string QueryName){
            var query = Entities.AsQueryable();

            if (QueryName == "PaymentGreaterThan500"){
                //Filter those who have diets with more than 500;
                query = query.Where(c => c.Amount > 500.00m);

                //Filter by those who have status aprobado, registrado, generado.
                //query = query.Where(c => c.StatusId == 4);
                query = query.Where(c => c.StatusId ==2);
            }

            if (QueryName == "NonCompliantPayment"){
                query = query.Where(c => c.StatusId == 2);
            }

            //Bring home the result
            return query.Include(c => c.Case)
                       .Include(c => c.Status)
                       .Include(c => c.CaseDetail)
                       .Include(c => c.CaseDetail.Entity);
        }

         public IEnumerable<Payment> FindCasesDormantExpunged()
         {
             var query = Entities.AsQueryable();

             //Filter to the the cases that are on Expunge or Dormant
             query.Where(c => c.CaseDetail.EBTStatus == "Dormant" || c.CaseDetail.EBTStatus == "Expunged");

             query = query.Where(c => c.StatusId == 3);

             //Bring home the result
             return query.Include(c => c.Case)
                        .Include(c => c.Concept)
                        .Include(c => c.Case.Region)
                        .Include(c => c.Case.Clinic)
                        .Include(c => c.CaseDetail)
                        .Include(c => c.CaseDetail.Entity);
         }
    }
}
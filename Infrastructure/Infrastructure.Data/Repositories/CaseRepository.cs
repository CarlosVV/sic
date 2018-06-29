namespace Nagnoi.SiC.Infrastructure.Data
{
    #region References

    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Helpers;
    using Domain.Core.Model;
    using Domain.Core.Repositories;

    #endregion

    public sealed class CaseRepository : EfRepository<Case>, ICaseRepository
    {
        public IEnumerable<ResumenPagosPorBeneficiario_Result> ResumenPagosPorBeneficiario(string caseNumber)
        {
            var _casenumber = new SqlParameter { ParameterName = "CaseNumber", Value = caseNumber };
            return this.Context.SqlQuery<ResumenPagosPorBeneficiario_Result>(
            "exec WebApp.ResumenPagosPorBeneficiario @CaseNumber ", _casenumber).ToList();
        }

        public IEnumerable<ResumenPagosPorConcepto_Result> ResumenPagosPorConcepto(string caseNumber)
        {
            var _casenumber = new SqlParameter { ParameterName = "CaseNumber", Value = caseNumber };
            return this.Context.SqlQuery<ResumenPagosPorConcepto_Result>(
            "exec WebApp.ResumenPagosPorConcepto @CaseNumber ", _casenumber).ToList();
        }

        public IEnumerable<BuscarCasos_Result> BuscarCasos(string nombre, string sSN, string eBT, string numeroCaso)
        {

            var _nombre = !string.IsNullOrEmpty(nombre) ? new SqlParameter { ParameterName = "@Nombre", Value = nombre } :
                new SqlParameter { ParameterName = "@Nombre", SqlDbType = SqlDbType.NVarChar, Size = 50, SqlValue = DBNull.Value };

            var _ssn = !string.IsNullOrEmpty(sSN) ? new SqlParameter { ParameterName = "@SSN", Value = sSN } :
                new SqlParameter { ParameterName = "@SSN", SqlDbType = SqlDbType.NVarChar, Size = 50, SqlValue = DBNull.Value };

            var _ebt = !string.IsNullOrEmpty(eBT) ? new SqlParameter { ParameterName = "@EBT", Value = eBT } :
                new SqlParameter { ParameterName = "@EBT", SqlDbType = SqlDbType.NVarChar, Size = 50, SqlValue = DBNull.Value };

            var _numercaso = !string.IsNullOrEmpty(numeroCaso) ? new SqlParameter { ParameterName = "@NumeroCaso", Value = numeroCaso } :
                new SqlParameter { ParameterName = "@NumeroCaso", SqlDbType = SqlDbType.NVarChar, Size = 50, SqlValue = DBNull.Value };

            object[] parameters = new object[] { _nombre, _ssn, _ebt, _numercaso };


            return this.Context.SqlQuery<BuscarCasos_Result>(
            "exec WebApp.BuscarCasos @Nombre,@SSN,@EBT,@NumeroCaso ", parameters).ToList();
        }

        public IEnumerable<InformacionCaso_Result> InformacionCaso(int? caseId)
        {
            var _caseid = new SqlParameter { ParameterName = "@CaseId", Value = caseId };

            return this.Context.SqlQuery<InformacionCaso_Result>(
            "exec WebApp.InformacionCaso @CaseId ", _caseid).ToList();
        }

        public IEnumerable<Case> SearchCases(string caseNumber, string injuredName, string ssn, DateTime? dateOfBirth, DateTime? caseDate, int? regionId, int? clinicId, string accountEbt)
        {
            var query = Entities.AsQueryable();

            if (!string.IsNullOrEmpty(caseNumber))
            {
                query = query.Where(c => c.CaseNumber == caseNumber);
            }

            if (!string.IsNullOrEmpty(injuredName))
            {
                query = query.Where(c => c.CaseDetails.Any(d => d.Entity.FullName.Contains(injuredName)));
            }

            if (!string.IsNullOrEmpty(ssn))
            {
                query = query.Where(c => c.CaseDetails.Any(d => d.Entity.SSN == ssn));
            }

            if (dateOfBirth.HasValue)
            {
                query = query.Where(c => c.CaseDetails.Any(d => d.Entity.BirthDate == dateOfBirth));
            }

            if (caseDate.HasValue)
            {
                query = query.Where(c => c.CaseDate == caseDate);
            }

            if (regionId.HasValue)
            {
                query = query.Where(c => c.RegionId == regionId);
            }

            if (clinicId.HasValue)
            {
                query = query.Where(c => c.ClinicId == clinicId);
            }

            if (!string.IsNullOrEmpty(accountEbt))
            {
                query = query.Where(c => c.CaseDetails.Any(cd => cd.EBTAccount.Contains(accountEbt)));
            }

            return query.AsNoTracking()
                        .Include(c => c.CaseDetails.Select(d => d.Entity))
                        .Include(c => c.Region)
                        .Include(c => c.Clinic)
                        .Include(c => c.CaseDetails.Select(d => d.Entity.Addresses))
                        .Include(c => c.EmployerStatus)
                        .Include(c => c.CaseDetails);
        }

        public Task<List<Case>> SearchCasesAsync(string caseNumber, string injuredName, string ssn, DateTime? dateOfBirth, DateTime? caseDate, int? regionId, int? clinicId, string accountEbt)
        {
            var query = Entities.AsQueryable();

            if (!string.IsNullOrEmpty(caseNumber))
            {
                query = query.Where(c => c.CaseNumber == caseNumber);
            }

            if (!string.IsNullOrEmpty(injuredName))
            {
                query = query.Where(c => c.CaseDetails.Any(d => d.Entity.FullName.Contains(injuredName)));
            }

            if (!string.IsNullOrEmpty(ssn))
            {
                query = query.Where(c => c.CaseDetails.Any(d => d.Entity.SSN == ssn));
            }

            if (dateOfBirth.HasValue)
            {
                query = query.Where(c => c.CaseDetails.Any(d => d.Entity.BirthDate == dateOfBirth));
            }

            if (caseDate.HasValue)
            {
                query = query.Where(c => c.CaseDate == caseDate);
            }

            if (regionId.HasValue)
            {
                query = query.Where(c => c.RegionId == regionId);
            }

            if (clinicId.HasValue)
            {
                query = query.Where(c => c.ClinicId == clinicId);
            }

            if (!string.IsNullOrEmpty(accountEbt))
            {
                query = query.Where(c => c.CaseDetails.Any(cd => cd.EBTAccount.Contains(accountEbt)));
            }

            return query.AsNoTracking()
                        .Include(c => c.CaseDetails.Select(d => d.Entity))
                        .Include(c => c.Region)
                        .Include(c => c.Clinic)
                        .Include(c => c.CaseDetails.Select(d => d.Entity.Addresses))
                        .Include(c => c.EmployerStatus)
                        .Include(c => c.CaseDetails)
                        .ToListAsync();
        }

        public Case GetCaseByNumber(string caseNumber)
        {
            return FindOne(c => c.CaseNumber == caseNumber,
                           c => c.CaseDetails.Select(d => d.Entity), c => c.Region, c => c.Clinic, c => c.CaseDetails.Select(d => d.Entity.Addresses), c => c.EmployerStatus, c => c.CaseDetails);
        }

        public Task<Case> GetCaseByNumberAsync(string caseNumber)
        {
            return FindOneAsync(c => c.CaseNumber == caseNumber,
                                c => c.CaseDetails.Select(d => d.Entity), c => c.Region, c => c.Clinic, c => c.CaseDetails.Select(d => d.Entity.Addresses), c => c.EmployerStatus, c => c.CaseDetails);
        }
        
        public IEnumerable<Case> GetTransactionDetail(string caseNumber)
        {
            List<string> transactions = this.Context.SqlQuery<string>(
                                                                    "EXEC [WebApp].[GetRelatedCasesByCaseNumber] @CaseNumber",
                                                                    new SqlParameter("CaseNumber", caseNumber)
                                                                 ).ToList();
            return Enumerable.Empty<Case>();
        }

        public decimal? GetTotalAdjudicationByCase(int? caseId)
        {
            decimal? d = this.Context.SqlQuery<decimal?>(@"SELECT SUM(TransactionAmount) AS TransactionAmount 
                                                           FROM [SiC DB].[SiC].[vTransaction] 
                                                           WHERE CaseKey = '00' AND CaseId = @caseId
                                                           GROUP BY [CaseId]", new SqlParameter("caseId", caseId)).FirstOrDefault();
            return d == null ? 0 : d;
        }

        public decimal? GetTotalAdjudicationByOtherCases(int? caseId)
        {
            decimal? d = this.Context.SqlQuery<decimal?>(@"[WebApp].[GetTotalAdjudicationByOtherCases] @caseId", new SqlParameter("caseId", caseId)).FirstOrDefault();
            return d == null ? 0 : d;
        }

        public IEnumerable<Case> GetRelatedCasesByCaseNumber(string caseNumber)
        {
            List<string> casenumbers = this.Context.SqlQuery<string>(
                                                                    "EXEC [WebApp].[GetRelatedCasesByCaseNumber] @CaseNumber",
                                                                    new SqlParameter("CaseNumber", caseNumber)
                                                                 ).ToList();

            List<Case> cases = new List<Case>();
            foreach (string c in casenumbers)
            {
                cases.Add(GetCaseByNumber(c));
            }

            return cases;
        }

        public IEnumerable<Case> GetOtherRelatedCasesByCaseNumber(string caseNumber)
        {
            List<string> casenumbers = this.Context.SqlQuery<string>(
                                                                    "EXEC [WebApp].[GetOtherRelatedCasesByCaseNumber] @CaseNumber",
                                                                    new SqlParameter("CaseNumber", caseNumber)
                                                                 ).ToList();

            List<Case> cases = new List<Case>();
            foreach (string c in casenumbers)
            {

                cases.Add(GetCaseByNumber(c));
            }

            return cases;
        }

        public bool AddPreexistingCase(string CaseNumber, string PreexistingCaseNumber)
        {
            string user = "edwin";
            string query = string.Format("EXEC [WebApp].[AddPreexistingCase] @CaseNumber1=N'{0}',@CaseNumber2=N'{1}', @CreatedBy=N'{2}'", CaseNumber, PreexistingCaseNumber, user);

            int result = this.Context.ExecuteSqlCommand(query);

            return result == 1 ? true : false;
        }

        public bool RemovePreexistingCase(string CaseNumber, string PreexistingCaseNumber)
        {
            string user = "edwin";
            string query = String.Format("EXEC [WebApp].[RemovePreexistingCase] @CaseNumber1=N'{0}',@CaseNumber2=N'{1}', @CreatedBy=N'{2}'", CaseNumber, PreexistingCaseNumber, user);

            int result = this.Context.ExecuteSqlCommand(query);

            return result == 1 ? true : false;

        }

        public decimal? GetBalanceByCase(int? caseDetailId)
        {
            var caseDetailIdParameter = new SqlParameter()
            {
                ParameterName = "caseDetailId",
                Value = caseDetailId
            };

            return this.Context.SqlQuery<decimal?>("Select balance from SIC.vBalance where CaseDetailId = @caseDetailId ", caseDetailIdParameter).FirstOrDefault();
        }
        
        public bool ActualizarEstadoPagos(int caseId, int transactionId, int statusId, string reason)
        {
            var transactionNumber = transactionId.ToString().PadLeft(9, '0');
            var pagos = from p in this.Context.Payments
                           .Include(u => u.Status)
                        where p.CaseId == caseId &&
                              p.TransactionNum == transactionNumber
                        select p;

            foreach (var p in pagos.ToList())
            {
                var editpayment = this.Context.Payments.Find(p.PaymentId);
                editpayment.ModifiedBy = WebHelper.GetUserName();
                editpayment.ModifiedDateTime = DateTime.Now;
                editpayment.StatusChangeDate = DateTime.Now;
                editpayment.ModifiedDateTime = DateTime.Now;
                editpayment.StatusId = statusId;
                this.Context.Entry(editpayment).State = System.Data.Entity.EntityState.Modified;
                this.Context.SaveChanges();
            }

            var transaccion = this.Context.Transactions.Find(transactionId);
            transaccion.RejectedReason = reason;
            this.Context.SaveChanges();

            return true;
        }

        public bool ActualizarEstadoDieta(int caseid, int estado, string razon)
        {

            var pagos = from p in this.Context.Payments
                           .Include(u => u.Status)
                        where p.CaseId == caseid && p.ConceptId == 2 && p.ClassId == 11
                        select p;

            foreach (var p in pagos.ToList())
            {
                var editpayment = this.Context.Payments.Find(p.PaymentId);
                editpayment.ModifiedBy = WebHelper.GetUserName();
                editpayment.ModifiedDateTime = DateTime.Now;
                editpayment.StatusChangeDate = DateTime.Now;
                editpayment.ModifiedDateTime = DateTime.Now;
                editpayment.StatusId = estado;
                this.Context.Entry(editpayment).State = System.Data.Entity.EntityState.Modified;
                this.Context.SaveChanges();
            }

            this.Context.SaveChanges();
            return true;
        }

        public PaymentBalance GetBalanceDetailByCase(int? caseDetailId)
        {
            var caseDetailIdParameter = new SqlParameter()
            {
                ParameterName = "caseDetailId",
                Value = caseDetailId
            };

            var p = this.Context.SqlQuery<PaymentBalance>("Select CaseNumber, CaseKey, Balance AS Amount, CaseId, CaseDetailId, ConceptId, ClassId from SIC.vBalanceCertification where CaseDetailId = @caseDetailId and Balance <> 0", caseDetailIdParameter).FirstOrDefault();

            if (p != null)
            {
                p.Concept = (from c in this.Context.Concepts
                             where c.ConceptId == p.ConceptId
                             select c).FirstOrDefault();
            }
            return p;
        }

        public void InsertPaymentCertification(Payment payment, Transaction transaction, bool isDiet, bool transactionExist)
        {

            if (isDiet)
            {
                this.Context.Payments.Add(payment);
                this.Context.SaveChanges();
            }
            else
            {
                if (transactionExist)
                {
                    this.Context.Payments.Add(payment);
                    this.Context.Entry(transaction).State = System.Data.Entity.EntityState.Modified;
                    this.Context.SaveChanges();
                    //DbContextTransaction tran = null;

                    //try {
                    //    tran = this.Context.Database.BeginTransaction(System.Data.IsolationLevel.RepeatableRead);
                    //    this.Context.Payments.Add(payment);
                    //    this.Context.Entry(transaction).State = EntityState.Modified;

                    //    await this.Context.SaveChangesAsync();

                    //}
                    //catch(Exception e) { 
                    //    tran.Rollback(); 
                    //}
                    //finally { tran.Dispose(); }
                    //this.Context.Database.BeginTransaction();
                    //this.Context.Payments.Add(payment);


                    //bool saveFailed;
                    //do
                    //{
                    //    saveFailed = false;
                    //    try
                    //    {
                    //        this.Context.Entry(transaction).State = EntityState.Modified;
                    //        this.Context.SaveChanges();
                    //    }
                    //    catch (DbUpdateConcurrencyException e)
                    //    {
                    //        saveFailed = true;

                    //        var entry = e.Entries.Single();
                    //        entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                    //    }
                    //} while (saveFailed);
                    //this.Context.Database.
                }
                else
                {
                    this.Context.Payments.Add(payment);
                    this.Context.Transactions.Add(transaction);
                    this.Context.SaveChanges();
                }
            }
        }

        public IEnumerable<RelatedCasesByCompensationRegion> GetRelatedCasesByCompensationRegion(string caseNumber)
        {
            var r = this.Context.SqlQuery<RelatedCasesByCompensationRegion>(@"
                                                                        EXEC [WebApp].[GetRelatedCasesByCompensationRegion] @CaseNumber",
                                                                        new SqlParameter("CaseNumber", caseNumber)).ToList();
            return r;
        }

        public IEnumerable<Case> GetRelatedCasesUsedInDecision(string caseNumber)
        {
            var casenumbers = this.Context.SqlQuery<String>(@"
                                                   EXEC [WebApp].[GetRelatedCasesUsedInDecision] @CaseNumber",
                                                   new SqlParameter("CaseNumber", caseNumber)).ToList();

            List<Case> cases = new List<Case>();
            foreach (string c in casenumbers)
            {
                cases.Add(GetCaseByNumber(c));
            }

            return cases;
        }
    }
}
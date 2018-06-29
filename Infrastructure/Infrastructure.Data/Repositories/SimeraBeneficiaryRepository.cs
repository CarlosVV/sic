namespace Nagnoi.SiC.Infrastructure.Data
{
    #region Referencias

    using Nagnoi.SiC.Domain.Core.Model;
    using Nagnoi.SiC.Domain.Core.Repositories;
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    #endregion

    public sealed class SimeraBeneficiaryRepository : EfRepository<SimeraBeneficiary>, ISimeraBeneficiaryRepository
    {
        public SimeraBeneficiary InsertSimeraBeneficiary(SimeraBeneficiary entity)
        {
            this.Context.SimeraBeneficiary.Add(entity);
            this.Context.SaveChanges();

            return entity;
        }

        public void DeleteSimeraBeneficiary(SimeraBeneficiary entity)
        {
            this.Context.SimeraBeneficiary.Remove(entity);
            this.Context.SaveChanges();
        }

        public void DeleteCaseBeneficiaries(string caseNumber)
        {
            var entities = this.Context.SimeraBeneficiary.Where(x => x.CaseNumber == caseNumber);

            foreach (var entity in entities)
            {
                this.Context.SimeraBeneficiary.Remove(entity);
            }

            this.Context.SaveChanges();
        }
    }
}
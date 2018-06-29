namespace Nagnoi.SiC.Domain.Core.Model
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [Serializable]
    [Table("Entity.Beneficiary")]
    public class Beneficiary : ICloneable
    {
        public int BeneficiaryId { get; set; }

        public object Clone()
        {
            return new Beneficiary()
            {
                BeneficiaryId = this.BeneficiaryId
            };
        }
    }
}
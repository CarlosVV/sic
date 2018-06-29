namespace Nagnoi.SiC.Domain.Core.Model
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [Serializable]
    [Table("SiC.TransactionDetail")]
    public class TransactionDetail2 
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionDetailId { get; set; }

        public int? TransactionId { get; set; }

        public int? CompensationRegionId { get; set; }

        [Column(TypeName = "decimal")]
        public decimal? Percent { get; set; }

        [Column(TypeName = "money")]
        public decimal? Amount { get; set; }

        public bool? Hidden { get; set; }


        public override bool Equals(object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            TransactionDetail2 p = obj as TransactionDetail2;
            if ((System.Object)p == null)
            {
                return false;
            }

            // Return true if the fields match:
            return (TransactionId == p.TransactionId)
                    && (CompensationRegionId == p.CompensationRegionId)
                    && (Percent == p.Percent);
        }
        
    }
}
namespace Nagnoi.SiC.Domain.Core.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Serializable]
    [Table("WebApp.ResourcesString")]
    public class ResourcesString : ICloneable
    {
        public int ResourcesStringId { get; set; }

        [Required]
        [StringLength(200)]
        public string ResourceName { get; set; }

        [Required]
        public string ResourceValue { get; set; }

        [Required]
        [StringLength(150)]
        public string CreatedBy { get; set; }

        public DateTime CreatedDateTime { get; set; }

        [StringLength(150)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDateTime { get; set; }

        public object Clone()
        {
            return new ResourcesString()
            {
                ResourcesStringId = this.ResourcesStringId,
                ResourceName = this.ResourceName,
                ResourceValue = this.ResourceValue,
                CreatedBy = this.CreatedBy,
                CreatedDateTime = this.CreatedDateTime,
                ModifiedBy = this.ModifiedBy,
                ModifiedDateTime = this.ModifiedDateTime
            };
        }
    }
}
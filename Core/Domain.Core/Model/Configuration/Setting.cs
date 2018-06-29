namespace Nagnoi.SiC.Domain.Core.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Serializable]
    [Table("WebApp.Setting")]
    public class Setting : ICloneable
    {
        public int SettingId { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [Required]
        [StringLength(2000)]
        public string Value { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [StringLength(150)]
        public string CreatedBy { get; set; }

        public DateTime CreatedDateTime { get; set; }

        [StringLength(150)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDateTime { get; set; }

        public object Clone()
        {
            Setting clonedSetting = new Setting();

            clonedSetting.SettingId = this.SettingId;
            clonedSetting.Name = this.Name;
            clonedSetting.Value = this.Value;
            clonedSetting.Description = this.Description;
            clonedSetting.CreatedBy = this.CreatedBy;
            clonedSetting.CreatedDateTime = this.CreatedDateTime;
            clonedSetting.ModifiedBy = this.ModifiedBy;
            clonedSetting.ModifiedDateTime = this.ModifiedDateTime;

            return clonedSetting;
        }
    }
}
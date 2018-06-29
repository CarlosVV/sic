namespace Nagnoi.SiC.Domain.Core.Model {
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("WebApp.MessageTemplate")]
    public class MessageTemplate : ICloneable {
        public int MessageTemplateId { get; set; }

        [Required]
        [StringLength(100)]
        public string MessageTemplateKeyword { get; set; }

        [Required]
        [StringLength(200)]
        public string BccEmailAddresses { get; set; }

        [Required]
        [StringLength(200)]
        public string Subject { get; set; }

        [Required]        
        public string Body { get; set; }

        [Required]        
        public bool IsActive { get; set; }

        [Required]
        [StringLength(150)]
        public string CreatedBy { get; set; }

        [Required]
        public DateTime CreatedDateTime { get; set; }

        [StringLength(150)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDateTime { get; set; }

        public object Clone() {
            MessageTemplate clonedMessageTemplate = new MessageTemplate();

            clonedMessageTemplate.MessageTemplateId = this.MessageTemplateId;
            clonedMessageTemplate.MessageTemplateKeyword = this.MessageTemplateKeyword;
            clonedMessageTemplate.BccEmailAddresses = this.BccEmailAddresses;
            clonedMessageTemplate.Subject = this.Subject;
            clonedMessageTemplate.Body = this.Body;
            clonedMessageTemplate.IsActive = this.IsActive;
            clonedMessageTemplate.CreatedBy = this.CreatedBy;
            clonedMessageTemplate.CreatedDateTime = this.CreatedDateTime;
            clonedMessageTemplate.ModifiedBy = this.ModifiedBy;
            clonedMessageTemplate.ModifiedDateTime = this.ModifiedDateTime;

            return clonedMessageTemplate;
        }
    }
}
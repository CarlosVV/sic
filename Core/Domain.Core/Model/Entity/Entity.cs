namespace Nagnoi.SiC.Domain.Core.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Serializable]
    [Table("Entity.Entity")]
    public class Entity : AuditableEntity, ICloneable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Entity()
        {
            Addresses = new HashSet<Address>();            
            PaymentRemitters = new HashSet<Payment>();            
            ThirdPartyScheduleRemitters = new HashSet<ThirdPartySchedule>();
            CaseDetailEntities = new HashSet<CaseDetail>();
            CaseDetailEntityIncas = new HashSet<CaseDetail>();
            CaseDetailEntityChecks = new HashSet<CaseDetail>();
            CaseDetailEntityLawyers = new HashSet<CaseDetail>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EntityId { get; set; }

        public int? MergeId { get; set; }

        public long EntityBk { get; set; }

        public int SourceId { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string MiddleName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(50)]
        public string SecondLastName { get; set; }

        [StringLength(200)]
        public string FullName { get; set; }

        [StringLength(13)]
        public string VendorNumber { get; set; }

        [StringLength(11)]
        public string CaseNumber { get; set; }

        [NotMapped]
        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(2)]
        public string CaseKey { get; set; }

        [StringLength(11)]
        public string SSN { get; set; }

        [StringLength(15)]
        public string IDNumber { get; set; }

        public int? GenderId { get; set; }

        [Column(TypeName = "date")]
        public DateTime? BirthDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DeceaseDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? MarriageDate { get; set; }

        public int? CivilStatusId { get; set; }

        public int? ParticipantTypeId { get; set; }

        public int? ParticipantStatusId { get; set; }

        public bool? Deleted { get; set; }

        public DateTime? DeletedDate { get; set; }

        public bool? IsStudying { get; set; }

        [Column(TypeName = "date")]
        public DateTime? SchoolStartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? SchoolEndDate { get; set; }

        public bool? HasDisability { get; set; }

        public int? OccupationId { get; set; }

        [Column(TypeName = "money")]
        public decimal? MonthlyIncome { get; set; }

        public bool? IsRehabilitated { get; set; }

        public bool? IsWorking { get; set; }

        public bool? IsEmancipated { get; set; }

        [NotMapped]
        public bool Hidden { get; set; }

        public bool? HasWidowCertification { get; set; }

        [Column(TypeName = "date")]
        public DateTime? WidowCertificationDate { get; set; }

        public int? ModifiedReasonId { get; set; }

        [StringLength(250)]
        public string OtherModifiedReason { get; set; }

        [StringLength(250)]
        public string Comments { get; set; }

        [StringLength(50)]
        public string ETLFingerprint { get; set; }

        [StringLength(25)]
        public string HomePhoneNumber { get; set; }

        [StringLength(25)]
        public string CellPhoneNumber { get; set; }

        [StringLength(25)]
        public string WorkPhoneNumber { get; set; }

        [StringLength(25)]
        public string FaxPhoneNumber { get; set; }

        [StringLength(25)]
        public string OtherPhoneNumber { get; set; }

        public virtual CivilStatus CivilStatus { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Address> Addresses { get; set; }

        public virtual Gender Gender { get; set; }

        public virtual Master Master { get; set; }

        public virtual ParticipantStatus ParticipantStatus { get; set; }

        public virtual ParticipantType ParticipantType { get; set; }

        public virtual ModifiedReason ModifiedReason { get; set; }

        public virtual Occupation Occupation { get; set; }
        

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Payment> PaymentRemitters { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CaseDetail> CaseDetailEntities { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CaseDetail> CaseDetailEntityIncas { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CaseDetail> CaseDetailEntityChecks { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CaseDetail> CaseDetailEntityDiets { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CaseDetail> CaseDetailEntityLawyers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ThirdPartySchedule> ThirdPartyScheduleRemitters { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CaseDetail> CaseDetailEntityLegalGuardians { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CaseDetail> CaseDetailEntitySif { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CaseDetail> CaseDetailEntitySic { get; set; }

        public object Clone()
        {
            Entity clonedEntity = new Entity();

            clonedEntity.EntityId = this.EntityId;
            clonedEntity.MergeId = this.MergeId;
            clonedEntity.EntityBk = this.EntityBk;
            clonedEntity.FirstName = this.FirstName;
            clonedEntity.MiddleName = this.MiddleName;
            clonedEntity.LastName = this.LastName;
            clonedEntity.Email = this.Email;
            clonedEntity.SecondLastName = this.SecondLastName;
            clonedEntity.FullName = this.FullName;
            clonedEntity.SSN = this.SSN;
            clonedEntity.GenderId = this.GenderId;
            clonedEntity.BirthDate = this.BirthDate;
            clonedEntity.CivilStatusId = this.CivilStatusId;
            clonedEntity.ParticipantTypeId = this.ParticipantTypeId;
            clonedEntity.ETLFingerprint = this.ETLFingerprint;
            clonedEntity.Deleted = this.Deleted;
            clonedEntity.SourceId = this.SourceId;
            clonedEntity.DeceaseDate = this.DeceaseDate;
            clonedEntity.IsStudying = this.IsStudying;

            foreach (var address in this.Addresses)
            {
                clonedEntity.Addresses.Add((Address)address.Clone());
            }

            if (this.Gender != null)
            {
                clonedEntity.Gender = this.Gender.Clone() as Gender;
            }

            if (this.ParticipantType != null)
            {
                clonedEntity.ParticipantType = this.ParticipantType.Clone() as ParticipantType;
            }

            if (this.CivilStatus != null)
            {
                clonedEntity.CivilStatus = this.CivilStatus.Clone() as CivilStatus;
            }

            return clonedEntity;
        }
    }
}
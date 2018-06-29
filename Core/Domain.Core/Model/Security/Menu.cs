namespace Nagnoi.SiC.Domain.Core.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Infrastructure.Core.Dependencies;
    using Services;
    [Table("WebApp.Menu")]
    public class Menu : ICloneable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Menu()
        {
            Children = new HashSet<Menu>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MenuId { get; set; }

        [Required]
        [StringLength(50)]
        public string MenuName { get; set; }

        [StringLength(200)]
        public string MenuDescription { get; set; }

        [Required]
        [StringLength(100)]
        public string MenuUrl { get; set; }

        public bool IsActive { get; set; }

        public bool Rendered { get; set; }

        public int? ParentId { get; set; }

        public int? FunctionalityId { get; set; }

        public int DisplayOrder { get; set; }

        [Required]
        [StringLength(150)]
        public string CreatedBy { get; set; }

        public DateTime CreatedDateTime { get; set; }

        [StringLength(150)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDateTime { get; set; }

        public virtual Functionality Functionality { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Menu> Children { get; set; }

        public virtual Menu ParentMenu { get; set; }
        
        [NotMapped]
        public bool CanDisplay
        {
            get
            {
                if (!this.FunctionalityId.HasValue)
                {
                    return true;
                }

                if (this.Functionality == null)
                {
                    return true;
                }

                var aclService = IoC.Resolve<IAccessControlLevelService>();
                
                return aclService.IsFunctionalityAllowed(Functionality.FunctionalityName);
            }
        }

        public object Clone()
        {
            Menu clonedMenu = new Menu();

            clonedMenu.MenuId = this.MenuId;
            clonedMenu.MenuName = this.MenuName;
            clonedMenu.MenuDescription = this.MenuDescription;
            clonedMenu.MenuUrl = this.MenuUrl;
            clonedMenu.IsActive = this.IsActive;
            clonedMenu.ParentId = this.ParentId;
            clonedMenu.FunctionalityId = this.FunctionalityId;
            clonedMenu.Rendered = this.Rendered;
            clonedMenu.DisplayOrder = this.DisplayOrder;

            if (this.Functionality != null)
            {
                clonedMenu.Functionality = this.Functionality.Clone() as Functionality;
            }

            return clonedMenu;
        }
    }
}
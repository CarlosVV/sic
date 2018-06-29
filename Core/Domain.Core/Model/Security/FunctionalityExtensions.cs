namespace Nagnoi.SiC.Domain.Core.Model {
    public static class FuncionalityExtensions {
        public static Functionality Clone(this Functionality entity) {
            return new Functionality {
                FunctionalityId = entity.FunctionalityId,
                FunctionalityName = entity.FunctionalityName,
                IsActive = entity.IsActive,
                ModifiedBy = entity.ModifiedBy,
                ModifiedDateTime = entity.ModifiedDateTime,
                CreatedBy = entity.CreatedBy,
                CreatedDateTime = entity.CreatedDateTime
            };
        }
    }
}


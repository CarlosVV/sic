using Nagnoi.SiC.Domain.Core.Services;
using Nagnoi.SiC.Infrastructure.Core.Dependencies;
using Nagnoi.SiC.Domain.Core.Model;
using Nagnoi.SiC.Infrastructure.Web.Utilities;
using Nagnoi.SiC.Infrastructure.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Nagnoi.SiC.Web.Models.Demographic
{
    public class DemographicViewModel : BaseViewModel
    {
        public DemographicViewModel() 
        {
            CivilStatus = GetCivilStatus();
            Occupations = GetOccupations();
            RelationshipType = GetRelationshipType();
            TransferType = GetTransferType();
            ModifiedReason = GetModifiedReason();
            Cities = new List<SelectListItem>(); 
            States = new List<SelectListItem>(); 
            Countries = GetCountries();
            ParticipantType = GetParticipantType();
            Lawyer = GetLawyersEntities();
        }

        #region Properties
        public int CityId { get; set; }

        public int StateId { get; set; }

        public int CountryId { get; set; }

        public int CivilStatusId { get; set; }

        public int OccupationId { get; set; }

        public int RelationshipTypeId { get; set; }

        public int TransferTypeId { get; set; }

        public int ModifiedReasonId { get; set; }

        public int ParticipantTypeId { get; set; }

        public int EntityId { get; set; }

        public IList<SelectListItem> Cities { get; set; }

        public IList<SelectListItem> States { get; set; }

        public IList<SelectListItem> Countries { get; set; }

        public IList<SelectListItem> CivilStatus { get; set; }

        public IList<SelectListItem> Occupations { get; set; }

        public IList<SelectListItem> RelationshipType { get; set; }

        public IList<SelectListItem> TransferType { get; set; }

        public IList<SelectListItem> ModifiedReason { get; set; }

        public IList<SelectListItem> ParticipantType { get; set; }

        public IList<SelectListItem> Lawyer { get; set; }

        public CaseDetail CaseDetail { get; set; }

        public int CaseDetailId { get; set; }

        public bool HasChildren { get; set; }

        public bool HasAddress { get; set; }

        public string Relationship { get; set; }

        public bool InsertNewEntity { get; set; }

        public Nagnoi.SiC.Domain.Core.Model.Address PostalAddress { get; set; }

        public string City { get; set; }

        #endregion

        #region Private Method
        /// <summary>
        /// Returns all the civil status availble in the service.
        /// </summary>
        /// <returns></returns>
        private IList<SelectListItem> GetCivilStatus()
        {
            var civilStatus = IoC.Resolve<ICivilStatusService>().GetCivilStatusAll().Where(x => x.Hidden == false);

            return civilStatus.ToSelectList(c => c.CivilStatus1, c => c.CivilStatusId.ToString(), null, "Seleccionar");
        }

        /// <summary>
        /// Returns all occupations available in the service.
        /// </summary>
        /// <returns></returns>
        private IList<SelectListItem> GetOccupations()
        {
            var occupations = IoC.Resolve<IOccupationService>().GetAll();//.Where(x => x.Hidden == false); //Añadir hidden a la tabla de occupation

            return occupations.ToSelectList(o => o.Occupation1, o => o.OccupationId.ToString(), null, "Seleccionar");
        }

        /// <summary>
        /// Groups all relationship types and Returns the distinct relationships types 
        /// </summary>
        /// <returns></returns>
        private IList<SelectListItem> GetRelationshipType()
        {
            var relationshipType = IoC.Resolve<IRelationshipTypeService>().GetRelationshipTypes().Where(x => x.Hidden == false);

            var rList = relationshipType.GroupBy(g => g.RelationshipType1)
                                        .Select(
                                            group =>
                                                new
                                                {
                                                    RelationshipId = group.Max(g => g.RelationshipTypeId),
                                                    RelationshipType = group.Key
                                                }
                                        );
            return rList.ToSelectList(r => r.RelationshipType, r => r.RelationshipType, null, "Seleccionar");
        }

        /// <summary>
        /// Groups by countries and returns the grouped data. 
        /// </summary>
        /// <returns></returns>
        private IList<SelectListItem> GetCountries()
        {
            var countries = IoC.Resolve<ILocationService>().GetAllCountries().Where(x => x.Hidden == false);

            return countries.ToSelectList(c => c.Country1, c => c.CountryId.ToString(), null, "Seleccionar");
        }
        
        /// <summary>
        /// Returns all payment transfer type from the service.
        /// </summary>
        /// <returns></returns>
        private IList<SelectListItem> GetTransferType()
        {
            var transferType = IoC.Resolve<ITransferTypeService>().GetTransferTypes().Where(x => x.Hidden == false);

            return transferType.ToSelectList(t => t.TransferType1, t => t.TransferTypeId.ToString(), null, "Seleccionar");
        }

        /// <summary>
        /// Returns all Modified Reason available in the service.
        /// </summary>
        /// <returns></returns>
        private IList<SelectListItem> GetModifiedReason()
        {
            var mReason = IoC.Resolve<IModifiedReasonService>().GetAll().Where(x => x.Hidden == false);

            return mReason.ToSelectList(m => m.ModifiedReason1, m => m.ModifiedReasonId.ToString(), null, "Seleccionar");
        }

        /// <summary>
        /// Returns all Modified Reason available in the service.
        /// </summary>
        /// <returns></returns>
        private IList<SelectListItem> GetParticipantType()
        {
            var pType = IoC.Resolve<IParticipantTypeService>().GetAll().Where(x => x.Status == true && x.ParticipantType1 != "Lesionado" && x.ParticipantType1 != "Beneficiario" && x.ParticipantType1 != "Abogado");

            return pType.ToSelectList(m => m.ParticipantType1, m => m.ParticipantTypeId.ToString(), null, "Seleccionar");
        }

        /// <summary>
        /// Returns all available lawyers entity of CFSE.
        /// </summary>
        /// <returns></returns>
        private IList<SelectListItem> GetLawyersEntities()
        {
            var lawyer = IoC.Resolve<IEntityService>().GetBySourceId(8).Where(x => !String.IsNullOrEmpty(x.FullName)); ;

            return lawyer.ToSelectList(l => l.FullName, l => l.EntityId.ToString(), null, "Seleccionar");
        }

        #endregion
    }
}
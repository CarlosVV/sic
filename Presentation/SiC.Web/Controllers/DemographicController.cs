using CDI.WebApplication.DataTables.Result;
using Nagnoi.SiC.Infrastructure.Web.Controllers;
using Nagnoi.SiC.Web.Models.Demographic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Nagnoi.SiC.Domain.Core.Model;
using Nagnoi.SiC.Infrastructure.Core.Helpers;

namespace Nagnoi.SiC.Web.Controllers
{    
    public class DemographicController : BaseController
    {
        // GET: Demographic
        public ActionResult Index()
        {
            var model = new DemographicViewModel();
            return View(model);
        }

        //Retrieve demographic information based on the casedetailid and entityid.
        [HttpPost]
        [Route("Demographic/Result/")]
        public JsonResult Result(int caseDetailId, int entityId, string caseNumber)
        {
            var injuredDeceaseDate = CaseService.FindCaseDetailByNumber(caseNumber, "00").Entity.DeceaseDate;
            var demographic = CaseService.FindInjuredDetail(caseDetailId, entityId);
            var PostalAddress = demographic.Entity.Addresses.Where(x => x.AddressType.AddressType1 == "Postal").FirstOrDefault();
            //var phones = demographic.Entity.Phones.GroupBy(x => x.PhoneTypeId, (key, xs) => xs.OrderByDescending(x => x.PhoneId).FirstOrDefault());

            var result = new { Data = demographic, Address = PostalAddress, InjuredDeceaseDate = injuredDeceaseDate };
            return Json(result);
        }

        //Process client side data and updates demographic data in the database.
        public JsonResult Edit(DemographicViewModel demographic)
        {
            if (demographic.CaseDetail != null)
            {

                var caseDetail = CaseService.FindCaseDetailById(demographic.CaseDetailId);
                if (caseDetail != null)
                {
                    if (caseDetail.CaseKey != "00")
                    {
                        var relationship = RelationshipTypeService.GetRelationshipTypes()
                                    .Where(x => x.RelationshipType1 == demographic.Relationship &&
                                                x.WidowCertification == demographic.CaseDetail.Entity.HasWidowCertification)
                                    .FirstOrDefault();

                        caseDetail.RelationshipType = relationship;
                        caseDetail.RelationshipTypeId = relationship.RelationshipTypeId;
                    }

                    #region Entity Information
                    Entity entity = new Entity();
                    if (!demographic.InsertNewEntity)
                    {
                        entity = EntityService.GetById(Convert.ToInt32(caseDetail.EntityId_Sic));
                    }

                    entity.FirstName = demographic.CaseDetail.Entity.FirstName.Trim();
                    entity.MiddleName = demographic.CaseDetail.Entity.MiddleName != null ? demographic.CaseDetail.Entity.MiddleName.Trim() : null;
                    entity.LastName = demographic.CaseDetail.Entity.LastName.Trim();
                    entity.SecondLastName = demographic.CaseDetail.Entity.SecondLastName != null ? demographic.CaseDetail.Entity.SecondLastName.Trim() : null;
                    entity.FullName = string.Concat(
                            entity.FirstName + " ", 
                            entity.MiddleName.IsNullOrEmpty() ? String.Empty : entity.MiddleName + " ",
                            entity.LastName + "  ",
                            entity.SecondLastName.IsNullOrEmpty() ? String.Empty : entity.SecondLastName + " ");

                    entity.CaseNumber = demographic.CaseDetail.CaseNumber;
                    entity.CaseKey = demographic.CaseDetail.CaseKey;
                    entity.SSN = demographic.CaseDetail.Entity.SSN;
                    entity.IDNumber = demographic.CaseDetail.Entity.IDNumber;
                    entity.BirthDate = demographic.CaseDetail.Entity.BirthDate;
                    entity.DeceaseDate = demographic.CaseDetail.Entity.DeceaseDate;
                    entity.MarriageDate = demographic.CaseDetail.Entity.MarriageDate;
                    entity.CivilStatusId = demographic.CaseDetail.Entity.CivilStatusId;
                    entity.IsStudying = demographic.CaseDetail.Entity.IsStudying;
                    entity.SchoolStartDate = demographic.CaseDetail.Entity.SchoolStartDate;
                    entity.SchoolEndDate = demographic.CaseDetail.Entity.SchoolEndDate;
                    entity.HasDisability = demographic.CaseDetail.Entity.HasDisability;
                    entity.OccupationId = demographic.CaseDetail.Entity.OccupationId;
                    entity.MonthlyIncome = demographic.CaseDetail.Entity.MonthlyIncome;
                    entity.IsRehabilitated = demographic.CaseDetail.Entity.IsRehabilitated;
                    entity.IsWorking = demographic.CaseDetail.Entity.IsWorking;
                    entity.IsEmancipated = demographic.CaseDetail.Entity.IsEmancipated;
                    entity.HasWidowCertification = demographic.CaseDetail.Entity.HasWidowCertification;
                    entity.WidowCertificationDate = demographic.CaseDetail.Entity.WidowCertificationDate;
                    entity.ModifiedReasonId = demographic.CaseDetail.Entity.ModifiedReasonId;
                    entity.OtherModifiedReason = demographic.CaseDetail.Entity.OtherModifiedReason;
                    entity.Comments = demographic.CaseDetail.Entity.Comments;
                    entity.Email = demographic.CaseDetail.Entity.Email;
                    entity.HomePhoneNumber = demographic.CaseDetail.Entity.HomePhoneNumber;
                    entity.CellPhoneNumber = demographic.CaseDetail.Entity.CellPhoneNumber;
                    entity.WorkPhoneNumber = demographic.CaseDetail.Entity.WorkPhoneNumber;
                    entity.FaxPhoneNumber = demographic.CaseDetail.Entity.FaxPhoneNumber;
                    entity.OtherPhoneNumber = demographic.CaseDetail.Entity.OtherPhoneNumber;

                    entity.ModifiedBy = WebHelper.GetUserName();
                    entity.ModifiedDateTime = DateTime.Now;
                    #endregion

                    #region Address Information
                    if (demographic.PostalAddress != null)
                    {
                        Address PostalAddress = demographic.PostalAddress;
                        
                        //Verificar una manera de traer toda la informaicon de coutry / state a traves de ciudad.
                        var city = LocationService.GetAllCities().Where(x => x.CityId == demographic.PostalAddress.CityId).FirstOrDefault();
                        string state = "";
                        if (demographic.PostalAddress.StateId != null)
                        {
                            var state1 = LocationService.GetAllStates().Where(x => x.StateId == demographic.PostalAddress.StateId).FirstOrDefault();
                            state = state1.State1;
                        }
                        var country = LocationService.GetAllCountries().Where(x => x.CountryId == demographic.PostalAddress.CountryId).FirstOrDefault();

                        PostalAddress.FullAddress =
                                        demographic.PostalAddress.Line1
                                        + (demographic.PostalAddress.Line2.IsNullOrEmpty() ? "" : " " + demographic.PostalAddress.Line2.Trim())
                                        + (demographic.PostalAddress.CityId == null ? "" : city.City1 != "Otro" ? " " + city.City1 : " " + demographic.PostalAddress.OtherCity)
                                        + (demographic.PostalAddress.StateId == null ? "" : ", " + state)
                                        + (demographic.PostalAddress.CountryId == null ? "" : ", " + country.Country1)
                                        + (demographic.PostalAddress.ZipCode == null ? "" : " " + demographic.PostalAddress.ZipCode)
                                        + (demographic.PostalAddress.ZipCodeExt == null ? "" : "-" + demographic.PostalAddress.ZipCodeExt);
                        PostalAddress.AddressTypeId = AddressTypeService.GetAddressTypes().Where(x => x.AddressType1 == "Postal").Select(x => x.AddressTypeId).FirstOrDefault();
                        PostalAddress.SourceId = 8;
                        PostalAddress.ModifiedBy = WebHelper.GetUserName();
                        PostalAddress.ModifiedDateTime = DateTime.Now;

                        if (!demographic.HasAddress)
                        {
                            PostalAddress.CreatedBy = WebHelper.GetUserName();
                            PostalAddress.CreatedDateTime = DateTime.Now;
                            entity.Addresses.Add(PostalAddress);
                        }
                        else
                        {
                            foreach (var a in demographic.CaseDetail.Entity.Addresses)
                            {
                                if (a.AddressType.AddressType1 == "Postal" && a.AddressId == demographic.PostalAddress.AddressId)
                                {
                                    a.Line1 = demographic.PostalAddress.Line1;
                                    a.Line2 = demographic.PostalAddress.Line2;
                                    a.CityId = demographic.PostalAddress.CityId;
                                    a.OtherCity = demographic.PostalAddress.OtherCity;
                                    a.StateId = demographic.PostalAddress.StateId;
                                    a.CountryId = demographic.PostalAddress.CountryId;
                                    a.ZipCode = demographic.PostalAddress.ZipCode;
                                    a.ZipCodeExt = demographic.PostalAddress.ZipCodeExt;
                                    a.FullAddress = demographic.PostalAddress.FullAddress;
                                    a.EntityId = demographic.PostalAddress.EntityId;
                                }
                            }
                        }
                    }
                    #endregion

                    //#region Phone Information
                    
                    //#endregion

                    if (demographic.InsertNewEntity)
                    {
                        entity.SourceId = 8; //SourceId = Entity Sic Source
                        entity.CreatedBy = WebHelper.GetUserName();
                        entity.CreatedDateTime = DateTime.Now;
                        try
                        {
                            EntityService.CreateEntity(entity);
                        }
                        catch
                        {
                            throw new Exception();
                        }
                    }
                    else
                    {
                        try
                        {
                            EntityService.ModifyEntity(entity);
                        }
                        catch
                        {
                            throw new Exception();
                        }
                    }

                    caseDetail.Entity = entity;
                    caseDetail.EntitySic = entity;
                    caseDetail.EntityId = entity.EntityId;
                    caseDetail.EntityId_Sic = entity.EntityId;

                    try
                    {
                        CaseService.UpdateCaseDetail(caseDetail);
                    }
                    catch
                    {
                        EntityService.Delete(entity.EntityId);
                        throw new Exception();
                    }

                    return Json(new { Data = demographic.CaseDetail });
                }
                else
                {
                    throw new Exception();
                }
                
            }
            else
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Creates new participant entity
        /// </summary>
        /// <param name="entity">Entity Data</param>
        /// <param name="PostalAddress">Address Data</param>
        /// <returns></returns>
        public JsonResult CreateParticipant(Entity entity, Address PostalAddress)
        {
            if (entity != null)
            {
                entity.FullName = string.Concat(
                  entity.FirstName + " ",
                  entity.MiddleName.IsNullOrEmpty() ? String.Empty : entity.MiddleName + " ",
                  entity.LastName + "  ",
                  entity.SecondLastName.IsNullOrEmpty() ? String.Empty : entity.SecondLastName + " ");

                if (PostalAddress != null)
                {
                    //Address address = new Address();
                    //address.Line1 = PostalAddress.Line1;
                    var city = LocationService.GetAllCities().Where(x => x.CityId == PostalAddress.CityId).FirstOrDefault();
                    string state = "";
                    if (PostalAddress.StateId != null)
                    {
                        var state1 = LocationService.GetAllStates().Where(x => x.StateId == PostalAddress.StateId).FirstOrDefault();
                        state = state1.State1;
                    }
                    var country = LocationService.GetAllCountries().Where(x => x.CountryId == PostalAddress.CountryId).FirstOrDefault();

                    PostalAddress.FullAddress =
                           PostalAddress.Line1
                        + (PostalAddress.Line2.IsNullOrEmpty() ? "" : " " + PostalAddress.Line2.Trim())
                        + (PostalAddress.CityId == null ? "" : city.City1 != "Otro" ? " " + city.City1 : " " + PostalAddress.OtherCity)
                        + (PostalAddress.StateId == null ? "" : ", " + state)
                        + (PostalAddress.CountryId == null ? "" : ", " + country.Country1)
                        + (PostalAddress.ZipCode == null ? "" : " " + PostalAddress.ZipCode)
                        + (PostalAddress.ZipCodeExt == null ? "" : "-" + PostalAddress.ZipCodeExt);
                    PostalAddress.AddressTypeId = AddressTypeService.GetAddressTypes().Where(x => x.AddressType1 == "Postal").Select(x => x.AddressTypeId).FirstOrDefault();
                    PostalAddress.SourceId = 8;
                    PostalAddress.CreatedBy = WebHelper.GetUserName();
                    PostalAddress.CreatedDateTime = DateTime.Now;
                    PostalAddress.ModifiedBy = WebHelper.GetUserName();
                    PostalAddress.ModifiedDateTime = DateTime.Now;
                    entity.Addresses.Add(PostalAddress);
                }

                entity.SourceId = 8;
                entity.CreatedBy = WebHelper.GetUserName();
                entity.CreatedDateTime = DateTime.Now;
                entity.ModifiedBy = WebHelper.GetUserName();
                entity.ModifiedDateTime = DateTime.Now;

                try
                {
                    EntityService.CreateEntity(entity);
                    return Json(entity);
                }
                catch
                {
                    throw new Exception();
                }
            }
            else
                throw new Exception();
        }

        /// <summary>
        /// Gets all available legal guardian.
        /// </summary>
        /// <param name="caseNumber">Case Number</param>
        /// <param name="caseKey">Beneficiary Key</param>
        /// <returns></returns>
        public JsonResult GetLegalGuardians(string caseNumber, string caseKey)
        {
            var legalGuardians = EntityService.GetByCaseNumber(caseNumber).Where(x => x.ParticipantType != null && x.BirthDate != null);
            
            if (legalGuardians != null)
            {
                //string[] participantType = {"Abogado","Tutor (C.I.)", "Beneficiario"};

                var lgData = legalGuardians.Where(
                    //x => x.ParticipantType.ParticipantType1.Contains(participantType) &&
                    x => (
                            ( //Mejorar codigo
                                x.ParticipantType.ParticipantType1 == "Abogado" ||
                                x.ParticipantType.ParticipantType1 == "Tutor (C.I.)" ||
                                x.ParticipantType.ParticipantType1 == "Beneficiario"
                            ) && 
                            GetAge(x.BirthDate) >= 21 && 
                            (x.FullName != null && x.FullName.Trim() != "") && 
                            x.ParticipantType.Hidden == false)
                    ).Select(x => new {x.EntityId, x.FullName});

                return Json(lgData);
            }
            else
            {
                return Json(null);
            }
        }

        /// <summary>
        /// Get all available states.
        /// </summary>
        /// <returns>Returns all the states filtered by country</returns>
        public JsonResult GetStates(int countryId)
        {
            var states = LocationService.GetAllStates().Where(x => x.CountryId == countryId).Select(x => new { x.StateId, x.State1});

            return Json(states);
        }

        /// <summary>
        /// Get cities based on the state selected.
        /// </summary>
        /// <returns>Returns cities filtered by state</returns>
        public JsonResult GetCitiesByState(int stateId)
        {
            var cities = LocationService.GetAllCities().Where(x => x.StateId == stateId).Select(x => new {x.CityId, x.City1});

            return Json(cities);
        }

        /// <summary>
        /// Returns cities based on the country selected.
        /// </summary>
        /// <returns>Returns cities filtered by country</returns>
        public JsonResult GetCitiesByCountry(int countryId)
        {
            var cities = LocationService.GetAllCities().Where(x => x.CountryId == countryId).Select(x => new { x.CityId, x.City1 });

            return Json(cities);
        }

        /// <summary>
        /// Calculates the age.
        /// </summary>
        /// <param name="dob">Date Of Birthparam>
        /// <returns>Returns calculated age</returns>
        public int GetAge(DateTime? dob)
        {
            var today = DateTime.Today;
            if(dob != null){
                var dobdt = (DateTime)dob;
                var age = today.Year - dobdt.Year;
                if (dob > today.AddYears(-age)) 
                    age--;
                return age;
            }
            else{
                return -1;
            }
            
        }
    }
}
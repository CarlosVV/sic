namespace Nagnoi.SiC.Web.Controllers
{
    #region References

    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Infrastructure.Web.Controllers;
    using Domain.Core.Model;
    using Web.Models.ReferenceTables;

    #endregion    
    public class ReferenceTablesController : BaseController
    {
        #region Actions

        public ActionResult Index()
        {
            var model = new IndexViewModel();

            model.ReferenceTables = GetAllTableReferences().ToList();

            bool isAllow = PermissionService.IsFunctionalityAllowed("ReferenceTable.ListTables");

            if (isAllow)
            {
                // Do Something
            }

            return View(model);
        }

        public IEnumerable<SelectListItem> GetAllTableReferences()
        {
            IList<SelectListItem> referenceTables = new List<SelectListItem>();

            referenceTables.Add(new SelectListItem { Value = "0", Text = "--Seleccione una tabla de referencia--" });
            referenceTables.Add(new SelectListItem { Value = "1", Text = "Tipo Dirección" });
            referenceTables.Add(new SelectListItem { Value = "2", Text = "Aplica a" });
            referenceTables.Add(new SelectListItem { Value = "4", Text = "Estado Civil" });
            referenceTables.Add(new SelectListItem { Value = "5", Text = "Genero" });            
            referenceTables.Add(new SelectListItem { Value = "7", Text = "Clase del Pago" });
            referenceTables.Add(new SelectListItem { Value = "8", Text = "Tipo de Transferencia" });
            referenceTables.Add(new SelectListItem { Value = "9", Text = "Tipo de Transacción" });
            referenceTables.Add(new SelectListItem { Value = "10", Text = "Concepto de Pago" });

            return referenceTables;
        }

        public JsonResult ListarTablaReferenciaTipoDireccion()
        {
            var addressTypeList = this.AddressTypeService.GetAddressTypesAll();
            var resultado = addressTypeList.ToList().Select
                (item => new
                {
                    AddressTypeId = item.AddressTypeId.ToString(),
                    AddressType1 = item.AddressType1,
                    Hidden = item.Hidden
                }).ToList();

            return Json(new { data = resultado }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddressTypeUpdate(string action)
        {
            var addressTypesUpdateList = new List<AddressType>();
            var addressTypeList = this.AddressTypeService.GetAddressTypesAll().Select(m => m.AddressTypeId).ToList();
            int recordCount = addressTypeList.Count;
            int recordIndex = 0;

            if (action == "create")
            {
                string addressType1 = Request.Form["data[0][AddressType1]"].ToString();
                bool hidden = bool.Parse(Request.Form["data[0][Hidden]"].ToString());
                AddressType addressType = new AddressType()
                {
                    AddressType1 = addressType1,
                    Hidden = hidden
                };
                addressTypesUpdateList.Add(addressType);
                this.AddressTypeService.InsertAddressType(addressTypesUpdateList[0]);
            }
            else {
                do
                {
                    if (Request.Form[string.Format("data[{0}][AddressType1]", addressTypeList[recordIndex])] != null)
                    {
                        int addressTypeId = addressTypeList[recordIndex];
                        string addressType1 = Request.Form[string.Format("data[{0}][AddressType1]", addressTypeList[recordIndex])].ToString();
                        bool hidden = bool.Parse(Request.Form[string.Format("data[{0}][Hidden]", addressTypeList[recordIndex])].ToString());
                        AddressType addressType = new AddressType()
                        {
                            AddressTypeId = addressTypeId,
                            AddressType1 = addressType1,
                            Hidden = hidden
                        };
                        addressTypesUpdateList.Add(addressType);
                    }
                    recordIndex++;
                }
                while (recordIndex < recordCount);

                for (int i = 0; i < addressTypesUpdateList.Count; i++)
                {
                    switch (action)
                    {
                        case "edit":
                            this.AddressTypeService.UpdateAddressType(addressTypesUpdateList[i]);
                            break;
                        case "remove":
                            this.AddressTypeService.DeleteAddressType(addressTypesUpdateList[i].AddressTypeId);
                            break;
                    }
                }
            }

            return Json(new { data = addressTypesUpdateList }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListarTablaReferenciaGenero()
        {
            var genderList = this.GenderService.GetGender();
            var resultado = genderList.ToList().Select
                (item => new
                {
                    GenderId = item.GenderId.ToString(),
                    Gender1 = item.Gender1,
                    GenderCode = item.GenderCode,
                    Hidden = item.Hidden
                }).ToList();

            return Json(new { data = resultado }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GenderUpdate(string action)
        {
            List<Gender> genderUpdateList = new List<Gender>();
            var genderList = this.GenderService.GetGender().Select(m => m.GenderId).ToList();
            int recordCount = genderList.Count;
            int recordIndex = 0;

            if (action == "create")
            {
                string gender1 = Request.Form["data[0][Gender1]"].ToString();
                string genderCode = Request.Form["data[0][GenderCode]"].ToString();
                bool hidden = bool.Parse(Request.Form["data[0][Hidden]"].ToString());
                Gender gender = new Gender()
                {
                    Gender1 = gender1,
                    GenderCode = genderCode,
                    Hidden = hidden
                };
                genderUpdateList.Add(gender);
                this.GenderService.InsertGender(genderUpdateList[0]);
            }
            else
            {
                do
                {
                    if (Request.Form[string.Format("data[{0}][Gender1]", genderList[recordIndex])] != null)
                    {
                        int genderId = genderList[recordIndex];
                        string gender1 = Request.Form[string.Format("data[{0}][Gender1]", genderList[recordIndex])].ToString();
                        string genderCode = Request.Form[string.Format("data[{0}][GenderCode]", genderList[recordIndex])].ToString();
                        bool hidden = bool.Parse(Request.Form[string.Format("data[{0}][Hidden]", genderList[recordIndex])].ToString());
                        Gender gender = new Gender()
                        {
                            GenderId = genderId,
                            Gender1 = gender1,
                            GenderCode = genderCode,
                            Hidden = hidden
                        };
                        genderUpdateList.Add(gender);
                    }
                    recordIndex++;
                }
                while (recordIndex < recordCount);

                for (int i = 0; i < genderUpdateList.Count; i++)
                {
                    switch (action)
                    {
                        case "edit":
                            this.GenderService.UpdateGender(genderUpdateList[i]);
                            break;
                        case "remove":
                            this.GenderService.DeleteGender(genderUpdateList[i].GenderId);
                            break;
                    }
                }
            }

            return Json(new { data = genderUpdateList }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarTablaReferenciaClasePago()
        {
            var classList = this.ClassService.GetClasses();
            var resultado = classList.ToList().Select
                (item => new
                {
                    ClassId = item.ClassId,
                    Class1 = item.Class1,
                    Concept = item.Concept,
                    Hidden = item.Hidden
                });

            return Json(new { data = resultado }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ClassUpdate(string action)
        {
            List<Class> classUpdateList = new List<Class>();
            var classList = this.ClassService.GetClasses().Select(m => m.ClassId).ToList();
            int recordCount = classList.Count;
            int recordIndex = 0;

            if (action == "create")
            {
                string class1 = Request.Form["data[0][Class1]"].ToString();
                string concept = Request.Form["data[0][Concept]"].ToString();
                bool hidden = bool.Parse(Request.Form["data[0][Hidden]"].ToString());
                Class _class = new Class()
                {
                    Class1 = class1,
                    Concept = concept,
                    Hidden = hidden
                };
                classUpdateList.Add(_class);
                this.ClassService.InsertClass(classUpdateList[0]);
            }
            else
            {
                do
                {
                    if (Request.Form[string.Format("data[{0}][Class1]", classList[recordIndex])] != null)
                    {
                        int classId = classList[recordIndex];
                        string class1 = Request.Form[string.Format("data[{0}][Class1]", classList[recordIndex])].ToString();
                        string concept = Request.Form[string.Format("data[{0}][Concept]", classList[recordIndex])];
                        bool hidden = bool.Parse(Request.Form[string.Format("data[{0}][Hidden]", classList[recordIndex])].ToString());
                        Class _class = new Class()
                        {
                            ClassId = classId,
                            Class1 = class1,
                            Concept = concept,
                            Hidden = hidden
                        };
                        classUpdateList.Add(_class);
                    }
                    recordIndex++;
                }
                while (recordIndex < recordCount);

                for (int i = 0; i < classUpdateList.Count; i++)
                {
                    switch (action)
                    {
                        case "edit":
                            this.ClassService.UpdateClass(classUpdateList[i]);
                            break;
                        case "remove":
                            this.ClassService.DeleteClass(classUpdateList[i].ClassId);
                            break;
                    }
                }
            }

            return Json(new { data = classUpdateList }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListarTablaReferenciaTipoDeTransferencia()
        {
            var transferTypeList = this.TransferTypeService.GetTransferTypes();
            var resultado = transferTypeList.ToList().Select
                            (item => new
                            {
                                TransferTypeId = item.TransferTypeId.ToString(),
                                TransferType1 = item.TransferType1,
                                Hidden = item.Hidden
                            }).ToList();

            return Json(new { data = resultado }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult TransferTypeUpdate(string action)
        {
            List<Nagnoi.SiC.Domain.Core.Model.TransferType> transferTypeUpdateList = new List<Nagnoi.SiC.Domain.Core.Model.TransferType>();
            var transferTypeList = this.TransferTypeService.GetTransferTypes().Select(m => m.TransferTypeId).ToList();
            int recordCount = transferTypeList.Count;
            int recordIndex = 0;

            if (action == "create")
            {
                string transferType1 = Request.Form["data[0][TransferType1]"].ToString();
                bool hidden = bool.Parse(Request.Form["data[0][Hidden]"].ToString());
                TransferType transferType = new TransferType()
                {
                    TransferType1 = transferType1,
                    Hidden = hidden
                };
                transferTypeUpdateList.Add(transferType);
                this.TransferTypeService.InsertTransferType(transferTypeUpdateList[0]);
            }
            else
            {
                do
                {
                    if (Request.Form[string.Format("data[{0}][TransferType1]", transferTypeList[recordIndex])] != null)
                    {
                        int transferTypeId = transferTypeList[recordIndex];
                        string transferType1 = Request.Form[string.Format("data[{0}][TransferType1]", transferTypeList[recordIndex])].ToString();
                        bool hidden = bool.Parse(Request.Form[string.Format("data[{0}][Hidden]", transferTypeList[recordIndex])].ToString());
                        TransferType transferType = new TransferType()
                        {
                            TransferTypeId = transferTypeId,
                            TransferType1 = transferType1,
                            Hidden = hidden
                        };
                        transferTypeUpdateList.Add(transferType);
                    }
                    recordIndex++;
                }
                while (recordIndex < recordCount);

                for (int i = 0; i < transferTypeUpdateList.Count; i++)
                {
                    switch (action)
                    {
                        case "edit":
                            this.TransferTypeService.UpdateTransferType(transferTypeUpdateList[i]);
                            break;
                        case "remove":
                            this.TransferTypeService.DeleteTransferType(transferTypeUpdateList[i].TransferTypeId);
                            break;
                    }
                }

            }
            return Json(new { data = transferTypeUpdateList }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListarTablaReferenciaTipoDeTransaccion()
        {
            var transactionTypeList = this.TransactionTypeService.GetTransactionTypes();
            var resultado = transactionTypeList.ToList().Select
                            (item => new
                            {
                                TransactionTypeId = item.TransactionTypeId.ToString(),
                                TransactionType1 = item.TransactionType1,
                                Hidden = item.Hidden
                            }).ToList();

            return Json(new { data = resultado }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult TransactionTypeUpdate(string action)
        {
            List<Nagnoi.SiC.Domain.Core.Model.TransactionType> transactionTypeUpdateList = new List<Nagnoi.SiC.Domain.Core.Model.TransactionType>();
            var transactionTypeList = this.TransactionTypeService.GetTransactionTypes().Select(m => m.TransactionTypeId).ToList();
            int recordCount = transactionTypeList.Count;
            int recordIndex = 0;

            if (action == "create")
            {
                string transactionType1 = Request.Form["data[0][TransactionType1]"].ToString();
                bool hidden = bool.Parse(Request.Form["data[0][Hidden]"].ToString());
                TransactionType transactionType = new TransactionType()
                {
                    TransactionType1 = transactionType1,
                    Hidden = hidden
                };
                transactionTypeUpdateList.Add(transactionType);
                this.TransactionTypeService.InsertTransactionType(transactionTypeUpdateList[0]);
            }
            else
            {
                do
                {
                    if (Request.Form[string.Format("data[{0}][TransactionType1]", transactionTypeList[recordIndex])] != null)
                    {
                        int transactionTypeId = transactionTypeList[recordIndex];
                        string transactionType1 = Request.Form[string.Format("data[{0}][TransactionType1]", transactionTypeList[recordIndex])].ToString();
                        bool hidden = bool.Parse(Request.Form[string.Format("data[{0}][Hidden]", transactionTypeList[recordIndex])].ToString());
                        TransactionType transactionType = new TransactionType()
                        {
                            TransactionTypeId = transactionTypeId,
                            TransactionType1 = transactionType1,
                            Hidden = hidden
                        };
                        transactionTypeUpdateList.Add(transactionType);
                    }
                    recordIndex++;
                }
                while (recordIndex < recordCount);

                for (int i = 0; i < transactionTypeUpdateList.Count; i++)
                {
                    switch (action)
                    {
                        case "edit":
                            this.TransactionTypeService.UpdateTransactionType(transactionTypeUpdateList[i]);
                            break;
                        case "remove":
                            this.TransactionTypeService.DeleteTransactionType(transactionTypeUpdateList[i].TransactionTypeId);
                            break;
                    }
                }

            }
            return Json(new { data = transactionTypeUpdateList }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListarTablaReferenciaApplyTo()
        {
            var applyToList = this.ApplyToService.GetApplyTosAll();
            var resultado = applyToList.ToList().Select
                (item => new
                {
                    ApplyToId = item.ApplyToId.ToString(),
                    ApplyTo1 = item.ApplyTo1,
                    Hidden = item.Hidden
                }).ToList();

            return Json(new { data = resultado }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ApplyToUpdate(string action)
        {
            List<ApplyTo> applyToUpdateList = new List<ApplyTo>();
            var applyToList = this.ApplyToService.GetApplyTosAll().Select(m => m.ApplyToId).ToList();
            int recordCount = applyToList.Count;
            int recordIndex = 0;

            if (action == "create")
            {
                string applyTo1 = Request.Form["data[0][ApplyTo1]"].ToString();
                bool hidden = bool.Parse(Request.Form["data[0][Hidden]"].ToString());
                var applyTo = new ApplyTo()
                {
                    ApplyTo1 = applyTo1,
                    Hidden = hidden
                };
                applyToUpdateList.Add(applyTo);
                this.ApplyToService.InsertApplyTo(applyToUpdateList[0]);
            }
            else
            {
                do
                {
                    if (Request.Form[string.Format("data[{0}][ApplyTo1]", applyToList[recordIndex])] != null)
                    {
                        int applyToId = applyToList[recordIndex];
                        string applyTo1 = Request.Form[string.Format("data[{0}][ApplyTo1]", applyToList[recordIndex])].ToString();
                        bool hidden = bool.Parse(Request.Form[string.Format("data[{0}][Hidden]", applyToList[recordIndex])].ToString());
                        var applyTo = new ApplyTo()
                        {
                            ApplyToId = applyToId,
                            ApplyTo1 = applyTo1,
                            Hidden = hidden
                        };
                        applyToUpdateList.Add(applyTo);
                    }
                    recordIndex++;
                }
                while (recordIndex < recordCount);

                for (int i = 0; i < applyToUpdateList.Count; i++)
                {
                    switch (action)
                    {
                        case "edit":
                            this.ApplyToService.UpdateApplyTo(applyToUpdateList[i]);
                            break;
                        case "remove":
                            this.ApplyToService.DeleteApplyTo(applyToUpdateList[i].ApplyToId);
                            break;
                    }
                }
            }

            return Json(new { data = applyToUpdateList }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListarTablaReferenciaCivilStatus()
        {
            var civilStatusList = this.CivilStatusService.GetCivilStatusAll();
            var resultado = civilStatusList.ToList().Select
                (item => new
                {
                    CivilStatusId = item.CivilStatusId.ToString(),
                    CivilStatus1 = item.CivilStatus1,
                    CivilStatusCode = item.CivilStatusCode,
                    Hidden = item.Hidden
                }).ToList();

            return Json(new { data = resultado }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CivilStatusUpdate(string action)
        {
            var civilStatusUpdateList = new List<CivilStatus>();
            var civilStatusList = this.CivilStatusService.GetCivilStatus().Select(m => m.CivilStatusId).ToList();
            int recordCount = civilStatusList.Count;
            int recordIndex = 0;

            if (action == "create")
            {
                string civilStatus1 = Request.Form["data[0][CivilStatus1]"].ToString();
                string civilStatusCode = Request.Form["data[0][CivilStatusCode]"].ToString();
                bool hidden = bool.Parse(Request.Form["data[0][Hidden]"].ToString());
                var civilStatus = new CivilStatus()
                {
                    CivilStatus1 = civilStatus1,
                    CivilStatusCode = civilStatusCode,
                    Hidden = hidden
                };
                civilStatusUpdateList.Add(civilStatus);
                this.CivilStatusService.InsertCivilStatus(civilStatusUpdateList[0]);
            }
            else
            {
                do
                {
                    if (Request.Form[string.Format("data[{0}][CivilStatus1]", civilStatusList[recordIndex])] != null)
                    {
                        int CivilStatusId = civilStatusList[recordIndex];
                        string CivilStatus1 = Request.Form[string.Format("data[{0}][CivilStatus1]", civilStatusList[recordIndex])].ToString();
                        string civilStatusCode = Request.Form[string.Format("data[{0}][CivilStatusCode]", civilStatusList[recordIndex])].ToString();
                        bool hidden = bool.Parse(Request.Form[string.Format("data[{0}][Hidden]", civilStatusList[recordIndex])].ToString());
                        CivilStatus CivilStatus = new CivilStatus()
                        {
                            CivilStatusId = CivilStatusId,
                            CivilStatus1 = CivilStatus1,
                            CivilStatusCode = civilStatusCode,
                            Hidden = hidden
                        };
                        civilStatusUpdateList.Add(CivilStatus);
                    }
                    recordIndex++;
                }
                while (recordIndex < recordCount);

                for (int i = 0; i < civilStatusUpdateList.Count; i++)
                {
                    switch (action)
                    {
                        case "edit":
                            this.CivilStatusService.UpdateCivilStatus(civilStatusUpdateList[i]);
                            break;
                        case "remove":
                            this.CivilStatusService.DeleteCivilStatus(civilStatusUpdateList[i].CivilStatusId);
                            break;
                    }
                }
            }

            return Json(new { data = civilStatusUpdateList }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarTablaReferenciaConcept()
        {
            var conceptList = this.ConceptService.GetAllConcepts();
            var resultado = conceptList.ToList().Select
                (item => new
                {
                    ConceptId = item.ConceptId.ToString(),
                    Concept1 = item.Concept1,
                    ConceptCode = item.ConceptCode,
                    ConceptType = item.ConceptType,
                    Hidden = item.Hidden
                }).ToList();

            return Json(new { data = resultado }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ConceptUpdate(string action)
        {
            var conceptUpdateList = new List<Concept>();
            var conceptList = this.ConceptService.GetAllConcepts().Select(m => m.ConceptId).ToList();
            int recordCount = conceptList.Count;
            int recordIndex = 0;

            if (action == "create")
            {
                string concept1 = Request.Form["data[0][Concept1]"].ToString();
                string conceptCode = Request.Form["data[0][ConceptCode]"].ToString();
                string conceptType = Request.Form["data[0][ConceptType]"].ToString();
                bool hidden = bool.Parse(Request.Form["data[0][Hidden]"].ToString());
                var concept = new Concept()
                {
                    Concept1 = concept1,
                    ConceptCode = conceptCode,
                    ConceptType = conceptType,
                    Hidden = hidden
                };
                conceptUpdateList.Add(concept);
                this.ConceptService.InsertConcept(conceptUpdateList[0]);
            }
            else {
                do
                {
                    if (Request.Form[string.Format("data[{0}][Concept1]", conceptList[recordIndex])] != null)
                    {
                        int conceptId = conceptList[recordIndex];
                        string concept1 = Request.Form[string.Format("data[{0}][Concept1]", conceptList[recordIndex])].ToString();
                        string conceptCode = Request.Form[string.Format("data[{0}][ConceptCode]", conceptList[recordIndex])].ToString();
                        string conceptType = Request.Form[string.Format("data[{0}][ConceptType]", conceptList[recordIndex])].ToString();
                        bool hidden = bool.Parse(Request.Form[string.Format("data[{0}][Hidden]", conceptList[recordIndex])].ToString());
                        Concept concept = new Concept()
                        {
                            ConceptId = conceptId,
                            Concept1 = concept1,
                            ConceptCode = conceptCode,
                            ConceptType = conceptType,
                            Hidden = hidden
                        };
                        conceptUpdateList.Add(concept);
                    }
                    recordIndex++;
                }
                while (recordIndex < recordCount);

                for (int i = 0; i < conceptUpdateList.Count; i++)
                {
                    switch (action)
                    {
                        case "edit":
                            this.ConceptService.UpdateConcept(conceptUpdateList[i]);
                            break;
                        case "remove":
                            this.ConceptService.DeleteConcept(conceptUpdateList[i].ConceptId);
                            break;
                    }
                }
            }

            return Json(new { data = conceptUpdateList }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListarTablaReferenciaBeneficiary()
        {
            var BeneficiaryList = this.BeneficiaryService.GetBeneficiariesAll();
            var resultado = BeneficiaryList.ToList().Select
                (item => new
                {
                    BeneficiaryId = item.BeneficiaryId.ToString()
                }).ToList();

            return Json(new { data = resultado }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListarTablaReferenciaTipoDeRelacion()
        {
            var relationshipTypeList = this.RelationshipTypeService.GetRelationshipTypes();
            var resultado = relationshipTypeList.ToList().Select
                            (item => new
                            {
                                RelationshipTypeId = item.RelationshipTypeId.ToString(),
                                RelationshipType1 = item.RelationshipType1,
                                RelationshipTypeCode = item.RelationshipTypeCode,
                                Handicapped = item.Handicapped,
                                WithChildren = item.WithChildren,
                                WidowCertification = item.WidowCertification,
                                SchoolCertification = item.SchoolCertification,
                                VitalData = item.VitalData,
                                Hidden = item.Hidden
                            }).ToList();

            return Json(new { data = resultado }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RelationshipTypeUpdate(string action)
        {
            List<Nagnoi.SiC.Domain.Core.Model.RelationshipType> relationshipTypeUpdateList = new List<Nagnoi.SiC.Domain.Core.Model.RelationshipType>();
            var relationshipTypeList = this.RelationshipTypeService.GetRelationshipTypes().Select(m => m.RelationshipTypeId).ToList();
            int recordCount = relationshipTypeList.Count;
            int recordIndex = 0;

            if (action == "create")
            {
                string relationshipTypeDescription = Request.Form["data[0][RelationshipType1]"].ToString();
                string relationshipTypeCode = Request.Form["data[0][RelationshipTypeCode]"].ToString();
                bool hidden = bool.Parse(Request.Form["data[0][Hidden]"].ToString());
                bool handicapped = bool.Parse(Request.Form["data[0][Handicapped]"].ToString());
                bool withChildren = bool.Parse(Request.Form["data[0][WithChildren]"].ToString());
                bool widowCertification = bool.Parse(Request.Form["data[0][WidowCertification]"].ToString());
                bool schoolCertification = bool.Parse(Request.Form["data[0][SchoolCertification]"].ToString());
                bool vitalData = bool.Parse(Request.Form["data[0][VitalData]"].ToString());

                RelationshipType relationshipTypeNew = new RelationshipType()
                {
                    RelationshipType1 = relationshipTypeDescription,
                    RelationshipTypeCode = relationshipTypeCode,
                    Handicapped = handicapped,
                    WithChildren = withChildren,
                    WidowCertification = widowCertification,
                    SchoolCertification = schoolCertification,
                    VitalData = vitalData,
                    Hidden = hidden

                };
                relationshipTypeUpdateList.Add(relationshipTypeNew);
                this.RelationshipTypeService.InsertRelationshipType(relationshipTypeUpdateList[0]);
            }
            else
            {
                do
                {
                    if (Request.Form[string.Format("data[{0}][RelationshipType1]", relationshipTypeList[recordIndex])] != null)
                    {
                        int relationshipTypeId = relationshipTypeList[recordIndex];// !String.IsNullOrEmpty(relationshipTypeIdString) ? int.Parse(relationshipTypeIdString) : 0;
                        string relationshipTypeDescription = Request.Form[string.Format("data[{0}][RelationshipType1]", relationshipTypeList[recordIndex])].ToString();
                        string relationshipTypeCode = Request.Form[string.Format("data[{0}][RelationshipTypeCode]", relationshipTypeList[recordIndex])].ToString();
                        bool handicapped = bool.Parse(Request.Form[string.Format("data[{0}][Handicapped]", relationshipTypeList[recordIndex])].ToString());
                        bool withChildren = bool.Parse(Request.Form[string.Format("data[{0}][WithChildren]", relationshipTypeList[recordIndex])].ToString());
                        bool widowCertification = bool.Parse(Request.Form[string.Format("data[{0}][WidowCertification]", relationshipTypeList[recordIndex])].ToString());
                        bool schoolCertification = bool.Parse(Request.Form[string.Format("data[{0}][SchoolCertification]", relationshipTypeList[recordIndex])].ToString());
                        bool vitalData = bool.Parse(Request.Form[string.Format("data[{0}][VitalData]", relationshipTypeList[recordIndex])].ToString());
                        bool hidden = bool.Parse(Request.Form[string.Format("data[{0}][Hidden]", relationshipTypeList[recordIndex])].ToString());
                        RelationshipType relationshipTypeNew = new RelationshipType()
                        {
                            RelationshipTypeId = relationshipTypeId,
                            RelationshipType1 = relationshipTypeDescription,
                            RelationshipTypeCode = relationshipTypeCode,
                            Handicapped = handicapped,
                            WithChildren = withChildren,
                            WidowCertification = widowCertification,
                            SchoolCertification = schoolCertification,
                            VitalData = vitalData,
                            Hidden = hidden
                        };
                        relationshipTypeUpdateList.Add(relationshipTypeNew);
                    }
                    recordIndex++;
                }
                while (recordIndex < recordCount);

                for (int i = 0; i < relationshipTypeUpdateList.Count; i++)
                {
                    switch (action)
                    {
                        case "edit":
                            this.RelationshipTypeService.UpdateRelationshipType(relationshipTypeUpdateList[i]);
                            break;
                        case "remove":
                            this.RelationshipTypeService.DeleteRelationshipType(relationshipTypeUpdateList[i].RelationshipTypeId);
                            break;
                    }
                }

            }
            return Json(new { data = relationshipTypeUpdateList }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListarTablaReferenciaCategoriaRelacion()
        {
            var relationshipCategoryList = this.RelationshipCategoryService.GetRelationshipCategories();

            var resultado = relationshipCategoryList.ToList().Select
                            (item => new
                            {
                                RelationshipCategoryId = item.RelationshipCategoryId.ToString(),
                                RelationshipCategory1 = item.RelationshipCategory1,
                                Hidden = item.Hidden
                            }).ToList();

            return Json(new { data = resultado }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RelationshipCategoryUpdate(string action)
        {
            List<RelationshipCategory> relationshipCategoryUpdateList = new List<RelationshipCategory>();
            var relationshipCategoryList = this.RelationshipCategoryService.GetRelationshipCategories().Select(m => m.RelationshipCategoryId).ToList();
            int recordCount = relationshipCategoryList.Count;
            int recordIndex = 0;

            if (action == "create")
            {
                string relationshipCategoryDescription = Request.Form["data[0][RelationshipCategory1]"].ToString();
                bool hidden = bool.Parse(Request.Form["data[0][Hidden]"].ToString());

                RelationshipCategory relationshipCategoryNew = new RelationshipCategory()
                {
                    RelationshipCategory1 = relationshipCategoryDescription,
                    Hidden = hidden
                };
                relationshipCategoryUpdateList.Add(relationshipCategoryNew);
                this.RelationshipCategoryService.InsertRelationshipCategory(relationshipCategoryUpdateList[0]);
            }
            else
            {
                do
                {
                    if (Request.Form[string.Format("data[{0}][RelationshipCategory1]", relationshipCategoryList[recordIndex])] != null)
                    {
                        int relationshipCategoryId = relationshipCategoryList[recordIndex];
                        string RelationshipCategoryDescription = Request.Form[string.Format("data[{0}][RelationshipCategory1]", relationshipCategoryList[recordIndex])].ToString();
                        bool hidden = bool.Parse(Request.Form[string.Format("data[{0}][Hidden]", relationshipCategoryList[recordIndex])].ToString());
                        RelationshipCategory relationshipCategoryNew = new RelationshipCategory()
                        {
                            RelationshipCategoryId = relationshipCategoryId,
                            RelationshipCategory1 = RelationshipCategoryDescription,
                            Hidden = hidden
                        };
                        relationshipCategoryUpdateList.Add(relationshipCategoryNew);
                    }
                    recordIndex++;
                }
                while (recordIndex < recordCount);

                for (int i = 0; i < relationshipCategoryUpdateList.Count; i++)
                {
                    switch (action)
                    {
                        case "edit":
                            this.RelationshipCategoryService.UpdateRelationshipCategory(relationshipCategoryUpdateList[i]);
                            break;
                        case "remove":
                            this.RelationshipCategoryService.DeleteRelationshipCategory(relationshipCategoryUpdateList[i].RelationshipCategoryId);
                            break;
                    }
                }

            }
            return Json(new { data = relationshipCategoryUpdateList }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
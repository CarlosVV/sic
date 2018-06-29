using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using SAP.Middleware.Connector;
using Nagnoi.SiC.Domain.Core.Model;
using Nagnoi.SiC.Infrastructure.Core.Log;
using Nagnoi.SiC.Infrastructure.Core.Helpers;

namespace SIC.SapInterfaceConsole.Modules {
    public class BAPI : BaseService {

        private Dictionary<string, string> caseTobeProcess = new Dictionary<string, string>();

        public void Init() {
            try {
                Dictionary<string, string> dicVendor = new Dictionary<string, string>();
                Dictionary<string, string> dicPayment = new Dictionary<string, string>();
                Dictionary<string, string> dicPaymentDoc = new Dictionary<string, string>();
                HashSet<SapTransaction> listOfTransactions = new HashSet<SapTransaction>();

                IEnumerable<Payment> payments = PaymentService.FindPaymentsToSAP();

                if (payments.IsNull()) {
                    Console.WriteLine("No se encontraron pagos para enviar a SAP");
                    Logger.Debug("No se encontraron pagos para enviar a SAP");
                    return;
                }
                Console.WriteLine("Pagos encontrados, generando información de pagos para ser enviada.....");

                //Console.WriteLine("Nro: " + payments.Count().ToString());                

                foreach (var item in payments) {
                    
                    Console.WriteLine(String.Format("{0} {1} {2}", "Inicio procesamiento pago SAP para el Case Number: ", item.CaseDetail.CaseNumber, item.CaseDetail.CaseKey));

                    //Generar Log
                    MapVendor(ref dicVendor, item.CaseDetail);
                    MapPayment(ref dicPayment, item);
                    MapPaymentDoc(ref dicPaymentDoc, item);
                    listOfTransactions.Add(GenerateSapTransactionLog(dicVendor, dicPayment, dicPaymentDoc));

                    Console.WriteLine("Log Generado");
                    //Fin Generar Log


                    //Inicio Enviar Vendor/Payment/Payment Doc a SAP

                    //Fin Enviar Vendor/Payment/Payment Doc a SAP
                    ZFI_VENDOR_CREATION(item.CaseDetail, item.Remitter);
                    Console.WriteLine(String.Format("{0} {1} {2} {3} {4}", "Pago enviado para el Case Number: ", item.CaseDetail.CaseNumber, item.CaseDetail.CaseKey, "por el Concepto de: ", !item.CaseDetail.Case.Concept.IsNull() ? item.CaseDetail.Case.Concept.Concept1 : string.Empty));

                    Console.WriteLine(String.Format("{0} {1} {2}", "Fin procesamiento pago SAP para el Case Number: ", item.CaseDetail.CaseNumber, item.CaseDetail.CaseKey));
                }

                //Crear Log
                SapTransactionService.CreateBulkSapTransaction(listOfTransactions.ToList());                

            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                Logger.Error(ex.Message, ex);
            }
        }

        #region SAP Methods
        private void ZFI_AP_DOC_POSTING(Payment paymentToBeProcess) {

            RfcDestination rfcDestination = GetParameters();
            RfcRepository repo = rfcDestination.Repository;

            bool isLawyer = false;

            if (paymentToBeProcess.CaseDetail.Entity.ParticipantType.ParticipantType1.ToUpper().Equals("ABOGADO", StringComparison.InvariantCultureIgnoreCase))
                isLawyer = true;

            IRfcFunction docPostingDoc = repo.CreateFunction("ZFI_AP_DOC_POSTING");

            docPostingDoc.SetValue("IM_INTERFACE_CODE", "ITF-DOCPST");

            docPostingDoc.SetValue("IM_FLATFILE", "N");

            IRfcTable idRange = docPostingDoc.GetTable("TBL_AP_DOCPOST");

            idRange.Insert();

            idRange.SetValue("GL_INVOICE_GRP", "1");
            idRange.SetValue("BLDAT", DateTime.UtcNow);
            idRange.SetValue("BUDAT", DateTime.UtcNow);
            idRange.SetValue("BLART", "ZX");
            idRange.SetValue("BUKRS", SettingService.GetSettingValueByName("Interface.SAP.CompanyCode"));
            idRange.SetValue("WAERS", SettingService.GetSettingValueByName("Interface.SAP.CurrencyKey"));
            idRange.SetValue("XBLNR", "Texto de prueba");
            idRange.SetValue("BKTXT", "Texto de prueba header");
            idRange.SetValue("NEWKO", "2010010030");
            idRange.SetValue("WRBTR", paymentToBeProcess.Amount.Value);
            idRange.SetValue("KOSTL", "1003301004");
            idRange.SetValue("AUFNR", "");
            idRange.SetValue("PRCTR", "");
            idRange.SetValue("ZUONR", "20135625527");
            idRange.SetValue("BUKRS1", SettingService.GetSettingValueByName("Interface.SAP.CompanyCode"));
            idRange.SetValue("GL_FUNC_AREA", "PBL");
            idRange.SetValue("FUND", "1");
            idRange.SetValue("FIPEX", "AP");
            idRange.SetValue("FISTL", "10033010");

            idRange.Append();

            idRange.SetValue("GL_JOUR_GRP", "1");
            idRange.SetValue("BLDAT", DateTime.UtcNow);
            idRange.SetValue("BUDAT", DateTime.UtcNow);
            idRange.SetValue("BLART", "ZM");
            idRange.SetValue("BUKRS", SettingService.GetSettingValueByName("Interface.SAP.CompanyCode"));
            idRange.SetValue("WAERS", SettingService.GetSettingValueByName("Interface.SAP.CurrencyKey"));
            idRange.SetValue("XBLNR", "Texto de prueba");
            idRange.SetValue("BKTXT", "Texto de prueba header");
            idRange.SetValue("NEWKO", String.Format("{0} {1}", paymentToBeProcess.CaseNumber, paymentToBeProcess.CaseKey));
            idRange.SetValue("WRBTR", -1 * paymentToBeProcess.Amount.Value);
            idRange.SetValue("KOSTL", "1003301004");
            idRange.SetValue("AUFNR", "");
            idRange.SetValue("PRCTR", "");
            idRange.SetValue("ZUONR", "20135625527");
            idRange.SetValue("BUKRS1", SettingService.GetSettingValueByName("Interface.SAP.CompanyCode"));
            idRange.SetValue("GL_FUNC_AREA", "PBL");
            idRange.SetValue("FUND", "1");
            idRange.SetValue("FIPEX", "AP");
            idRange.SetValue("FISTL", "10033010");

            //add selection range to customerList function to search for all customers            

            //IRfcTable addressData = docPosting.GetTable("AddressData");
            docPostingDoc.Invoke(rfcDestination);

            //IRfcFunction docPosting2 = repo.CreateFunction("ZFI_GL_PROCESSING");         

            IRfcTable idRange2 = docPostingDoc.GetTable("TBL_MESSAGE_LOG");
            docPostingDoc.Invoke(rfcDestination);

            for (int cuIndex = 0; cuIndex < idRange2.RowCount; cuIndex++) {

                idRange2.CurrentIndex = cuIndex;
                Console.WriteLine(idRange2.GetString("OUTPUT3"));
            }

            Console.ReadLine();
        }

        private void ZFI_VENDOR_CREATION(CaseDetail caseTobeProcess, Entity remitterTobeProcess) {

            RfcDestination rfcDestination = GetParameters();

            RfcRepository repoVendor = rfcDestination.Repository;

            bool isLawyer = false;

            //if (remitterTobeProcess.ParticipantType.ParticipantType1.ToUpper().Equals("ABOGADO", StringComparison.InvariantCultureIgnoreCase))
                isLawyer = true;

            IRfcFunction docVendor = repoVendor.CreateFunction("ZFI_VENDOR_CREATION");

            docVendor.SetValue("IM_INTERFACE_CODE", "ITF-VENDOR");
            docVendor.SetValue("IM_FLAT_FILE_FLAG", "N");

            IRfcTable idTable = docVendor.GetTable("TBL_SOURCE_DATA");

            idTable.Insert();

            //idTable.SetValue("LIFNR", String.Format("{0}{1}", caseTobeProcess.CaseNumber, caseTobeProcess.CaseKey));
            idTable.SetValue("LIFNR", "");
            idTable.SetValue("BUKRS", SettingService.GetSettingValueByName("Interface.SAP.CompanyCode"));
            idTable.SetValue("KTOKK", isLawyer ? "ZABO" : "ZCOM");
            idTable.SetValue("NAME1", "Prueba");
            idTable.SetValue("NAME2", "Prueba");
            idTable.SetValue("LAST_NAME1", "Prueba");
            idTable.SetValue("LAST_NAME2", "Prueba");
            idTable.SetValue("SORT1", isLawyer ? "HONORARIOS" : "COMPENSACIONES");

            var currentAddress = remitterTobeProcess.Addresses.Where(a => a.AddressType.AddressType1.ToUpper().Equals("POSTAL", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

            idTable.SetValue("STREET", currentAddress.IsNull() ? string.Empty : currentAddress.Line1);
            idTable.SetValue("STR_SUPPL1", currentAddress.IsNull() ? string.Empty : currentAddress.Line2);
            idTable.SetValue("CITY", currentAddress.IsNull() ? string.Empty : (currentAddress.City.IsNull() ? string.Empty : "New York"));
            idTable.SetValue("ZIP_CODE", currentAddress.IsNull() ? string.Empty : "10001");
            idTable.SetValue("COUNTRY", currentAddress.IsNull() ? string.Empty : (currentAddress.Country.IsNull() ? string.Empty : currentAddress.Country.CountryKey));
            idTable.SetValue("REGION", currentAddress.IsNull() ? string.Empty : (currentAddress.State.IsNull() ? string.Empty : currentAddress.Country.CountryKey.Equals("PR") ? currentAddress.State.State1.Substring(1, 3) : "NY"));
            idTable.SetValue("LANGU", SettingService.GetSettingValueByName("Interface.SAP.LanguageKey"));
            idTable.SetValue("AKONT", isLawyer ? "2030050090" : "2030050090");
            idTable.SetValue("ZTERM", SettingService.GetSettingValueByName("Interface.SAP.TermsOfPaymentKey"));
            idTable.SetValue("ZWELS", "C");
            idTable.SetValue("ZAHLS", "A");

            var currentPhone = remitterTobeProcess.Phones.Where(p => !p.PhoneNumber.IsNullOrEmpty()).FirstOrDefault();

            idTable.SetValue("TEL_NUMBER", currentPhone.IsNull() ? "Prueba" : currentPhone.PhoneNumber ?? "Prueba");
            idTable.SetValue("STCD1", "580000002" ?? "Prueba");
            idTable.SetValue("WAERS", SettingService.GetSettingValueByName("Interface.SAP.CurrencyKey"));
            idTable.SetValue("WITHT", isLawyer ? "A7" : string.Empty);
            idTable.SetValue("WT_WITHCD", isLawyer ? "07" : string.Empty);
            idTable.SetValue("QLAND", isLawyer ? "PR" : string.Empty);
            idTable.SetValue("WT_SUBJCT", isLawyer ? "X" : string.Empty);
            idTable.SetValue("FDGRV", isLawyer ? "A010" : "A029");
            idTable.SetValue("Birthdate", remitterTobeProcess.BirthDate.HasValue ? remitterTobeProcess.BirthDate.Value.ToString("YYYYMMDD") : string.Empty);
            idTable.SetValue("DeadDate", remitterTobeProcess.DeceaseDate.HasValue ? remitterTobeProcess.DeceaseDate.Value.ToString("YYYYMMDD") : string.Empty);
            idTable.SetValue("Gender", remitterTobeProcess.Gender.IsNull() ? "U" : remitterTobeProcess.Gender.GenderCode);

            docVendor.Invoke(rfcDestination);

            IRfcTable tblOuput = docVendor.GetTable("TBL_MESSAGE_LOG");
            docVendor.Invoke(rfcDestination);

            if (tblOuput.RowCount == 0)
                Console.WriteLine("Parece que todo fue bien con el Case Number: " + caseTobeProcess.CaseNumber + " " + caseTobeProcess.CaseKey);
            else {
                Console.WriteLine("Oops!, error al procesar el pago del Case Number: " + caseTobeProcess.CaseNumber + " " + caseTobeProcess.CaseKey);
                Console.WriteLine("Empezamos a mostrar los errores......");
            }

            for (int cuIndex = 0; cuIndex < tblOuput.RowCount; cuIndex++) {
                tblOuput.CurrentIndex = cuIndex;
                Console.WriteLine(tblOuput.GetString("OUTPUT3"));
            }            
        }

        private void ZFI_GL_PROCESSING(Payment paymentTobeProcess) {
            RfcDestination rfcDestination = GetParameters();
            RfcRepository repo = rfcDestination.Repository;

            IRfcFunction docPosting = repo.CreateFunction("ZFI_GL_PROCESSING");

            docPosting.SetValue("IM_INTERFACE_CODE", "ITF-GLPROC");

            docPosting.SetValue("IM_FLATFILE", "N");

            IRfcTable idRange = docPosting.GetTable("TBL_JOURNAL");

            idRange.Insert();

            idRange.SetValue("GL_JOUR_GRP", "1");
            idRange.SetValue("BLDAT", DateTime.UtcNow);
            idRange.SetValue("BUDAT", DateTime.UtcNow);
            idRange.SetValue("BLART", "ZX");
            idRange.SetValue("BUKRS", SettingService.GetSettingValueByName("Interface.SAP.CompanyCode"));
            idRange.SetValue("WAERS", SettingService.GetSettingValueByName("Interface.SAP.CurrencyKey"));
            idRange.SetValue("XBLNR", "Texto de prueba");
            idRange.SetValue("BKTXT", "Texto de prueba header");
            idRange.SetValue("NEWKO", "2010010030");
            idRange.SetValue("WRBTR", paymentTobeProcess.Amount.Value);
            idRange.SetValue("KOSTL", "1003301004");
            idRange.SetValue("AUFNR", "");
            idRange.SetValue("PRCTR", "");
            idRange.SetValue("ZUONR", "20135625527");
            idRange.SetValue("BUKRS1", SettingService.GetSettingValueByName("Interface.SAP.CompanyCode"));
            idRange.SetValue("GL_FUNC_AREA", "PBL");
            idRange.SetValue("FUND", "1");
            idRange.SetValue("FIPEX", "AP");
            idRange.SetValue("FISTL", "10033010");

            idRange.Append();

            idRange.SetValue("GL_JOUR_GRP", "1");
            idRange.SetValue("BLDAT", DateTime.UtcNow);
            idRange.SetValue("BUDAT", DateTime.UtcNow);
            idRange.SetValue("BLART", "ZM");
            idRange.SetValue("BUKRS", SettingService.GetSettingValueByName("Interface.SAP.CompanyCode"));
            idRange.SetValue("WAERS", SettingService.GetSettingValueByName("Interface.SAP.CurrencyKey"));
            idRange.SetValue("XBLNR", "Texto de prueba");
            idRange.SetValue("BKTXT", "Texto de prueba header");
            idRange.SetValue("NEWKO", String.Format("{0} {1}", paymentTobeProcess.CaseNumber, paymentTobeProcess.CaseKey));
            idRange.SetValue("WRBTR", -1 * paymentTobeProcess.Amount.Value);
            idRange.SetValue("KOSTL", "1003301004");
            idRange.SetValue("AUFNR", "");
            idRange.SetValue("PRCTR", "");
            idRange.SetValue("ZUONR", "20135625527");
            idRange.SetValue("BUKRS1", SettingService.GetSettingValueByName("Interface.SAP.CompanyCode"));
            idRange.SetValue("GL_FUNC_AREA", "PBL");
            idRange.SetValue("FUND", "1");
            idRange.SetValue("FIPEX", "AP");
            idRange.SetValue("FISTL", "10033010");

            //add selection range to customerList function to search for all customers            

            //IRfcTable addressData = docPosting.GetTable("AddressData");
            docPosting.Invoke(rfcDestination);

            //IRfcFunction docPosting2 = repo.CreateFunction("ZFI_GL_PROCESSING");         

            IRfcTable idRange2 = docPosting.GetTable("TBL_MESSAGE_LOG");
            docPosting.Invoke(rfcDestination);

            for (int cuIndex = 0; cuIndex < idRange2.RowCount; cuIndex++) {

                idRange2.CurrentIndex = cuIndex;
                Console.WriteLine(idRange2.GetString("OUTPUT3"));
            }

            Console.ReadLine();
        }

        private RfcDestination GetParameters() {

            RfcConfigParameters parameters = new RfcConfigParameters();

            parameters[RfcConfigParameters.Name] = SettingService.GetSettingValueByName("Interface.SAP.Connection.Name");
            parameters[RfcConfigParameters.User] = SettingService.GetSettingValueByName("Interface.SAP.Connection.User");
            parameters[RfcConfigParameters.Password] = SettingService.GetSettingValueByName("Interface.SAP.Connection.Password");
            parameters[RfcConfigParameters.Client] = SettingService.GetSettingValueByName("Interface.SAP.Connection.Client");
            parameters[RfcConfigParameters.Language] = SettingService.GetSettingValueByName("Interface.SAP.Connection.Language");
            parameters[RfcConfigParameters.AppServerHost] = SettingService.GetSettingValueByName("Interface.SAP.Connection.ServerHost");
            parameters[RfcConfigParameters.SystemNumber] = SettingService.GetSettingValueByName("Interface.SAP.Connection.SystemNumber");
            parameters[RfcConfigParameters.PoolSize] = SettingService.GetSettingValueByName("Interface.SAP.Connection.PoolSize");
            parameters[RfcConfigParameters.PeakConnectionsLimit] = SettingService.GetSettingValueByName("Interface.SAP.Connection.PeakConnectionsLimit");
            parameters[RfcConfigParameters.IdleTimeout] = SettingService.GetSettingValueByName("Interface.SAP.Connection.IdleTimeout");

            RfcDestination destination = RfcDestinationManager.GetDestination(parameters);

            return destination;
        }

        #endregion

        #region Log
        private SapTransaction GenerateSapTransactionLog(Dictionary<string, string> caseToBeProcess, Dictionary<string, string> paymentToBeProcess, Dictionary<string, string> paymentDocToBeProcess) {

            if (caseToBeProcess.IsNull())
                throw new ArgumentNullException();

            string log = XmlHelper.SerializeKeyValuePairs("VENDOR", caseToBeProcess);

            return new SapTransaction {
                 TransactionDetail = log,
                 CreatedBy = "SAP Interface",
                 CreatedDateTime = DateTime.UtcNow,
                 ModifiedBy = "SAP Interface",
                 ModifiedDateTime = DateTime.UtcNow
            };
        }
        #endregion

        #region Mapping SAP Entities to Dictionary
        private void MapVendor(ref Dictionary<string, string> dicVendor, CaseDetail caseTobeProcess) {

            if (dicVendor.IsNull())
                dicVendor = new Dictionary<string, string>();

            if (caseTobeProcess.IsNull())
                throw new ArgumentNullException();

            dicVendor.Add("FUNCTION", "ZFI_VENDOR_CREATION");
            dicVendor.Add("IM_INTERFACE_CODE", "ITF_VENDOR");
            dicVendor.Add("IM_FLAT_FILE_FLAG", "N");
            dicVendor.Add("TABLE", "TBL_SOURCE_DATA");
            dicVendor.Add("LIFNR", String.Format("{0}{1}", caseTobeProcess.CaseNumber, caseTobeProcess.CaseKey));
            dicVendor.Add("BUKRS", "1000");
            dicVendor.Add("KTOKK", caseTobeProcess.Entity.ParticipantType.ParticipantType1.ToUpper().Equals("ABOGADO", StringComparison.InvariantCultureIgnoreCase) ? "ZABO" : "ZCOM");
            dicVendor.Add("NAME1", caseTobeProcess.Entity.FirstName);
            dicVendor.Add("NAME2", caseTobeProcess.Entity.MiddleName);
            dicVendor.Add("LNAME1", caseTobeProcess.Entity.LastName);
            dicVendor.Add("LNAME2", caseTobeProcess.Entity.SecondLastName);
            dicVendor.Add("SORT1", caseTobeProcess.Entity.ParticipantType.ParticipantType1.ToUpper().Equals("ABOGADO", StringComparison.InvariantCultureIgnoreCase) ? "HONORARIOS" : "COMPENSACIONES");

            var currentAddress = caseTobeProcess.Entity.Addresses.Where(a => a.AddressType.AddressType1.ToUpper().Equals("POSTAL", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

            dicVendor.Add("STREET", currentAddress.IsNull() ? string.Empty : currentAddress.Line1);
            dicVendor.Add("STR_SUPPL1", currentAddress.IsNull() ? string.Empty : currentAddress.Line2);
            dicVendor.Add("CITY", currentAddress.IsNull() ? string.Empty : (currentAddress.City.IsNull() ? string.Empty : currentAddress.City.City1));
            dicVendor.Add("ZIP_CODE", currentAddress.IsNull() ? string.Empty : currentAddress.ZipCode);
            dicVendor.Add("COUNTRY", currentAddress.IsNull() ? string.Empty : (currentAddress.Country.IsNull() ? string.Empty : currentAddress.Country.Country1));
            dicVendor.Add("REGION", currentAddress.IsNull() ? string.Empty : (currentAddress.State.IsNull() ? string.Empty : currentAddress.State.State1));
            dicVendor.Add("LANGU", "E");
            dicVendor.Add("AKONT", caseTobeProcess.Entity.ParticipantType.ParticipantType1.ToUpper().Equals("ABOGADO", StringComparison.InvariantCultureIgnoreCase) ? "2030050090" : "2010010010");
            dicVendor.Add("ZTERM", "0001");
            dicVendor.Add("ZWELS", "0001");

            var currentPhone = caseTobeProcess.Entity.Phones.Where(p => !p.PhoneNumber.IsNullOrEmpty()).FirstOrDefault();

            dicVendor.Add("TEL_NUMBER", currentPhone.IsNull() ? string.Empty : currentPhone.PhoneNumber);
            dicVendor.Add("STCD1", "580000001");
            dicVendor.Add("WAERS", "USD");
            dicVendor.Add("WITHT", caseTobeProcess.Entity.ParticipantType.ParticipantType1.ToUpper().Equals("ABOGADO", StringComparison.InvariantCultureIgnoreCase) ? "A7" : string.Empty);
            dicVendor.Add("WT_WITHCD", caseTobeProcess.Entity.ParticipantType.ParticipantType1.ToUpper().Equals("ABOGADO", StringComparison.InvariantCultureIgnoreCase) ? "07" : string.Empty);
            dicVendor.Add("QLAND", caseTobeProcess.Entity.ParticipantType.ParticipantType1.ToUpper().Equals("ABOGADO", StringComparison.InvariantCultureIgnoreCase) ? "PR" : string.Empty);
            dicVendor.Add("WT_SUBJCT", caseTobeProcess.Entity.ParticipantType.ParticipantType1.ToUpper().Equals("ABOGADO", StringComparison.InvariantCultureIgnoreCase) ? "X" : string.Empty);
            dicVendor.Add("Birthdate", caseTobeProcess.Entity.BirthDate.HasValue ? caseTobeProcess.Entity.BirthDate.Value.ToString("YYYYMMDD") : string.Empty);
            dicVendor.Add("DeadDate", caseTobeProcess.Entity.DeceaseDate.HasValue ? caseTobeProcess.Entity.DeceaseDate.Value.ToString("YYYYMMDD") : string.Empty);
            dicVendor.Add("Gender", caseTobeProcess.Entity.Gender.IsNull() ? "U" : caseTobeProcess.Entity.Gender.GenderCode);
        }
        private void MapPaymentDoc(ref Dictionary<string, string> dicPaymentDoc, Payment paymentTobeProcess) {
        }
        private void MapPayment(ref Dictionary<string, string> dicPayment, Payment paymentTobeProcess) {
        }
        #endregion
    }
}

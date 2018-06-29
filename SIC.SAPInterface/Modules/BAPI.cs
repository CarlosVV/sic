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

namespace SIC.SAPInterface.Modules {
    public class BAPI : BaseService {

        public void Init() {
            try {
                Dictionary<string, string> dicVendor = new Dictionary<string, string>();

                var payments = PaymentService.FindPaymentsToSAP();                

                if (payments.IsNull()) {
                    Logger.Debug("No se encontraron pagos para enviar a SAP");
                    return;
                }

                foreach (var item in payments) {
                    MapCaseDetailToVendor(ref dicVendor, item.CaseDetail);

                    CreateLog(dicVendor, item);

                    ZFI_VENDOR_CREATION(item.CaseDetail);
                }

            } catch(Exception ex) {
                Logger.Error(ex.Message, ex);
            }
        }

        public Dictionary<string, string> caseTobeProcess = new Dictionary<string, string>();

        public void ZFI_AP_DOC_POSTING() {

            RfcDestination rfcDestination = GetParameters();
            
            rfcDestination.Ping();                                  
        }

        public void ZFI_VENDOR_CREATION(CaseDetail caseTobeProcess) {

            RfcDestination rfcDestination = GetParameters();

            RfcRepository repoVendor = rfcDestination.Repository;

            bool isLawyer = false;

            if (caseTobeProcess.Entity.ParticipantType.ParticipantType1.ToUpper().Equals("ABOGADO", StringComparison.InvariantCultureIgnoreCase))
                isLawyer = true;

            IRfcFunction docVendor = repoVendor.CreateFunction("ZFI_VENDOR_CREATION");

            docVendor.SetValue("IM_INTERFACE_CODE", "ITF-GLPROC");
            docVendor.SetValue("IM_FLATFILE", "N");

            IRfcTable idTable = docVendor.GetTable("TBL_SOURCE_DATA");

            idTable.Insert();

            idTable.SetValue("LIFNR", String.Format("{0}{1}", caseTobeProcess.CaseNumber, caseTobeProcess.CaseKey));
            idTable.SetValue("BUKRS", "1000");
            idTable.SetValue("KTOKK", isLawyer ? "ZABO" : "ZCOM");
            idTable.SetValue("NAME1", caseTobeProcess.Entity.FirstName);
            idTable.SetValue("NAME2", caseTobeProcess.Entity.MiddleName);
            idTable.SetValue("LNAME1", caseTobeProcess.Entity.LastName);
            idTable.SetValue("LNAME2", caseTobeProcess.Entity.SecondLastName);
            idTable.SetValue("SORT1", isLawyer ? "HONORARIOS" : "COMPENSACIONES");

            var currentAddress = caseTobeProcess.Entity.Addresses.Where(a => a.AddressType.AddressType1.ToUpper().Equals("POSTAL", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

            idTable.SetValue("STREET", currentAddress.IsNull()? string.Empty: currentAddress.Line1);
            idTable.SetValue("STR_SUPPL1", currentAddress.IsNull() ? string.Empty : currentAddress.Line2);
            idTable.SetValue("CITY", currentAddress.IsNull() ? string.Empty : (currentAddress.City.IsNull() ? string.Empty : currentAddress.City.City1));
            idTable.SetValue("ZIP_CODE", currentAddress.IsNull() ? string.Empty : currentAddress.ZipCode);
            idTable.SetValue("COUNTRY", currentAddress.IsNull() ? string.Empty : (currentAddress.Country.IsNull() ? string.Empty : currentAddress.Country.Country1));
            idTable.SetValue("REGION", currentAddress.IsNull() ? string.Empty : (currentAddress.State.IsNull() ? string.Empty : currentAddress.State.State1));
            idTable.SetValue("LANGU", "E");
            idTable.SetValue("AKONT", isLawyer ? "2030050090" : "2010010010");
            idTable.SetValue("ZTERM", "0001");
            idTable.SetValue("ZWELS", "0001");

            var currentPhone = caseTobeProcess.Entity.Phones.Where(p => !p.PhoneNumber.IsNullOrEmpty()).FirstOrDefault();
           
            idTable.SetValue("TEL_NUMBER", currentPhone.IsNull()? string.Empty: currentPhone.PhoneNumber);
            idTable.SetValue("STCD1", caseTobeProcess.Entity.SSN);
            idTable.SetValue("WAERS", "USD");
            idTable.SetValue("WITHT", isLawyer ? "A7" : string.Empty);
            idTable.SetValue("WT_WITHCD", isLawyer ? "07" : string.Empty);
            idTable.SetValue("QLAND", isLawyer ? "PR" : string.Empty);
            idTable.SetValue("WT_SUBJCT", isLawyer ? "X" : string.Empty);
            idTable.SetValue("FDGRV", isLawyer ? "A010" : "A029");

            docVendor.Invoke(rfcDestination);
        }

        public void ZFI_GL_PROCESSING() {
            RfcDestination rfcDestination = GetParameters();

            rfcDestination.Ping();
        }

        private RfcDestination GetParameters() {
                
            RfcConfigParameters parameters = new RfcConfigParameters();
            
            parameters[RfcConfigParameters.User] = System.Configuration.ConfigurationManager.AppSettings["User"];
            parameters[RfcConfigParameters.Password] = System.Configuration.ConfigurationManager.AppSettings["Password"];
            parameters[RfcConfigParameters.Client] = System.Configuration.ConfigurationManager.AppSettings["Client"];
            parameters[RfcConfigParameters.Language] = System.Configuration.ConfigurationManager.AppSettings["Language"];
            parameters[RfcConfigParameters.AppServerHost] = System.Configuration.ConfigurationManager.AppSettings["ServerHost"];
            parameters[RfcConfigParameters.SystemNumber] = System.Configuration.ConfigurationManager.AppSettings["SystemNumber"];
            parameters[RfcConfigParameters.PoolSize] = System.Configuration.ConfigurationManager.AppSettings["PoolSize"];
            parameters[RfcConfigParameters.PeakConnectionsLimit] = System.Configuration.ConfigurationManager.AppSettings["PeakConnectionsLimit"];
            parameters[RfcConfigParameters.IdleTimeout] = System.Configuration.ConfigurationManager.AppSettings["IdleTimeout"];

            RfcDestination destination = RfcDestinationManager.GetDestination(parameters);

            return destination;
        }

        public void CreateLog(Dictionary<string, string> caseToBeProcess, Payment paymentsToBeProcess) {

            if (caseToBeProcess.IsNull())
                throw new ArgumentNullException();

            XmlHelper.SerializeKeyValuePairs("VENDOR", caseToBeProcess);

        }

        public void MapCaseDetailToVendor(ref Dictionary<string, string> dicVendor, CaseDetail caseTobeProcess) {

            if (dicVendor.IsNull())
                dicVendor = new Dictionary<string, string>();

            if (caseTobeProcess.IsNull())
                throw new ArgumentNullException();

            dicVendor.Add("FUNCTION", "ZFI_VENDRO_CREATION");            
            dicVendor.Add("IM_INTERFACE_CODE", "ITF-GLPROC");
            dicVendor.Add("IM_FLATFILE", "N");
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
            dicVendor.Add("STCD1", caseTobeProcess.Entity.SSN);
            dicVendor.Add("WAERS", "USD");
            dicVendor.Add("WITHT", caseTobeProcess.Entity.ParticipantType.ParticipantType1.ToUpper().Equals("ABOGADO", StringComparison.InvariantCultureIgnoreCase) ? "A7" : string.Empty);
            dicVendor.Add("WT_WITHCD", caseTobeProcess.Entity.ParticipantType.ParticipantType1.ToUpper().Equals("ABOGADO", StringComparison.InvariantCultureIgnoreCase) ? "07" : string.Empty);
            dicVendor.Add("QLAND", caseTobeProcess.Entity.ParticipantType.ParticipantType1.ToUpper().Equals("ABOGADO", StringComparison.InvariantCultureIgnoreCase) ? "PR" : string.Empty);
            dicVendor.Add("WT_SUBJCT", caseTobeProcess.Entity.ParticipantType.ParticipantType1.ToUpper().Equals("ABOGADO", StringComparison.InvariantCultureIgnoreCase) ? "X" : string.Empty);
        }
    }
}

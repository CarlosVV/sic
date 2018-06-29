namespace Nagnoi.SiC.Web
{
    #region References

    using System.Web.Optimization;

    #endregion

    public class BundleConfig
    {
        #region Private Methods

        private static void RegisterStyles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/content/base/css").Include(
                      "~/content/bootstrap.min.css",
                      "~/content/jquery.bootgrid.min.css",
                      "~/content/font-awesome.min.css",
                      "~/content/toastr.min.css"));

            bundles.Add(new StyleBundle("~/content/datatables/css").Include(
                        "~/content/DataTables-1.10.12/media/css/dataTables.bootstrap.min.css",
                        "~/content/DataTables-1.10.12/extensions/buttons/css/buttons.bootstrap.min.css",
                        "~/content/DataTables-1.10.12/extensions/select/css/select.bootstrap.min.css",
                        "~/content/site.css"));

            bundles.Add(new StyleBundle("~/content/datatableseditor").Include(
                        "~/content/DataTables-1.10.12/extensions/Editor/css/editor.dataTables.min.css"
                        ));

            bundles.Add(new StyleBundle("~/content/datatablesextended").Include(
                "~/content/DataTables-1.10.12/media/css/jquery.dataTables.min.css",
                "~/content/DataTables-1.10.12/media/css/dataTables.bootstrap.css",
                "~/content/DataTables-1.10.12/extensions/Select/css/buttons.dataTables.min.css",
                "~/content/DataTables-1.10.12/extensions/Select/css/select.dataTables.min.css"
            ));

            bundles.Add(new StyleBundle("~/bundles/fileinput/css").Include(
                   "~/content/bootstrap-fileinput/css/fileinput.min.css"));

            bundles.Add(new StyleBundle("~/bundles/datepicker/css").Include(
                "~/content/bootstrap-datepicker3.min.css"));

            bundles.Add(new StyleBundle("~/content/PaymentCertification").Include(
                "~/content/bootstrap.min.css",
                "~/content/font-awesome.min.css",
                "~/content/bootstrap-datepicker3.min.css",
                "~/content/DataTables-1.10.12/media/css/dataTables.bootstrap.min.css",
                "~/content/DataTables-1.10.12/extensions/buttons/css/buttons.bootstrap.min.css",
                "~/content/DataTables-1.10.12/extensions/select/css/select.bootstrap.min.css",
                "~/content/DataTables-1.10.12/extensions/Editor/css/editor.dataTables.min.css",
                "~/content/site.css"
                ));

            bundles.Add(new StyleBundle("~/bundles/tablereferences/css").Include(
                "~/content/bootstrap.min.css",
                "~/content/font-awesome.min.css",
                "~/content/DataTables-1.10.12/media/css/dataTables.bootstrap.min.css",
                "~/content/DataTables-1.10.12/extensions/buttons/css/buttons.bootstrap.min.css",
                "~/content/DataTables-1.10.12/extensions/select/css/select.bootstrap.min.css",
                "~/content/DataTables-1.10.12/extensions/Editor/css/editor.dataTables.min.css",
                "~/content/site.css")
                );

            bundles.Add(new StyleBundle("~/bundles/thirdparties/css").Include(
                      "~/content/bootstrap.min.css",
                      "~/content/font-awesome.min.css",
                      "~/content/bootstrap-datepicker3.min.css",
                      "~/content/DataTables-1.10.12/media/css/dataTables.bootstrap.min.css",
                      "~/content/DataTables-1.10.12/extensions/buttons/css/buttons.bootstrap.min.css",
                      "~/content/DataTables-1.10.12/extensions/select/css/select.bootstrap.min.css",
                      "~/content/site.css")
                     );

            bundles.Add(new StyleBundle("~/content/Payments").Include(
                      "~/content/bootstrap.min.css",
                      "~/content/font-awesome.min.css",
                      "~/content/bootstrap-datepicker3.min.css",
                      "~/content/DataTables-1.10.12/media/css/dataTables.bootstrap.min.css",
                      "~/content/DataTables-1.10.12/extensions/buttons/css/buttons.bootstrap.min.css",
                      "~/content/DataTables-1.10.12/extensions/select/css/select.bootstrap.min.css",
                      "~/content/site.css"));

            bundles.Add(new StyleBundle("~/content/Aprobacion").Include(
                      "~/content/bootstrap.min.css",
                      "~/content/font-awesome.min.css",
                      "~/content/bootstrap-datepicker3.min.css",
                      "~/content/DataTables-1.10.12/media/css/dataTables.bootstrap.min.css",
                      "~/content/DataTables-1.10.12/extensions/buttons/css/buttons.bootstrap.min.css",
                      "~/content/DataTables-1.10.12/extensions/select/css/select.bootstrap.min.css",
                      "~/content/site.css"));

            bundles.Add(new StyleBundle("~/content/EditPayments").Include(
                      "~/content/bootstrap.min.css",
                      "~/content/font-awesome.min.css",
                      "~/content/bootstrap-datepicker3.min.css",
                      "~/content/DataTables-1.10.12/media/css/dataTables.bootstrap.min.css",
                      "~/content/DataTables-1.10.12/extensions/buttons/css/buttons.bootstrap.min.css",
                      "~/content/DataTables-1.10.12/extensions/select/css/select.bootstrap.min.css",
                      "~/content/site.css"));

            bundles.Add(new StyleBundle("~/content/EditThirdparties").Include(
                      "~/content/bootstrap.min.css",
                      "~/content/font-awesome.min.css",
                      "~/content/bootstrap-datepicker3.min.css",
                      "~/content/DataTables-1.10.12/media/css/dataTables.bootstrap.min.css",
                      "~/content/DataTables-1.10.12/extensions/buttons/css/buttons.bootstrap.min.css",
                      "~/content/DataTables-1.10.12/extensions/select/css/select.bootstrap.min.css",
                      "~/content/site.css"));

            bundles.Add(new StyleBundle("~/content/adjustmentebt/css").Include(
                      "~/content/bootstrap.min.css",
                      "~/content/font-awesome.min.css",
                      "~/content/bootstrap-datepicker3.min.css",
                      "~/content/DataTables-1.10.12/media/css/dataTables.bootstrap.min.css",
                      "~/content/DataTables-1.10.12/extensions/buttons/css/buttons.bootstrap.min.css",
                      "~/content/DataTables-1.10.12/extensions/select/css/select.bootstrap.min.css",
                      "~/content/site.css"));

            bundles.Add(new StyleBundle("~/content/QueryStylesheets").Include(
                     "~/content/bootstrap.min.css",
                     "~/content/font-awesome.min.css",
                     "~/content/bootstrap-datepicker3.min.css",
                     "~/content/DataTables-1.10.12/media/css/dataTables.bootstrap.min.css",
                     "~/content/DataTables-1.10.12/extensions/buttons/css/buttons.bootstrap.min.css",
                     "~/content/DataTables-1.10.12/extensions/select/css/select.bootstrap.min.css",
                     "~/content/site.css",
                     "~/content/QueryStylesheet.css"));

            bundles.Add(new StyleBundle("~/content/DemographicStylesheets").Include(
                     "~/content/bootstrap.min.css",
                     "~/content/font-awesome.min.css",
                     "~/content/bootstrap-datepicker3.min.css",
                     "~/content/DataTables-1.10.12/media/css/dataTables.bootstrap.min.css",
                     "~/content/DataTables-1.10.12/extensions/buttons/css/buttons.bootstrap.min.css",
                     "~/content/DataTables-1.10.12/extensions/select/css/select.bootstrap.min.css",
                     "~/content/site.css"));

            bundles.Add(new StyleBundle("~/content/PaymentHistoryStylesheets").Include(
                    "~/content/bootstrap.min.css",
                    "~/content/font-awesome.min.css",
                    "~/content/bootstrap-datepicker3.min.css",
                    "~/content/DataTables-1.10.12/media/css/dataTables.bootstrap.min.css",
                    "~/content/DataTables-1.10.12/extensions/buttons/css/buttons.bootstrap.min.css",
                    "~/content/DataTables-1.10.12/extensions/select/css/select.bootstrap.min.css",
                    "~/content/site.css"));
        }

        private static void RegisterScripts(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/scripts/jquery-{version}.js",
                        "~/scripts/jquery.mask.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/globalize").Include(
                        "~/scripts/cldr.js",
                        "~/scripts/cldr/event.js",
                        "~/scripts/cldr/supplemental.js",
                        "~/scripts/globalize.js",
                        "~/scripts/globalize/message.js",
                        "~/scripts/globalize/number.js",
                        "~/scripts/globalize/plural.js",
                        "~/scripts/globalize/date.js",
                        "~/scripts/globalize/currency.js",
                        "~/scripts/globalize/relative-time.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/scripts/jquery.validate.min.js",
                        "~/scripts/localization/messages_es.js",
                        "~/scripts/jquery.validate.globalize.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/datatables").Include(
                        "~/scripts/DataTables-1.10.12/media/js/jquery.datatables.min.js",
                        "~/scripts/DataTables-1.10.12/media/js/dataTables.bootstrap.min.js",
                        "~/scripts/DataTables-1.10.12/extensions/buttons/js/dataTables.buttons.min.js",                        
                        "~/scripts/DataTables-1.10.12/extensions/select/js/dataTables.select.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/DataTableExtended").Include(
                "~/scripts/DataTables-1.10.12/media/js/jquery.datatables.min.js",
                "~/scripts/DataTables-1.10.12/media/js/dataTables.bootstrap.min.js",
                "~/scripts/DataTables-1.10.12/extensions/buttons/js/dataTables.buttons.min.js",
                "~/scripts/DataTables-1.10.12/extensions/buttons/js/buttons.bootstrap.min.js",
                "~/scripts/DataTables-1.10.12/extensions/select/js/dataTables.select.min.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/DataTableEditor").Include(
                "~/scripts/DataTables-1.10.12/extensions/editor/js/dataTables.editor.min.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/custom").Include(
                        "~/scripts/common.js"));

            bundles.Add(new ScriptBundle("~/bundles/numeral").Include(
                        "~/scripts/numeral/numeral.js"));

            bundles.Add(new ScriptBundle("~/bundles/toastr").Include(
                        "~/scripts/toastr.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/blockui").Include(
                        "~/scripts/jquery.blockUI.js"));

            bundles.Add(new ScriptBundle("~/bundles/datepicker").Include(
                        "~/scripts/respond.min.js",                 
                        "~/scripts/moment.min.js",
                        "~/scripts/moment-with-locales.min.js",
                        "~/scripts/bootstrap-datepicker.min.js",
                        "~/scripts/locales/bootstrap-datepicker.es.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/scripts/bootstrap.min.js",
                      "~/scripts/bootbox.min.js",
                      "~/scripts/jquery.bootgrid.min.js",
                      "~/scripts/jquery.bootgrid.fa.js",
                      "~/scripts/respond.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/fileinput").Include(
                     "~/scripts/fileinput.min.js",
                     "~/scripts/fileinput_locale_es.js"));

            bundles.Add(new ScriptBundle("~/bundles/noty").Include(
                    "~/scripts/Noty/packaged/promise.js",
                    "~/scripts/Noty/packaged/jquery.noty.packaged.min.js",
                    "~/scripts/Noty/themes/default.js",
                    "~/scripts/Noty/themes/bootstrap.js",
                    "~/scripts/Noty/layouts/top.js"));

            bundles.Add(new ScriptBundle("~/bundles/event").Include(
                "~/scripts/PartialViews/Events.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/employerGroup").Include(
                "~/scripts/PartialViews/EmployerGroup.js"));

            bundles.Add(new ScriptBundle("~/bundles/payrollcfse").Include(
                "~/scripts/PartialViews/payrollcfse.js"));

            bundles.Add(new ScriptBundle("~/bundles/payrollother").Include(
                "~/scripts/PartialViews/payrollother.js"));

            bundles.Add(new ScriptBundle("~/bundles/managefile").Include(
                "~/scripts/PartialViews/managefile.js"));

            bundles.Add(new ScriptBundle("~/bundles/employerInformation").Include(
                "~/scripts/PartialViews/EmployerInformation.js"));

            bundles.Add(new ScriptBundle("~/bundles/employerProfile").Include(
                "~/scripts/PartialViews/EmployerProfile.js",
                "~/scripts/PartialViews/Address.js"));

            bundles.Add(new ScriptBundle("~/bundles/alerts").Include(
               "~/scripts/PartialViews/alerts.js"));

            bundles.Add(new ScriptBundle("~/bundles/SearchEmployerWithoutPolicy").Include(
               "~/scripts/PartialViews/SearchEmployerWithoutPolicy.js"));

            bundles.Add(new ScriptBundle("~/bundles/SearchInvIntEmployer").Include(
               "~/scripts/PartialViews/SearchInvIntEmployer.js"));

            bundles.Add(new ScriptBundle("~/bundles/PaymentHistorySearch") 
                .Include("~/scripts/DataTables-1.10.12/media/js/jquery.datatables.min.js",
                        "~/scripts/DataTables-1.10.12/media/js/dataTables.bootstrap.min.js",
                        "~/scripts/DataTables-1.10.12/extensions/select/js/dataTables.select.min.js",
                       "~/scripts/app.js",
                       "~/scripts/modules/case.search.js", 
                       "~/scripts/PartialViews/CaseSearch.js"));

            bundles.Add(new ScriptBundle("~/bundles/SearchCase").Include(
                "~/scripts/PartialViews/SearchCase.js"));

            bundles.Add(new ScriptBundle("~/bundles/inputmask").Include(                
                  "~/scripts/jquery.inputmask/inputmask.js",
                "~/scripts/jquery.inputmask/jquery.inputmask.js", 
                "~/scripts/jquery.inputmask/inputmask.extensions.js",
                "~/scripts/jquery.inputmask/inputmask.numeric.extensions.js"));

            bundles.Add(new ScriptBundle("~/bundles/autonumeric").Include("~/scripts/autonumeric.js"));

            bundles.Add(new ScriptBundle("~/bundles/Preexisting")
                .Include("~/scripts/autonumeric.js",
                       //"~/scripts/DataTables-1.10.12/media/js/jquery.datatables.min.js",
                       // "~/scripts/DataTables-1.10.12/media/js/dataTables.bootstrap.min.js",
                       // "~/scripts/DataTables-1.10.12/extensions/select/js/dataTables.select.min.js",                       
                        //"~/scripts/bootstrap.js",
                        //  "~/scripts/locales/bootstrap-datepicker.es.min.js",                        
                        "~/scripts/app.js")
                       .Include("~/scripts/modules/PreexistingCase.js")
                       .Include("~/scripts/modules/case.search.js")
                       .Include("~/scripts/PartialViews/SearchCase.js")
                       .Include("~/scripts/PartialViews/EditTransactionDetail.js"));

            bundles.Add(new ScriptBundle("~/bundles/PaymentCertification").Include(                
                "~/scripts/DataTables-1.10.12/media/js/jquery.datatables.min.js",
                "~/scripts/DataTables-1.10.12/media/js/dataTables.bootstrap.min.js",
                "~/scripts/DataTables-1.10.12/extensions/buttons/js/dataTables.buttons.min.js",
                "~/scripts/DataTables-1.10.12/extensions/buttons/js/buttons.bootstrap.min.js",
                "~/scripts/DataTables-1.10.12/extensions/select/js/dataTables.select.min.js",
                "~/scripts/DataTables-1.10.12/extensions/editor/js/dataTables.editor.min.js",
                "~/scripts/app.js",
                "~/scripts/modules/case.search.js",
                "~/scripts/PartialViews/PaymentCertification.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/PaymentCertificationTab").Include(                
                "~/scripts/DataTables-1.10.12/media/js/jquery.datatables.min.js",
                "~/scripts/DataTables-1.10.12/extensions/buttons/js/dataTables.buttons.min.js",
                "~/scripts/DataTables-1.10.12/extensions/buttons/js/buttons.bootstrap.min.js",
                "~/scripts/DataTables-1.10.12/extensions/select/js/dataTables.select.min.js",
                "~/scripts/DataTables-1.10.12/extensions/editor/js/dataTables.editor.min.js",                
                "~/scripts/PartialViews/PaymentCertificationTab.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/PaymentSumary").Include(
                "~/scripts/PartialViews/PaymentsSumary.js"));

            bundles.Add(new ScriptBundle("~/bundles/tablereferencesDatatables/scripts")
                 .Include("~/scripts/DataTables-1.10.12/media/js/jquery.datatables.min.js",
                        "~/scripts/DataTables-1.10.12/media/js/dataTables.bootstrap.min.js",
                        "~/scripts/DataTables-1.10.12/extensions/buttons/js/dataTables.buttons.min.js",
                        "~/scripts/DataTables-1.10.12/extensions/buttons/js/buttons.bootstrap.min.js",
                        "~/scripts/DataTables-1.10.12/extensions/select/js/dataTables.select.min.js",
                        "~/scripts/DataTables-1.10.12/extensions/editor/js/dataTables.editor.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/tablereferences/scripts")                 
                .Include("~/scripts/app.js")
                .Include("~/scripts/modules/TableReferences.js")
                .Include("~/scripts/modules/AddressType.js")
                .Include("~/scripts/modules/ApplyTo.js")
                .Include("~/scripts/modules/CivilStatus.js")
                .Include("~/scripts/modules/Gender.js")
                .Include("~/scripts/modules/InternetType.js")
                .Include("~/scripts/modules/PaymentClass.js")
                .Include("~/scripts/modules/PaymentConcept.js")
                .Include("~/scripts/modules/RelationshipCategory.js")
                .Include("~/scripts/modules/RelationshipType.js")
                .Include("~/scripts/modules/TransactionType.js")
                .Include("~/scripts/modules/TransferType.js"));

            bundles.Add(new ScriptBundle("~/bundles/thirdparties/scripts")
                .Include("~/scripts/app.js",
                        "~/scripts/modules/case.search.js",
                        "~/scripts/modules/ThirdParties.js"));

            bundles.Add(new ScriptBundle("~/bundles/Payments")
               .Include("~/scripts/DataTables-1.10.12/media/js/jquery.datatables.min.js",
                       "~/scripts/DataTables-1.10.12/media/js/dataTables.bootstrap.min.js",
                       "~/scripts/DataTables-1.10.12/extensions/buttons/js/dataTables.buttons.min.js",
                       "~/scripts/DataTables-1.10.12/extensions/buttons/js/buttons.bootstrap.min.js",
                       "~/scripts/DataTables-1.10.12/extensions/select/js/dataTables.select.min.js",                       
                       "~/scripts/app.js",
                       "~/scripts/modules/case.search.js",
                       "~/scripts/modules/desglose.ipp.js",
                       "~/scripts/modules/payment.registration.js"));

            bundles.Add(new ScriptBundle("~/bundles/aprobacion/scripts")
              .Include("~/scripts/DataTables-1.10.12/media/js/jquery.datatables.min.js",
                      "~/scripts/DataTables-1.10.12/media/js/dataTables.bootstrap.min.js",
                      "~/scripts/DataTables-1.10.12/extensions/buttons/js/dataTables.buttons.min.js",
                      "~/scripts/DataTables-1.10.12/extensions/buttons/js/buttons.bootstrap.min.js",
                      "~/scripts/DataTables-1.10.12/extensions/select/js/dataTables.select.min.js",
                      "~/scripts/app.js",
                       "~/scripts/modules/desglose.ipp.js",
                      "~/scripts/modules/Aprobacion.js"));

            bundles.Add(new ScriptBundle("~/bundles/EditThirdparties")
                .Include("~/scripts/DataTables-1.10.12/media/js/jquery.datatables.min.js",
                        "~/scripts/DataTables-1.10.12/media/js/dataTables.bootstrap.min.js",
                        "~/scripts/DataTables-1.10.12/extensions/buttons/js/dataTables.buttons.min.js",
                        "~/scripts/DataTables-1.10.12/extensions/buttons/js/buttons.bootstrap.min.js",
                        "~/scripts/DataTables-1.10.12/extensions/select/js/dataTables.select.min.js",                        
                        "~/scripts/app.js",
                        "~/scripts/modules/ThirdParties.js"));

            bundles.Add(new ScriptBundle("~/bundles/EditPayments")
               .Include("~/scripts/DataTables-1.10.12/media/js/jquery.datatables.min.js",
                       "~/scripts/DataTables-1.10.12/media/js/dataTables.bootstrap.min.js",
                       "~/scripts/DataTables-1.10.12/extensions/buttons/js/dataTables.buttons.min.js",
                       "~/scripts/DataTables-1.10.12/extensions/buttons/js/buttons.bootstrap.min.js",
                       "~/scripts/DataTables-1.10.12/extensions/select/js/dataTables.select.min.js",                       
                       "~/scripts/app.js",
                       "~/scripts/modules/EditPayments.js"));

            bundles.Add(new ScriptBundle("~/bundles/Queries")
                       .Include("~/scripts/DataTables-1.10.12/media/js/jquery.datatables.min.js",
                        "~/scripts/DataTables-1.10.12/media/js/dataTables.bootstrap.min.js",
                        "~/scripts/DataTables-1.10.12/extensions/select/js/dataTables.select.min.js",
                       "~/scripts/app.js",
                       "~/scripts/modules/case.search.js", 
                       "~/scripts/PartialViews/CaseInformation.js"));

              bundles.Add(new ScriptBundle("~/bundles/PaymentsWithMoreThan500")
                .Include("~/scripts/DataTables-1.10.12/media/js/jquery.datatables.min.js",
                        "~/scripts/DataTables-1.10.12/media/js/dataTables.bootstrap.min.js",
                        "~/scripts/DataTables-1.10.12/extensions/select/js/dataTables.select.min.js",                       
                       "~/scripts/app.js",
                       "~/scripts/PartialViews/PaymentWithMoreThan500.js"));

              bundles.Add(new ScriptBundle("~/bundles/NonCompliantPayments")
                .Include("~/scripts/DataTables-1.10.12/media/js/jquery.datatables.min.js",
                        "~/scripts/DataTables-1.10.12/media/js/dataTables.bootstrap.min.js",
                        "~/scripts/DataTables-1.10.12/extensions/select/js/dataTables.select.min.js",                       
                       "~/scripts/app.js",
                       "~/scripts/PartialViews/NonCompliantPayment.js"));

              bundles.Add(new ScriptBundle("~/bundles/CasesInDormantOrExpunged")
               .Include("~/scripts/autonumeric.js",
                      "~/scripts/DataTables-1.10.12/media/js/jquery.datatables.min.js",
                       "~/scripts/DataTables-1.10.12/media/js/dataTables.bootstrap.min.js",
                       "~/scripts/DataTables-1.10.12/extensions/select/js/dataTables.select.min.js",                      
                      "~/scripts/app.js",
                      "~/scripts/PartialViews/CasesInDormantOrExpunged.js"));

            bundles.Add(new ScriptBundle("~/bundles/adjustmentebtindex/scripts")
               .Include("~/scripts/DataTables-1.10.12/media/js/jquery.datatables.min.js",
                       "~/scripts/DataTables-1.10.12/media/js/dataTables.bootstrap.min.js",
                       "~/scripts/DataTables-1.10.12/extensions/buttons/js/dataTables.buttons.min.js",
                       "~/scripts/DataTables-1.10.12/extensions/buttons/js/buttons.bootstrap.min.js",
                       "~/scripts/DataTables-1.10.12/extensions/select/js/dataTables.select.min.js",                     
                       "~/scripts/app.js",
                       "~/scripts/modules/adjustmentebt.index.js"));

            bundles.Add(new ScriptBundle("~/bundles/adjustmentebtapprove/scripts")
                   .Include("~/scripts/DataTables-1.10.12/media/js/jquery.datatables.min.js",
                            "~/scripts/DataTables-1.10.12/media/js/dataTables.bootstrap.min.js",
                            "~/scripts/DataTables-1.10.12/extensions/buttons/js/dataTables.buttons.min.js",
                            "~/scripts/DataTables-1.10.12/extensions/buttons/js/buttons.bootstrap.min.js",
                            "~/scripts/DataTables-1.10.12/extensions/select/js/dataTables.select.min.js",                         
                            "~/scripts/app.js",
                            "~/scripts/modules/adjustmentebt.approve.js"));

            bundles.Add(new ScriptBundle("~/bundles/adjustmentebtdocument/scripts")
               .Include("~/scripts/DataTables-1.10.12/media/js/jquery.datatables.min.js",
                       "~/scripts/DataTables-1.10.12/media/js/dataTables.bootstrap.min.js",
                       "~/scripts/DataTables-1.10.12/extensions/buttons/js/dataTables.buttons.min.js",
                       "~/scripts/DataTables-1.10.12/extensions/buttons/js/buttons.bootstrap.min.js",
                       "~/scripts/DataTables-1.10.12/extensions/select/js/dataTables.select.min.js",                    
                       "~/scripts/app.js",
                       "~/scripts/modules/adjustmentebt.document.js"));

            bundles.Add(new ScriptBundle("~/bundles/adjustments/scripts")
               .Include("~/scripts/DataTables-1.10.12/media/js/jquery.datatables.min.js",
                       "~/scripts/DataTables-1.10.12/media/js/dataTables.bootstrap.min.js",
                       "~/scripts/DataTables-1.10.12/extensions/buttons/js/dataTables.buttons.min.js",
                       "~/scripts/DataTables-1.10.12/extensions/buttons/js/buttons.bootstrap.min.js",
                       "~/scripts/DataTables-1.10.12/extensions/select/js/dataTables.select.min.js",                       
                       "~/scripts/app.js",
                       "~/scripts/modules/case.search.js",
                       "~/scripts/modules/adjustments.index.js"));

        bundles.Add(new ScriptBundle("~/bundles/Demographic").Include(
                "~/scripts/DataTables-1.10.12/media/js/jquery.datatables.min.js",
                "~/scripts/DataTables-1.10.12/media/js/dataTables.bootstrap.min.js",
                "~/scripts/DataTables-1.10.12/extensions/select/js/dataTables.select.min.js",
                "~/scripts/moment.min.js",
                "~/scripts/moment-with-locales.min.js",
                "~/scripts/app.js",
                "~/scripts/modules/case.search.js",
                "~/scripts/PartialViews/demographic.js"
            ));
        }

        #endregion

        public static void RegisterBundles(BundleCollection bundles)
        {
            RegisterStyles(bundles);
            RegisterScripts(bundles);
        }
    }
}
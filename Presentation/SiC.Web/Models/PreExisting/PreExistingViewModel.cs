
namespace Nagnoi.SiC.Web.Models.PreExisting

{
    #region References

    using Infrastructure.Web.ViewModels;
    using Nagnoi.SiC.Web.Models;
    

    #endregion

    public class PreExistingViewModel : BaseViewModel
    {
        public Infrastructure.Web.ViewModels.CaseViewModel CaseModel = new Infrastructure.Web.ViewModels.CaseViewModel();
        public decimal? TotalAdjudicationAmount;
        public Models.Case.SearchViewModel SearchModel = new Models.Case.SearchViewModel();
        public Models.PaymentRegistration.IndexViewModel PaymentRegistration = new Models.PaymentRegistration.IndexViewModel();

        
        //public CaseViewModel CaseModel = new CaseViewModel();
        public TransactionDetailViewModel transactionDetailModel = new TransactionDetailViewModel();
        //public decimal TotalAdjudicationAmount;

        public PreExistingViewModel()
        { }
    }
}
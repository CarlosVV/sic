namespace Nagnoi.SiC.Infrastructure.Web.ViewModels
{
    #region References

    using Domain.Core.Services;
    using Infrastructure.Core.Dependencies;

    #endregion

    public class BaseViewModel
    {
        public BaseViewModel()
        { }

        public IResourceService ResourceService
        {
            get { return IoC.Resolve<IResourceService>(); }
        }
    }
}
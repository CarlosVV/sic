namespace Nagnoi.SiC.Web.Models.Employer
{
    #region References

    using Domain.Core.Model;
    using Infrastructure.Core.Validations;
    using Infrastructure.Core.Helpers;

    #endregion

    public class EmployerViewModel
    {
        #region Constructor

        public EmployerViewModel()
        {
            Nombre = string.Empty;
            EIN = string.Empty;
            Estatus = string.Empty;
            NumeroPoliza = string.Empty;
        }

        #endregion

        #region Properties

        public string Nombre { get; set; }

        public string EIN { get; set; }

        public string Estatus { get; set; }

        public string NumeroPoliza { get; set; }

        #endregion

        #region Public Methods

        public static EmployerViewModel CreateFrom(Case @case)
        {
            return new EmployerViewModel()
            {
                Nombre = Validate.EnsureNotNull(@case.EmployerName),
                EIN = Validate.EnsureNotNull(@case.EmployerEIN).ToEIN(),
                Estatus = @case.EmployerStatus == null ? string.Empty : @case.EmployerStatus.EmployerStatus1,
                NumeroPoliza = Validate.EnsureNotNull(@case.PolicyNo)
            };
        }

        #endregion
    }
}
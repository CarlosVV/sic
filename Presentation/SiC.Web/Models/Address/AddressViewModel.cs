namespace Nagnoi.SiC.Web.Models.Address
{
    #region References

    using Domain.Core.Model;

    #endregion

    public class AddressViewModel
    {
        #region Constructor

        public AddressViewModel()
        {
            Linea1 = string.Empty;
            Linea2 = string.Empty;
            Ciudad = string.Empty;
            Estado = string.Empty;
            CodigoPostal = string.Empty;
        }

        #endregion

        #region Properties

        public string Linea1 { get; set; }

        public string Linea2 { get; set; }

        public string Ciudad { get; set; }

        public string Estado { get; set; }

        public string CodigoPostal { get; set; }

        #endregion

        #region Public Methods

        public static AddressViewModel CreateFrom(Address address)
        {
            return new AddressViewModel()
            {
                Linea1 = address.Line1,
                Linea2 = address.Line2,
                Ciudad = address.City == null ? string.Empty : address.City.City1,
                Estado = address.State == null ? string.Empty : address.State.State1,
                CodigoPostal = string.IsNullOrEmpty(address.ZipCodeExt) ? address.ZipCode : string.Format("{0}-{1}", address.ZipCode, address.ZipCodeExt)
            };
        }

        #endregion
    }
}
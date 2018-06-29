using System.ComponentModel;

namespace Nagnoi.SiC.Domain.Core.Model {
    public enum PaymentTransferTypeEnum {
        [Description("Cheque")]
        Cheque = 1,
        [Description("EBT")]
        EBT = 2
    }
}

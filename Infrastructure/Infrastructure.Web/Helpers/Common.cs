namespace Nagnoi.SiC.Infrastructure.Web.Helpers {
    
    #region Imports

    using Nagnoi.SiC.Infrastructure.Core.Helpers;
    using System;
    using System.Diagnostics;
    using System.Web;
    using System.Web.Security;

    #endregion

    public class Common {
        public static short GetBusinessDays(DateTime startD, DateTime endD, int effectiveDays) {
            short calcBusinessDays = (short)(
                1 + ((endD - startD).TotalDays * 5 -
                (startD.DayOfWeek - endD.DayOfWeek) * 2) / 7);

            if (effectiveDays != 6)
                if (endD.DayOfWeek == DayOfWeek.Saturday) calcBusinessDays--;

            if (startD.DayOfWeek == DayOfWeek.Sunday) calcBusinessDays--;

            return calcBusinessDays;
        }
    }
}

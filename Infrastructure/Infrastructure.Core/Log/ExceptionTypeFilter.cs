namespace Nagnoi.SiC.Infrastructure.Core.Log
{
    #region Imports

    using log4net.Core;
    using log4net.Filter;

    #endregion

    /// <summary>
    /// Represents a custom log4net filter to match by exception type
    /// </summary>
    public class ExceptionTypeFilter : StringMatchFilter
    {
        /// <summary>
        /// Decides a logging event
        /// </summary>
        /// <param name="loggingEvent">LoggingEvent instance</param>
        /// <returns>Returns a filter decision</returns>
        public override FilterDecision Decide(LoggingEvent loggingEvent)
        {
            FilterDecision decision = FilterDecision.Deny;

            string exceptionString = loggingEvent.GetExceptionString();

            if (exceptionString.Contains("BusinessException") || exceptionString.Contains("CheckInException"))
            {
                decision = FilterDecision.Accept;
            }

            return decision;
        }
    }
}
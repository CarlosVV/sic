
namespace Nagnoi.SiC.Infrastructure.Core.Helpers {
    
    #region Imports

    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;


    #endregion
    /// <summary>
    /// JSON Helper class
    /// </summary>
    public static class JsonHelper
    {
        #region Public Methods

        /// <summary>
        /// Returns an error response
        /// </summary>
        /// <param name="error">Error description</param>
        /// <returns>Returns an anonymous object with format: Success = false, ErrorMessage = error message</returns>
        public static object ErrorResponse(string error)
        {
            return new { Success = false, ErrorMessage = error };
        }

        /// <summary>
        /// Returns a success response
        /// </summary>
        /// <returns>Returns an anonymous object with format: Success = true</returns>
        public static object SuccessResponse()
        {
            return new { Success = true };
        }

        /// <summary>
        /// Returns a success response
        /// </summary>
        /// <param name="referenceObject">Object reference which will be included within response</param>
        /// <returns>Returns an anonymous object with format: Success = true, Object = object reference</returns>
        public static object SuccessResponse(object referenceObject)
        {
            return new { Success = true, Object = referenceObject };
        }

        public static string Serialize(object referenceObject) {
            return JsonConvert.SerializeObject(referenceObject,
                Formatting.None,
                    new JsonSerializerSettings() {
                        ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                });
        }

        #endregion
    }
}
using System;
using System.Net;

namespace VOD.Common.Exceptions
{
    public class HttpResponseException : Exception
    {
        #region Properties

        public HttpStatusCode HttpStatusCode { get; }
        public object ValidationErrors { get; }

        #endregion

        #region Constructors

        public HttpResponseException(HttpStatusCode httpStatusCode, string message, object validationErrors) :
            base(message)
        {
            HttpStatusCode = httpStatusCode;
            ValidationErrors = validationErrors;
        }

        public HttpResponseException(HttpStatusCode httpStatusCode, string message) : this(httpStatusCode, message,
            null)
        {
        }

        public HttpResponseException(HttpStatusCode httpStatusCode) : this(httpStatusCode, string.Empty, null)
        {
        }

        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Util.ExceptionsHandler
{
    public class GenericException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }

        public GenericException(string message, HttpStatusCode statusCode) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}

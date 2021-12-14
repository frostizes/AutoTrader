using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Util.ExceptionsHandler
{
    public class BuisnessLogicException : GenericException
    {
        public BuisnessLogicException(string message, HttpStatusCode statusCode) : base(message, statusCode)
        {
        }
    }
}

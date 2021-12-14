using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Util.ExceptionsHandler
{
    class DuplicateAddingException : GenericException
    {
        public DuplicateAddingException(string message, HttpStatusCode statusCode) : base(message, statusCode)
        {
        }
    }
}

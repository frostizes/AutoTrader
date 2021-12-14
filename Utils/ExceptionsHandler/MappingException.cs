using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Util.ExceptionsHandler
{
    public class MappingException : GenericException
    {
        public MappingException(string message, HttpStatusCode statusCode) : base(message, statusCode)
        {
        }
    }
}

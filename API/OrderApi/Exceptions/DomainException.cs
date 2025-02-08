using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Exceptions
{
    public class DomainException : Exception
    {
        public DomainException(string message, Exception? innerException = null) : base(message, innerException){
            
        }
    }
}
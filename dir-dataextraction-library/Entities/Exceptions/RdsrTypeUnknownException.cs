using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
    public sealed class RdsrTypeUnknownException : RdsrException
    {
        public RdsrTypeUnknownException(int errorCode) 
            : base($"Rdsr type unknown. Error code {errorCode}")
        {
        }
    }
}

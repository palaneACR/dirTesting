using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
    public sealed class NotRdsrException : RdsrException
    {
        public NotRdsrException(int errorCode) 
            : base($"The file is not an RDSR. Please provide a valid file. Error code {errorCode}")
        {
        }
    }
}

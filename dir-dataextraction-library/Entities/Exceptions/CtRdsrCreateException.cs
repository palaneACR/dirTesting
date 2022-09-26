using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
    public sealed class CtRdsrCreateException : RdsrException
    {
        public CtRdsrCreateException(int errorCode) 
            : base($"Unable to create CtRdsr. Error code {errorCode}")
        {
        }
    }
}

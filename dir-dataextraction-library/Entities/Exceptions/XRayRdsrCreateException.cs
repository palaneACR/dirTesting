using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
    public sealed class XRayRdsrCreateException : RdsrException
    {
        public XRayRdsrCreateException(int errorCode)
            : base($"Unable to create PXRayRdsr. Error code {errorCode}")
        {
        }
    }
}

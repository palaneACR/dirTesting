using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
    public sealed class HeaderExtractException : RdsrException
    {
        public HeaderExtractException(int errorCode) 
            : base($"Could not extract headers. Error code {errorCode}")
        {
        }
    }
}

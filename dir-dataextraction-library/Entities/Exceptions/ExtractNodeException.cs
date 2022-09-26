using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
    public sealed class ExtractNodeException : RdsrException
    {
        public ExtractNodeException(int errorCode) 
            : base($"Could not extract nodes. Error code {errorCode}")
        {
        }
    }
}

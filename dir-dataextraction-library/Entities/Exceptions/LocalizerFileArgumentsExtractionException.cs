using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
    public sealed class LocalizerFileArgumentsExtractionException : RdsrException
    {
        public LocalizerFileArgumentsExtractionException(int errorCode)
            : base($"Error in file extraction. Error code {errorCode}")
        {
        }
    }
}

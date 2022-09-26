using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
    public sealed class ProcessException : RdsrException
    {
        public ProcessException(string message) 
            : base($"Unable to process dicom file. {message}")
        {
        }
    }
}

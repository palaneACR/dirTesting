using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
    public sealed class TagValueException : RdsrException
    {
        public TagValueException(int errorCode) 
            : base($"Failed to extract tag value. Error code {errorCode}")
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
    public sealed class LocalizerEngineAcessException : RdsrException
    {
        public LocalizerEngineAcessException(int errorCode)
            : base($"Error in accessing Localizer Engine. Error code {errorCode}")
        {
        }
    }
}

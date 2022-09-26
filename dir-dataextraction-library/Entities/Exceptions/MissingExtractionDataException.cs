using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
    public sealed class MissingExtractionDataException : RdsrException
    {
        public MissingExtractionDataException(string MissingColumns)
            : base($"Missing Extraction data columns {MissingColumns}")
        {
        }
    }
}

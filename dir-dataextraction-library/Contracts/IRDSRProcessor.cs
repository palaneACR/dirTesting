using Dicom;
using Shared.DataTransferObject.RDSR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IRDSRProcessor
    {
        Task<RdsrObjectDto> GetRdsrObjectAsync();
    }
}

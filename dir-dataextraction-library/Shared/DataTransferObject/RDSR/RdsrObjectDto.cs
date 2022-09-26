using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObject.RDSR
{
    public class RdsrObjectDto {
        public CtRdsr? CtRdsr { get; set; }
        public PXrayRdsr? PXrayRdsr { get; set; }
        public string RdsrType { get; set; }
    }
}

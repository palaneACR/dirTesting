using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObject.Localizer
{
    public class LocalizerDTO
    {
        public decimal Thickness { get; set; }
        public string? Orientation { get; set; }
        public string? Unit { get; set; }
        public string? StdOrErrOut { get; set; }
        public string? TenPercentThickness { get; set; }
        public string? MedianThickness { get; set; }
        public string? NinetyPercentThickness { get; set; }
    }
}

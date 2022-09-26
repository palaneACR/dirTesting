using Dir.DataExtraction.RDSR;
using Entities.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dir.DataExtraction.Tests
{
    public class LocaliserUnitTesting
    {

        [Fact]
        public void IsLocalizer_Dicom_Path_Invalid()
        {
            var path = @"\\acr.org\Shares\MIS\DIR\Dicom files\RDSR-Localizer\CT_RDSR_Test_WithLoc\412034443-RDSR-7798baf7-e.1183275518724ace9f16454da1f90282.f25ae759ab.txt";
            var ex = Record.Exception(() => new RDSRProcessor(path));

            Assert.NotNull(ex);
            Assert.IsType<Exception>(ex);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class CtDose
    {
        public string MeanCTDIvol { get; set; }
        public string CTDIwPhantomType { get; set; }
        public string DLP { get; set; }
        //DoseCheckAlertDetails
        public string DLPAlertValueConfigured { get; set; }
        public string CTDIvolAlertValueConfigured { get; set; }
        public string CTDIvolAlertValue { get; set; }
        //DoseCheckNotificationDetails
        public string DLPNotificationValueConfigured { get; set; }
        public string CTDIvolNotificationValueConfigured { get; set; }  
    }
}

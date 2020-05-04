using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportCatalog.Models
{
    public enum ReportEdditingMode { NewReport, ModifyReport };

    public class DesignerTask
    {
        public ReportEdditingMode mode { get; set; }
        public Guid reportID { get; set; }
    }
}

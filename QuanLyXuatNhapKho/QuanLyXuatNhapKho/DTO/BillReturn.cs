using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyXuatNhapKho.DTO
{
    public class BillReturn
    {
        public string BillReID { get; set; }
        public string BillID { get; set; }

        public DateTime? ReDate { get; set; }

        public string BillEmpID { get; set; }

        public string Reason { get; set; }

        public string CustomerID { get; set; }

        public string EmpID { get; set; }
        public DateTime? CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }

        public bool IsDeleted { get; set; }

    }

}

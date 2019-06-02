using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyXuatNhapKho.DTO
{
    public class BillsDetail
    {
        public string BillID { get; set; }

        public DateTime? Date { get; set; }

        public double TotalMoney { get; set; }

        public string Password { get; set; }

        public string CustomerID { get; set; }

        public string EmpID { get; set; }

        public string SupplierID { get; set; }

        public double TiLeGiam { get; set; }

        public int TrangThai { get; set; }
        public DateTime? CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }

        public bool IsDeleted { get; set; }

    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyXuatNhapKho.DTO
{
    public class Customer
    {
        public string CustomerID { get; set; }

        public string IdentityCard { get; set; }

        public DateTime? BirthDay { get; set; }

        public string FullName { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public string Sex { get; set; }

        public string Email { get; set; }

        public int CustomerTypeID { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }

        public bool IsDeleted { get; set; }

    }

}

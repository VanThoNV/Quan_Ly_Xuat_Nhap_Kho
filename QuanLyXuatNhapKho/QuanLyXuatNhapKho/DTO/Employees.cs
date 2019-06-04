using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyXuatNhapKho.DTO
{
    public class Employees
    {
        public string EmpID { get; set; }

        public string IdentityCard { get; set; }

        public DateTime? BirthDay { get; set; }

        public string FullName { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public Boolean Sex { get; set; }

        public string Email { get; set; }

        public string Note { get; set; }

        public DateTime WorkDay { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }

        public bool IsDeleted { get; set; }

        public string Image { get; set; }

    }

}

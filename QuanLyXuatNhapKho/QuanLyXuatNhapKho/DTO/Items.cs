using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyXuatNhapKho.DTO
{
    public class Items
    {
        public string ItemID { get; set; }
        public string ItemName {get; set;}
        public int ItemTypeID { get; set; }

        public int UnitID { get; set; }

        public DateTime? ManufactureDate { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public int QtyExists { get; set; }
        public DateTime? CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }

        public bool IsDeleted { get; set; }

    }

}

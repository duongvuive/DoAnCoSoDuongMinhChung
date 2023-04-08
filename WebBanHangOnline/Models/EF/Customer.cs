using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebBanHangOnline.Models.EF
{
    [Table("Customer")]
    public class Customer: CommonAbstract
    {
        public string CustomerID { get; set; }

        [Required]
        [StringLength(128)]
        public string FullName { get; set; }

        [Required]
        [StringLength(16)]
        public string Phone { get; set; }

        [Required]
        [StringLength(128)]
        public string Id { get; set; }

        [Required]
        [StringLength(40)]
        public string Address { get; set; }

        public int City { get; set; }

        public int District { get; set; }

        public int Ward { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime LastUpdated { get; set; }


        public virtual City City1 { get; set; }

        public virtual District District1 { get; set; }

        public virtual Ward Ward1 { get; set; }
    }
}
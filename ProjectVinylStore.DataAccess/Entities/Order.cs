using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectVinylStore.DataAccess.Entities
{
    public class Order
    {
        public int Id { get; set; }

        public string ProductName { get; set; }
        public int UserId { get; set; }

        public User Users { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
    }
}

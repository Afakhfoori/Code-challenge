using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Company
    {
        public Company(string name)
        {
            Name = name;
            Suppliers = new List<Supplier>();
            Products = new List<Product>();
            Barcodes = new List<Barcodes>();
        }
       public string Name { get; set; }
       public List<Supplier> Suppliers { get; set; }
       public List<Product> Products { get; set; }
       public List<Barcodes> Barcodes { get; set; }

       
    }
}

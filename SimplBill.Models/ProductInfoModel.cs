using System;
using System.Collections.Generic;
using System.Text;

namespace SimplBill.Models
{
    public class ProductInfoModel
    {
        public uint Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        //public List<string> ProductTags { get; set; }

        
    }
}

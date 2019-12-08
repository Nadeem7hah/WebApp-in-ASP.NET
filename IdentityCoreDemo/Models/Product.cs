using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityCoreDemo.Models
{
    public class Product
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public float BasePrice
        {
            get; set;
        }
        public float GetPriceWithTax(float taxPercent)
        {
            return BasePrice + (BasePrice * (taxPercent / 100));
        }
    }
}

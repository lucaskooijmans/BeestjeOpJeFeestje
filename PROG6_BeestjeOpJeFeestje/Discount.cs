using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeestjeOpJeFeestje.Domain
{
	[ExcludeFromCodeCoverage]

	public class Discount
    {
        public Discount(String name, int discount)
        {
            PercentageDiscount = discount;
            DiscountName = name;
        }
        public int PercentageDiscount { get; set; }
        public string DiscountName { get; set; }
    }
}

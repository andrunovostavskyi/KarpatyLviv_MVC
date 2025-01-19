using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karpaty.Models.ViewModels
{
    public class ShoppingCardVM
    {
        public List<ShoppingCard> ShoppingCardList { get; set; }
        public OrderHeader OrderHeader { get; set; }
    }
}

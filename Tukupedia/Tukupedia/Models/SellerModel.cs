using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tukupedia.Models
{
    public class SellerModel : Model
    {
        public SellerModel()
        {
            TableName = "SELLER";
            init();
        }
    }
}

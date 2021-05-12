using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Tukupedia.Models
{
    public class SellerModel : Model {
        public SellerModel()
        {
            TableName = "SELLER";
            init();
        }

    }
}

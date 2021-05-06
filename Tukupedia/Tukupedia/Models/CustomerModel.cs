using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tukupedia.Models
{
    public class CustomerModel : Model
    {
        public CustomerModel()
        {
            TableName = "Customer";
            init();
        }
    }
}

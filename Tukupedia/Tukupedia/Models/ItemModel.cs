using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tukupedia.Models
{
    public class ItemModel : Model
    {
        public ItemModel()
        {
            TableName = "ITEM";
            init();
        }


    }
}

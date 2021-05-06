using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tukupedia.Models
{
    public class CategoryModel : Model
    {
        public CategoryModel()
        {
            TableName = "Category";
            init();
        }
    }
}

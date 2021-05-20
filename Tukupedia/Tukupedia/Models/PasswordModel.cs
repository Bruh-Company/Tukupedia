using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tukupedia.Models
{
    class PasswordModel : Model
    {
        public PasswordModel(string tabel, string id)
        {
            this.initAdapter($"SELECT ID, PASSWORD from {tabel} where id = {id}");
        }
    }
}

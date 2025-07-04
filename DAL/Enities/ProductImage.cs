using DAL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Enities
{
    public class ProductImage : CommonFields
    {
        public string FilePath { get; set; }

        public string Name { get; set; }

        public Guid ProductId { get; set; }

        



    }
}

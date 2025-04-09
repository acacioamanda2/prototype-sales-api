using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.SalesRequest
{
    public class UpdateSaleRequest
    {
        public string Customer { get; set; }
        public string Branch { get; set; }
        public List<ItemRequest> Items { get; set; }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Ticaret.Entity
{
    public class Comments
    {
        public int Id { get; set; }

        public string Head { get; set; }
        public string UserName { get; set; }
        public string Description { get; set; }
        public bool isApproved { get; set; }


        public int ProductId { get; set; }
        public Product Product { get; set; }

    }
}
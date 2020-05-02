using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace E_Ticaret.Entity
{
    public class Product
    {
        public int Id { get; set; }

        [DisplayName("Ürün Adı")]
        public string Name { get; set; }

        [DisplayName("Açıklama")]
        public string Description { get; set; }

        [DisplayName("Fiyat")]
        public double Price { get; set; }

        [DisplayName("Stok")]
        public int Stock { get; set; }

        [DisplayName("Onay")]
        public bool IsApproved { get; set; }

        [DisplayName("Ürün Kodu")]
        public string ProductCode { get; set; }

        [DisplayName("Anasayfa")]
        public bool IsHome { get; set; }

        [DisplayName("Fotoğraf")]
        public string Image { get; set; }

        [DisplayName("Ürün Açıklaması")]
        public string BDescription { get; set; }

        [DisplayName("Kategori Adı")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        [DisplayName("Yorumlar")]
        public int CommentsId { get; set; }
        public List<Comments> Comments { get; set; }
    }
}
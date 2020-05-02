using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace E_Ticaret.Entity
{
    public class DataInitializer : DropCreateDatabaseIfModelChanges<DataContext>
    {
        protected override void Seed(DataContext context)
        {

            var kategoriler = new List<Category>()
            {
                new Category () {Name="Kamera",Description="Kameralar"},
                new Category () {Name="Bilgisayar",Description="Bilgisayarlar" },
                new Category () {Name="Beyaz Eşya",Description=" Beyaz Eşya" },
                new Category () {Name="Bilgisayar Ürünleri",Description="Bilgisayar Ürünleri " },
                new Category () {Name="Elektronik",Description="Elektronik " },
                new Category () {Name="Telefon",Description="Elektronik " },
                new Category () {Name="Eğlence",Description="Eğlence Ürünleri " }
            };

            foreach (var ctg in kategoriler)
            {
                context.Categories.Add(ctg);
            }
            context.SaveChanges();

            var urunler = new List<Product>()
            {
                new Product() {Name= "Apple",ProductCode="#23131",Image="5.jpg",IsHome=true,Description="iPhone 8 Plus 128GB Gold MX262TU/A Cep Telefonu - Apple Türkiye Garantili RC810APP647" , Price=5799, Stock=10, IsApproved=true , CategoryId=6},
                new Product() {Name= "Gamepower",ProductCode="#231321",Image="4.jpg",IsHome=true, Description="Gamepower Bane Siyah Oyuncu Mouse 0CVB.GMP.01.001766" , Price=84 , Stock=10 , IsApproved=true , CategoryId=4},
                new Product() {Name= "Televizyon",Image="3.jpg",ProductCode="#2311231",IsHome=true ,Description="OLED55C9 55' 140 Ekran Uydu Alıcılı 4K Ultra HD Smart OLED TV OLED55C9PLA" , Price=10088 , Stock=10 , IsApproved=true , CategoryId=5},
                new Product() {Name= "Vestel", IsHome=true,Image="2.jpg" ,ProductCode="#23121331",Description="CMI 9710 A+++ 1000 Devir 9 kg Çamaşır Makinesi 20260355" , Price=2048, Stock=10 , IsApproved=true , CategoryId=3},
                new Product() {Name= "Hp",Image="1.jpg",IsHome=true ,ProductCode="#231391", Description="15-DA1116NT Intel Core i5-8265U 8GB 1TB 128GB SSD 15.6 Windowns 10 Home 9QH75EA NB09QH75EA" , Price= 3799, Stock=32 , IsApproved=true , CategoryId=2},
                new Product() {Name= "Apple", IsHome=true,Image="max.jpg" ,ProductCode="#231230131",Description="MacBook Air Intel Core i5 5350U 8GB 128GB SSD MacOS Sierra 13.3' Taşınabilir Bilgisayar MQD32TU/A" , Price=5699 , Stock=20, IsApproved=true , CategoryId=2},
                new Product() {Name= "Samsung", IsHome=true,Image="hdd.jpg",ProductCode="#2311131",Description="Samsung M3 500GB 2.5' USB 3.0 Taşınabilir Disk (STSHX-M500TCB) OHD.SAM.729507528212" , Price=177, Stock=10 , IsApproved=true , CategoryId=4},
                new Product() {Name= "Xiaomi",Image="xior8.jpg",IsHome=true,ProductCode="#23138651", Description="Redmi Note 8 64GB Mavi (Xiaomi Türkiye Garantili) Cep Telefonu NOTE864" , Price= 1644, Stock=10 , IsApproved=true , CategoryId=6},
                new Product() {Name= "Huawei", IsHome=true,Image="huagt.jpg" ,ProductCode="#21753131",Description="Watch GT2 46mm Sport Akıllı Saat - Siyah HW-WTCHGT2" , Price=300 , Stock=10, IsApproved=true , CategoryId=5},
                new Product() {Name= "Xiaomi",Image="xioear.jpg",IsHome=true , ProductCode="#231323981",Description="Redmi Airdots Tws Bluetooth Basic 5.0 Kulaklık Mİ AİRDOTS TWS" , Price=500 , Stock=10 , IsApproved=true , CategoryId=5}

            };

            foreach (var prd in urunler)
            {
                context.Products.Add(prd);
            }
            context.SaveChanges();



            base.Seed(context);
        }
    }
}
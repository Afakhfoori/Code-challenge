using Domain;
using NUnit.Framework;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TestMegaMerg
{
    public class CompanyServiceTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestMerge()
        {

            var companyA = PopulateCompanyA();
            var companyB = PopulateCompanyB();
            var result = new List<ProductWithSource>();
            try
            {
                result = CompanyService.MergeCatalogs(companyA, companyB);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            Assert.AreEqual(7, result.Count);

            var companyAProducts = result.Where(p => p.Source == companyA.Name);
            Assert.AreEqual(5, companyAProducts.Count());

            var companyBProducts = result.Where(p => p.Source == companyB.Name);
            Assert.AreEqual(2, companyBProducts.Count());

            var product = result.Where(p => p.SKU == "647-vyk-317").ToList();
            Assert.AreNotEqual(null, product);
            Assert.AreEqual(1, product.Count());
            Assert.AreEqual(companyA.Name, product[0].Source);

            product = result.Where(p => p.SKU == "999-eol-949").ToList();
            Assert.AreNotEqual(null, product);
            Assert.AreEqual(1, product.Count());
            Assert.AreEqual(companyB.Name, product[0].Source);

            Assert.Pass();

        }

        private List<Barcodes> GenerateBarcodeList(string supplierId, string productId, string[] barcods)
        {
            var barcodeList = new List<Barcodes>();
            foreach(var barcode in barcods)
            {
                barcodeList.Add(new Barcodes { SupplierID = supplierId, SKU = productId, Barcode = barcode });
            }
            return barcodeList;
        }

        private Company PopulateCompanyA()
        {
            var companyA = new Company("A");
            var suppliers = new List<Supplier>() {
                new Supplier { ID = "00001", Name = "Twitterbridge" },
                new Supplier { ID = "00002", Name = "Thoughtsphere" },
                new Supplier { ID = "00003", Name = "Photobug" },
                new Supplier { ID = "00004", Name = "Jatri" },
                new Supplier { ID = "00005", Name = "Trunyx" }
            };
            companyA.Suppliers.AddRange(suppliers);

            var products = new List<Product>() {
                new Product { SKU = "647-vyk-317", Description= "Walkers Special Old Whiskey" },
                new Product { SKU = "280-oad-768", Description = "Bread - Raisin" },
                new Product { SKU = "165-rcy-650", Description = "Tea - Decaf 1 Cup" },
                new Product { SKU = "167-eol-949", Description = "Cheese - Grana Padano" },
                new Product { SKU = "650-epd-782", Description = "Carbonated Water - Lemon Lime" }
            };
            companyA.Products.AddRange(products);

            var barcodes = new List<Barcodes>();
            barcodes.AddRange(GenerateBarcodeList("00001", "647-vyk-317", new string[] { "z2783613083817", "z2783613083818", "z2783613083819", "n7405223693844", "c7417468772846",
                "w3744746803743", "w2572813758673", "s7013910076253", "m1161615509466"}));
            barcodes.AddRange(GenerateBarcodeList("00002", "280-oad-768", new string[] { "p2359014924610", "a7802303764525", "o5194275040472", "j9023946968130", "x5678105140949",
                "c9083052423045", "f4322915485228", "i0471865670980", "i0471865670981", "i0471865670982","b4381274928349"}));

            barcodes.AddRange(GenerateBarcodeList("00003", "165-rcy-650", new string[] { "u5160747892301", "m8967092785598", "l7342139757479", "p1667270888414", "v0874763455559",
            "p9774916416859", "c4858834209466", "x7331732444187", "u7720008047675", "i2431892662423", "	o1336108796249", "w7839803663600" }));

            barcodes.AddRange(GenerateBarcodeList("00004", "167-eol-949", new string[] { "a6971219877032", "a7340270328026", "a0126648261918", "a9858014383660", "a2338856941909",
                "a5056026479965", "a7425424390056", "a0864219864945", "a1257743939800", "a0880467790155", "a4469253605532", "a0891358702681" }));

            barcodes.AddRange(GenerateBarcodeList("00005", "650-epd-782", new string[] { "n8954999835177", "d2381485695273", "y0588794459804", "v8710606253394", "o5184937926186",
                "r4059282550570", "k3213966445562", "a3343396882074" }));

            companyA.Barcodes.AddRange(barcodes);

            return companyA;
        }

        private Company PopulateCompanyB()
        {
            var companyB = new Company("B");
            var suppliers = new List<Supplier>() {
                new Supplier { ID = "00001", Name = "Wikivu" },
                new Supplier { ID = "00002", Name = "Divavu" },
                new Supplier { ID = "00003", Name = "Flashdog" },
                new Supplier { ID = "00004", Name = "Bluejam" },
                new Supplier { ID = "00005", Name = "Twitterworks" }
            };
            companyB.Suppliers.AddRange(suppliers);

            var products = new List<Product>() {
                new Product { SKU = "999-vyk-317", Description= "Walkers Special Old Whiskey test" },
                new Product { SKU = "999-oad-768", Description = "Bread - Raisin" },
                new Product { SKU = "165-rcy-650", Description = "Tea - Decaf 1 Cup" },
                new Product { SKU = "999-eol-949", Description = "Cheese - Grana Padano" },
                new Product { SKU = "999-epd-782", Description = "	Carbonated Water - Lemon Lime" }
            };
            companyB.Products.AddRange(products);

            var barcodes = new List<Barcodes>();
            barcodes.AddRange(GenerateBarcodeList("00001", "999-vyk-317", new string[] { "z2783613083817", "n7405223693844", "c7417468772846", "w3744746803743", "w2572813758673",
                "s7013910076253", "m1161615509466" }));



            barcodes.AddRange(GenerateBarcodeList("00002", "999-oad-768", new string[] { "p2359014924610", "a7802303764525", "o5194275040472", "j9023946968130", "x5678105140949",
                "c9083052423045", "f4322915485228", "i0471865670980", "b4381274928349" }));



            barcodes.AddRange(GenerateBarcodeList("00003", "165-rcy-650", new string[] { "u5160747892301", "m8967092785598", "l7342139757479", "p1667270888414", "v0874763455559",
                "p9774916416859", "c4858834209466", "x7331732444187", "u7720008047675", "i2431892662423", "o1336108796249", "w7839803663600" }));


            barcodes.AddRange(GenerateBarcodeList("00004", "999-eol-949", new string[] { "x6971219877032", "x7340270328026", "x0126648261918", "x9858014383660", "x2338856941909",
                "x5056026479965", "x7425424390056", "x0864219864945", "x1257743939800", "x0880467790155", "x4469253605532", "x0891358702681" }));


            barcodes.AddRange(GenerateBarcodeList("00005", "999-epd-782", new string[] { "b8954999835177", "b2381485695273", "b0588794459804", "b8710606253394", "b5184937926186",
                "b4059282550570", "b3213966445562", "b3343396882074" }));


            companyB.Barcodes.AddRange(barcodes);

            return companyB;
        }
    }
}
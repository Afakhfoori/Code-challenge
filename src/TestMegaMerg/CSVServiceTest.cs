using CsvHelper;
using Domain;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestMegaMerg
{
    public class CSVServiceTest
    {
        [Test]
        public async Task Test_ReadCsvFile_Valid_File()
        {
            var content = CreateCsvContent();
            var inputFile = CreateFile(content);

            var productList = await CsvService.ReadCSVFile<Product>(inputFile);
            Assert.AreEqual(5, productList.Count());
        }

        [Test]
        public async Task Test_ReadCsvFile_Invalid_File()
        {
            var content = "Hello World from a Fake File";
            var inputFile = CreateFile(content);
            try
            {
                var productList = await CsvService.ReadCSVFile<Product>(inputFile);
                Assert.Fail();
            }
            catch(Exception ex)
            {
                Assert.AreEqual(ex.Message, $"{inputFile.Name} is not in correct format");
            }
        }

        [Test]
        public async Task Test_SaveCsvFile()
        {
            var products = CreateProductWithServiceList();
            var result = CsvService.SaveToCSV(products, "my-csv-file.csv");
            Assert.AreEqual("application/octet-stream", result.ContentType);
            Assert.AreEqual("my-csv-file.csv", result.FileDownloadName);
        }



        private IFormFile CreateFile(string content)
        {
            var file = new Mock<IFormFile>();
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;
            var fileName = "catalogA.csv";
            file.Setup(f => f.FileName).Returns(fileName).Verifiable();
            file.Setup(_ => _.Length).Returns(ms.Length);
            file.Setup(_ => _.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                .Returns((Stream stream, CancellationToken token) => ms.CopyToAsync(stream))
                .Verifiable();

            return file.Object;

        }

        private string CreateCsvContent()
        {
            var products = CreateProductList();
            using (var writer = new StringWriter())
            using (var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture, leaveOpen: true))
            {

                csvWriter.WriteHeader<Product>();
                csvWriter.NextRecord();
                csvWriter.WriteRecords(products);
                csvWriter.Flush();

                var content = writer.ToString();
                return content;
            }
        }

        private List<Product> CreateProductList()
        {
            return new List<Product>() {
                new Product { SKU = "647-vyk-317", Description= "Walkers Special Old Whiskey" },
                new Product { SKU = "280-oad-768", Description = "Bread - Raisin" },
                new Product { SKU = "165-rcy-650", Description = "Tea - Decaf 1 Cup" },
                new Product { SKU = "167-eol-949", Description = "Cheese - Grana Padano" },
                new Product { SKU = "650-epd-782", Description = "Carbonated Water - Lemon Lime" }
            };
        }

        private List<ProductWithSource> CreateProductWithServiceList()
        {
            return new List<ProductWithSource>() {
                new ProductWithSource { SKU = "647-vyk-317", Description= "Walkers Special Old Whiskey", Source ="A" },
                new ProductWithSource { SKU = "280-oad-768", Description = "Bread - Raisin", Source ="A"},
                new ProductWithSource { SKU = "165-rcy-650", Description = "Tea - Decaf 1 Cup", Source ="A" },
                new ProductWithSource { SKU = "167-eol-949", Description = "Cheese - Grana Padano", Source ="A" },
                new ProductWithSource { SKU = "650-epd-782", Description = "Carbonated Water - Lemon Lime", Source ="A" }
            };
        }
    }
}

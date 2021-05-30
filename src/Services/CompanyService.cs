using Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public static class CompanyService
    {
        public static List<ProductWithSource> MergeCatalogs(Company companyA, Company companyB)
        {
            var results = companyA.Barcodes.Join(companyB.Barcodes, arg => arg.Barcode, arg => arg.Barcode, (first, second) => new { SKUA = first.SKU, SKUB = second.SKU });
            var FinalProductList = new List<ProductWithSource>();
            foreach (var result in results)
            {
                var productA = companyA.Products.Where(p => p.SKU == result.SKUA).FirstOrDefault();
                var productB = companyB.Products.Where(p => p.SKU == result.SKUB).FirstOrDefault();
                if (productA != null)
                {
                    FinalProductList.Add(new ProductWithSource { SKU = productA.SKU, Description = productA.Description, Source = companyA.Name });
                    companyA.Products.Remove(productA);
                    if (productB != null)
                    {
                        companyB.Products.Remove(productB);
                    }
                }
            }

            FinalProductList.AddRange(companyA.Products.Select(p => new ProductWithSource { SKU = p.SKU, Description = p.Description, Source = companyA.Name }));
            FinalProductList.AddRange(companyB.Products.Select(p => new ProductWithSource { SKU = p.SKU, Description = p.Description, Source = companyB.Name }));

            return FinalProductList;
        }
    }
}

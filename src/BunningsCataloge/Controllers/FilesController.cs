using BunningsCataloge.Models;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BunningsCataloge.Controllers
{
    public class FilesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(ImportFileModel model)
        {
            ViewBag.Error = "";

            var companyA = new Company("A");
            var companyB = new Company("B");

            if (ModelState.IsValid)
            {
                if (model.BarcodeA != null)
                {
                    try
                    {
                        companyA.Suppliers = await CsvService.ReadCSVFile<Supplier>(model.SuppliersA);
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("SuppliersA", ex.Message);
                    }

                    try
                    {
                        companyB.Suppliers = await CsvService.ReadCSVFile<Supplier>(model.SuppliersB);
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("SuppliersB", ex.Message);
                    }

                    try
                    {
                        companyA.Products = await CsvService.ReadCSVFile<Product>(model.CatalogA);
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("CatalogA", ex.Message);
                    }

                    try
                    {
                        companyB.Products = await CsvService.ReadCSVFile<Product>(model.CatalogB);
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("CatalogB", ex.Message);
                    }

                    try
                    {
                        companyA.Barcodes = await CsvService.ReadCSVFile<Barcodes>(model.BarcodeA);
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("BarcodeA", ex.Message);
                    }

                    try
                    {
                        companyB.Barcodes = await CsvService.ReadCSVFile<Barcodes>(model.BarcodeB);
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("BarcodeB", ex.Message);
                    }

                    var FinalProductList = CompanyService.MergeCatalogs(companyA, companyB);
                    return CsvService.SaveToCSV(FinalProductList, "result_output.csv");

                }
            }

            return View(model);
        }


    }
}

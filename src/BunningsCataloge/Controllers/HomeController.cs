using BunningsCataloge.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Http;
using CsvHelper;
using System.Globalization;
using Domain;
using System.Text;
using Services;
using Microsoft.AspNetCore.Hosting;

namespace BunningsCataloge.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _env;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        public IActionResult Index()
        {
            var path = _env.ContentRootPath;
            var rootFolder = System.IO.Directory.GetParent(path);
            var folders = Directory.GetDirectories(rootFolder.FullName);
            var inputFolder = Directory.GetDirectories(rootFolder.FullName).Where(d => d.Contains("input")).FirstOrDefault();
            var outputFolder = Directory.GetDirectories(rootFolder.FullName).Where(d => d.Contains("output")).FirstOrDefault();

            var model = new FolderModel() { Input = inputFolder, Output = outputFolder };
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Index(FolderModel model)
        {
            var companyA = new Company("A");
            var companyB = new Company("B");

            ViewBag.SuccessMessage = "";

            if (ModelState.IsValid)
            {
                if (!Directory.Exists(model.Input))
                {
                    ModelState.AddModelError("Input", "Input Directory doesn't exist");
                    return View(model);
                }
                var inputFiles = Directory.GetFiles(model.Input);

                if (inputFiles.Length == 0)
                {
                    ModelState.AddModelError("Input", "Input Directory is empty");
                }
                var requiredInputFiles = new string[] { "barcodesA.csv", "barcodesB.csv", "catalogA.csv", "catalogB.csv", "suppliersA.csv", "suppliersB.csv" };
                var missingFile = new List<string>();
                foreach(var file in requiredInputFiles)
                {
                    if(!inputFiles.Any(f => f.Contains(file)))
                    {
                        missingFile.Add(file);
                    }
                }
                if(missingFile.Count > 0)
                {
                   ModelState.AddModelError("Input", $"{string.Join(", ",missingFile)} are missing in Input directory");
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        var supplierAPath = inputFiles.Where(f => f.Contains("suppliersA.csv")).FirstOrDefault();
                        companyA.Suppliers = CsvService.ReadCSVFileFromPath<Supplier>(supplierAPath);
                    } catch (Exception ex)
                    {
                        ModelState.AddModelError("Input", ex.Message );
                    }

                    try
                    {
                        var supplierBPath = inputFiles.Where(f => f.Contains("suppliersB.csv")).FirstOrDefault();
                        companyB.Suppliers = CsvService.ReadCSVFileFromPath<Supplier>(supplierBPath);
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("Input", ex.Message);
                    }

                    try
                    {
                        var productAPath = inputFiles.Where(f => f.Contains("catalogA.csv")).FirstOrDefault();
                        companyA.Products = CsvService.ReadCSVFileFromPath<Product>(productAPath);
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("Input", ex.Message);
                    }

                    try
                    {
                        var productBPath = inputFiles.Where(f => f.Contains("catalogB.csv")).FirstOrDefault();
                        companyB.Products = CsvService.ReadCSVFileFromPath<Product>(productBPath);
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("Input", ex.Message);
                    }

                    try
                    {
                        var barcodeAPath = inputFiles.Where(f => f.Contains("barcodesA.csv")).FirstOrDefault();
                        companyA.Barcodes = CsvService.ReadCSVFileFromPath<Barcodes>(barcodeAPath);
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("Input", ex.Message);
                    }

                    try
                    {
                        var barcodeBPath = inputFiles.Where(f => f.Contains("barcodesB.csv")).FirstOrDefault();
                        companyB.Barcodes = CsvService.ReadCSVFileFromPath<Barcodes>(barcodeBPath);
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("Input", ex.Message);
                    }

                    if (ModelState.IsValid)
                    {
                        try
                        {
                            var FinalProductList = CompanyService.MergeCatalogs(companyA, companyB);

                            if (!Directory.Exists(model.Output))
                            {
                                System.IO.Directory.CreateDirectory(model.Output);
                            }
                            else
                            {
                                System.IO.DirectoryInfo di = new DirectoryInfo(model.Output);
                                foreach (FileInfo file in di.GetFiles())
                                {
                                    file.Delete();
                                }
                            }

                            var result = CsvService.SaveToCSVPath(FinalProductList, $"{model.Output}/result_output.csv");
                            ViewBag.SuccessMessage = "Input files have been merged and output file has been created successfully";
                        } catch(Exception ex)
                        {
                            ModelState.AddModelError("Input", ex.Message);
                        }
                    }
                }

            }

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



    }
}

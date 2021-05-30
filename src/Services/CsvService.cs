using CsvHelper;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class CsvService
    {
        public static async Task<List<T>> ReadCSVFile<T>(IFormFile file) where T : class, new()
        {
            try
            {
                string filePath = string.Empty;
                var records = new List<T>();
                if (file.Length > 0)
                {
                    filePath = Path.GetTempFileName();
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await file.CopyToAsync(stream);
                    }

                    TextReader reader = new StreamReader(filePath);
                    var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
                    records = csvReader.GetRecords<T>().ToList();
                }
                return records;
            }
            catch (Exception ex)
            {
                throw new Exception($"{file.Name} is not in correct format");
            }
        }

        public static List<T> ReadCSVFileFromPath<T>(string filePath) where T : class, new()
        {
            try
            {
                var records = new List<T>();
                TextReader reader = new StreamReader(filePath);
                var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
                records = csvReader.GetRecords<T>().ToList();

                return records;
            }
            catch (Exception ex)
            {
                throw new Exception($"{filePath} is not in correct format");
            }
        }

        public static FileContentResult SaveToCSV<T>(List<T> list, string fileName) where T : class, new()
        {
            byte[] bytes = CreateCsvFileInBytes(list);
            var result = new FileContentResult(bytes, "application/octet-stream");
            result.FileDownloadName = fileName;
            return result;

        }

        public static string SaveToCSVPath<T>(List<T> list, string path) where T : class, new()
        {
                byte[] bytes = CreateCsvFileInBytes(list);
                File.WriteAllBytes(path, bytes);
                return path;
        }

        public static byte[] CreateCsvFileInBytes<T>(List<T> list) where T : class, new()
        {
            using (var writer = new StringWriter())
            using (var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture, leaveOpen: true))
            {

                csvWriter.WriteHeader<T>();
                csvWriter.NextRecord();
                csvWriter.WriteRecords(list);
                csvWriter.Flush();

                var content = writer.ToString();
                byte[] bytes = Encoding.ASCII.GetBytes(content);
                return bytes;
            }
        }
    }
}

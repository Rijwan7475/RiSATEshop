﻿using System.Collections.Generic;
using System.IO;
using CsvHelper;
using RiSAT.Eshop.Application.Common.Interfaces;
using RiSAT.Eshop.Application.Products.Queries.GetProductsFile;

namespace RiSAT.Eshop.Infrastructure.Files
{
    public class CsvFileBuilder : ICsvFileBuilder
    {
        public byte[] BuildProductsFile(IEnumerable<ProductRecordDto> records)
        {
            using var memoryStream = new MemoryStream();
            using (var streamWriter = new StreamWriter(memoryStream))
            {
                using var csvWriter = new CsvWriter(streamWriter);
                csvWriter.Configuration.RegisterClassMap<ProductFileRecordMap>();
                csvWriter.WriteRecords(records);
            }

            return memoryStream.ToArray();
        }
    }
}

﻿using CsvHelper.Configuration;
using RiSAT.Eshop.Application.Products.Queries.GetProductsFile;

namespace RiSAT.Eshop.Infrastructure.Files
{
    public sealed class ProductFileRecordMap : ClassMap<ProductRecordDto>
    {
        public ProductFileRecordMap()
        {
            AutoMap();
            Map(m => m.UnitPrice).Name("Unit Price").ConvertUsing(c => (c.UnitPrice ?? 0).ToString("C"));
        }
    }
}

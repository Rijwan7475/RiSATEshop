﻿using AutoMapper;
using RiSAT.Eshop.Application.Common.Mappings;
using RiSAT.Eshop.Domain.Entities;

namespace RiSAT.Eshop.Application.Employees.Queries.GetEmployeeDetail
{
    public class EmployeeTerritoryDto : IMapFrom<EmployeeTerritory>
    {
        public string TerritoryId { get; set; }

        public string Territory { get; set; }

        public string Region { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<EmployeeTerritory, EmployeeTerritoryDto>()
                .ForMember(d => d.TerritoryId, opts => opts.MapFrom(s => s.TerritoryId))
                .ForMember(d => d.Territory, opts => opts.MapFrom(s => s.Territory.TerritoryDescription))
                .ForMember(d => d.Region, opts => opts.MapFrom(s => s.Territory.Region.RegionDescription));
        }
    }
}

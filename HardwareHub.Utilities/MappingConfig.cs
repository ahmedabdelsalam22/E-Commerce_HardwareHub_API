﻿using AutoMapper;
using HardwareHub.Models.Dtos;
using HardwareHub.Models.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareHub.Utilities
{
    public class MappingConfig : Profile
    {
       public MappingConfig() 
        {
            CreateMap<Category,CategoryDto>().ReverseMap();
            CreateMap<CategoryCreateDto,Category>();

            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Category, CategoryUpdateDto>();
            CreateMap<ProductUpdateDto, Product>();

            CreateMap<ApplicationUser, ApplicationUserDto>();
        }
    }
}

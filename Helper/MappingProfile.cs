using AutoMapper;
using ShopOnline.Models;
using ShopOnline.Models.CreateModels;

namespace ShopOnline.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {
            CreateMap<CreateProductCategoryDto, ProductCategory>();
            CreateMap<CreateProductDto, Product>();
        }
    }
}

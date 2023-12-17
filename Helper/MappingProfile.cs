using AutoMapper;
using ShopOnline.Models;
using ShopOnline.Models.CreateModels;
using ShopOnline.Models.EditModels;
using ShopOnline.Models.ViewModels;

namespace ShopOnline.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {
            CreateMap<CreateProductCategoryDto, ProductCategory>();
            CreateMap<CreateProductDto, Product>();
            CreateMap<Product, CreateProductDto>();
            CreateMap<Product, EditProductDto>();
            CreateMap<EditProductDto, Product>();
            CreateMap<Product, ViewProductDetailModel>();
        }
    }
}

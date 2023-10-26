﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json.Serialization;
using System.Runtime.CompilerServices;
using ShopOnline.Helper.Validate;

namespace ShopOnline.Models.CreateModels
{
    public class CreateProductDto
    {
        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        [Required]
        [MaxLength(256 , ErrorMessage = "Alias must not exceed {0} characters")]
        public string Alias { get; set; }

        [Required]
        public int CategoryID { get; set; }

        [Required(ErrorMessage="Choose 1 file image")]
        [DataType(DataType.Upload)]
        [CheckFileExtensions(Extensions = "png,jpg,jpeg")]
        [Display(Name="Image")]
        
        public IFormFile ImageUpload { get; set; }

        [Column(TypeName = "xml")]
        public string? MoreImages { get; set; }

        [Required]
        public decimal Price { get; set; }
        public decimal OriginalPrice { set; get; }
        public decimal? PromotionPrice { set; get; }

        [Required]
        public string Description { set; get; }
        public string? Content { set; get; }

        public bool? HomeFlag { set; get; }
        public bool? HotFlag { set; get; }
        public int? ViewCount { set; get; }

        //[Required]
        //public string Tags { set; get; }

        [Required]
        public int Quantity { set; get; }

        public List<ProductCategory> ? ListCategories { get; set; }
        public List<Tag>? ListTags { get; set; }
        public List<string>? ListSelectedTags { get; set; }
    }
}

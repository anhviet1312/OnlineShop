using System.ComponentModel.DataAnnotations;

public class CreateProductCategoryDto
{
    [Required(ErrorMessage = "Name is required.")]
    [MaxLength(256, ErrorMessage = "Name must not exceed 256 characters.")]
    public string Name { set; get; }

    [Required(ErrorMessage = "Alias is required.")]
    [MaxLength(256, ErrorMessage = "Alias must not exceed 256 characters.")]
    public string Alias { set; get; }

    [MaxLength(500, ErrorMessage = "Description must not exceed 500 characters.")]
    public string Description { set; get; }

    public int? ParentID { set; get; }

    public int? DisplayOrder { set; get; }

    [MaxLength(256, ErrorMessage = "Image URL must not exceed 256 characters.")]
    public string? Image { set; get; }

    public bool? HomeFlag { set; get; }
}

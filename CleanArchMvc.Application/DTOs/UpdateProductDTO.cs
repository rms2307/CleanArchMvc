using CleanArchMvc.Application.Annotations;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleanArchMvc.Application.DTOs
{
    public class UpdateProductDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The Name is Required")]
        [MinLength(3)]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "The Description is Required")]
        [MinLength(5)]
        [MaxLength(200)]
        public string Description { get; set; }

        [Required(ErrorMessage = "The Price is Required")]
        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "The Stock is Required")]
        [Range(1, 9999)]
        public int Stock { get; set; }

        [FileSizeValidation(1024 * 1024, ErrorMessage = "Image size should be less than 1 MB.")]
        [FileExtensionsValidation(new string[] { ".jpg", ".jpeg", ".png" }, ErrorMessage = "Only .jpg, .jpeg and .png files are allowed.")]
        public IFormFile Image { get; set; }

        public string ImageUrl { get; set; }

        public int CategoryId { get; set; }
    }
}
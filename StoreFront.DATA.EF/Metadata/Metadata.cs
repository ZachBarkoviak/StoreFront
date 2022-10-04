using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace StoreFront.DATA.EF.Models//.Metadata
{
    //internal class Metadata
    //{
    //}

    public class CategoryMetadata
    {
        //public int CategoryId { get; set; }

        [Required(ErrorMessage = "*Category Name is required")]
        [StringLength(50,ErrorMessage = "*Must be 50 characters or less")]
        public string CategoryName { get; set; } = null!;

        [StringLength(500, ErrorMessage = "Must be 500 characters or less")]
        [DataType(DataType.MultilineText)]
        public string? CategoryDescription { get; set; }
    }

    public class ManufacturerMetadata
    {
        //public int ManufacturerId { get; set; }

        [Required(ErrorMessage = "*Name is required")]
        [StringLength(25, ErrorMessage = "*Must be 25 characters or less")]
        public string ManufacturerName { get; set; } = null!;

        public int ManufacturerPlanetId { get; set; }

        [StringLength(20,ErrorMessage = "*Must be 20 characters or less")]
        public string? ContactName { get; set; }
    }

    public class PlanetMetadata
    {
        //public int PlanetId { get; set; }
        
        [StringLength(30, ErrorMessage = "*Must be 30 characters or less")]
        public string? PlanetName { get; set; }
        
        [StringLength(30, ErrorMessage = "*Must be 30 characters or less")]
        public string? PlanetCapital { get; set; }
    }

    public class ProductMetadata
    {
        //public int ProductId { get; set; }
        //public int CategoryId { get; set; }
        [Required(ErrorMessage = "*Price is required")]
        [DisplayFormat(DataFormatString = "{0:c}", ApplyFormatInEditMode = false)]
        [Range(0, (double)decimal.MaxValue)]
        [Display(Name = "Price")]
        public decimal ProductPrice { get; set; }

        [StringLength(250, ErrorMessage = "*Must be 250 characters or less")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Description")]
        public string? ProductDescription { get; set; }

        //public int ManufacturerId { get; set; }

        [Display(Name = "In Stock?")]
        public bool StockStatus { get; set; }

        [Required(ErrorMessage = "*In Stock is required")]
        [Range(0, int.MaxValue)]
        [Display(Name = "In Stock")]
        public int StockQty { get; set; }

        [Range(0, int.MaxValue)]
        [Display(Name = "On Order")]
        public int? UnitsOrdered { get; set; }

        [Required(ErrorMessage ="*Name is required")]
        [StringLength(50, ErrorMessage = "*Must be 50 characters or less")]
        [Display(Name = "Name")]
        public string ProductName { get; set; } = null!;

        [StringLength(75)]
        [Display(Name = "Image")]
        public string ProductImage { get; set; } = null!;
    }
}

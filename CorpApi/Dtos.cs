using System;
using System.Collections.Generic;

namespace CorpApi
{
    public class Dtos
    {
        public SaleDto Sale { get; set; } = new SaleDto();
        public List<SoldItemDto> SoldItems { get; set; } = new List<SoldItemDto>(); 
        public List<ProductDto> Products { get; set; } = new List<ProductDto>();

        public class SaleDto
        {
            public int EmployeeId { get; set; } 
            public DateTime SaleDate { get; set; } 
        }

        public class SoldItemDto
        {
            public int ProductId { get; set; } 
            public int Quantity { get; set; }
        }

        public class ProductDto
        {
            public string Name { get; set; } = string.Empty; 
            public int CategoryId { get; set; }
            public int Quantity { get; set; } 
            public decimal Price { get; set; } 
            public string Supplier { get; set; } = string.Empty; 
        }

        public class UpdateProductDto
        {
            public string? Name { get; set; } 
            public int? CategoryId { get; set; } 
            public int? Quantity { get; set; } 
            public decimal? Price { get; set; } 
            public string? Supplier { get; set; } 
        }

        public class UpdateEmployeeDto
        {
            public string? Name { get; set; } 
            public string? Surname { get; set; }
            public string? Patronymic { get; set; }
            public string? Post { get; set; }
            public string? Phone { get; set; } 
        }

    
    }

}

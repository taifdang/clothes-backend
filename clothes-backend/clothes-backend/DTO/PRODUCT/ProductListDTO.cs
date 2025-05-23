﻿using clothes_backend.DTO.Product;

namespace clothes_backend.DTO.PRODUCT_DTO
{
    public class productListDTO
    {
        public int id { get; set; }
        public string title { get; set; }
        public List<ImageDTO> images { get; set; }
        public List<VariantDTO> variants { get; set; }    
        public List<OptionImageDTO> options_value { get; set;}       
        public List<OptionDTO> options { get; set; }

    }
}

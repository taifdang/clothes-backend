﻿using AutoMapper;
using clothes_backend.DTO.PRODUCT_DTO;
using clothes_backend.Models;

namespace clothes_backend.AutoMapper
{
    public class ProductProfile:Profile
    {
       public ProductProfile()
       {
            //variants
            CreateMap<ProductVariants, variants_DTO>();

            CreateMap<Products, product_DTO>()
                .ForMember(x => x.variants, y => y.MapFrom(src => src.product_variants))
                .ForMember(target => target.options_value, opt => opt.MapFrom(src =>
                    src.product_option_images
                        .GroupBy(x => x.options_values.options.title)
                        .Select(group_option => new OptionValueImageGroupDTO
                        {
                            title = group_option.Key,
                            option_id = group_option.Select(k => k.options_values.option_id).FirstOrDefault() ?? null,
                            options = group_option
                                .GroupBy(v => v.options_values.value)
                                .Select(group_option_value => new option_value_DTO
                                {
                                    //image =  valueGroup.Select(i => i.src).ToList()
                                    title = group_option_value.Key,
                                    image = group_option_value.Select(i => new image_DTO { id = i.id, src = i.src }).ToList()
                                })
                                .ToList()
                        })
                        .ToList()
                ));
           
        }
    }
}

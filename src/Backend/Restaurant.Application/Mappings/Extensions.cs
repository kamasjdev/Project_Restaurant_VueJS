﻿using Restaurant.Application.DTO;
using Restaurant.Domain.Entities;

namespace Restaurant.Application.Mappings
{
    internal static class Extensions
    {
        public static Addition AsEntity(this AdditionDto additionDto)
        {
            return new Addition(additionDto.Id, additionDto.AdditionName, additionDto.Price, additionDto.ProductKind);
        }

        public static AdditionDto AsDto(this Addition addition)
        {
            return new AdditionDto
            {
                Id = addition.Id,
                AdditionName = addition.AdditionName,
                Price = addition.Price,
                ProductKind = addition.AdditionKind.ToString()
            };
        }
    }
}

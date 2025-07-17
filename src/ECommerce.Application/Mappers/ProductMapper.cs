using ECommerce.Domain.Entities;
using ECommerce.Application.Models.DTOs;
using Riok.Mapperly.Abstractions;

namespace ECommerce.Application.Mappers;

[Mapper]
public partial class ProductMapper
{
    public partial ProductDto ToDto(Product product);
    public partial Product ToEntity(ProductDto dto);
} 
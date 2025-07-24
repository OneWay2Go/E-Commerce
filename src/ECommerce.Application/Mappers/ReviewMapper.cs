using ECommerce.Domain.Entities;
using ECommerce.Application.Models.DTOs;
using Riok.Mapperly.Abstractions;

namespace ECommerce.Application.Mappers;

[Mapper]
public partial class ReviewMapper
{
    public partial ReviewDto ToDto(Review review);
    public partial Review ToEntity(ReviewDto dto);
} 
using ECommerce.Domain.Entities;
using ECommerce.Application.Models.DTOs;
using Riok.Mapperly.Abstractions;

namespace ECommerce.Application.Mappers;

[Mapper]
public partial class PaymentMapper
{
    public partial PaymentDto ToDto(Payment payment);
    public partial Payment ToEntity(PaymentDto dto);
} 
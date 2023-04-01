using AutoMapper;
using insurance_policy_api.DTOs;
using insurance_policy_api_domain.Entities;
using insurance_policy_api_domain.Entities.Installment;
using System.Collections.ObjectModel;

namespace insurance_policy_api.Mapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<PolicyDTO, PolicyEntity>()
            .ForMember(dest => dest.EntityID, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Cpf, opt => opt.MapFrom(src => src.Cpf))
            .ForMember(dest => dest.Situacao, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.PremioTotal, opt => opt.MapFrom(src => src.PremiumTotal))
            .ForMember(dest => dest.Parcelas, opt => opt.MapFrom(src => src.Installments));

        CreateMap<InstallmentDTO, InstallmentEntity>()
            .ForMember(dest => dest.EntityID, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.IdApolice, opt => opt.Ignore())
            .ForMember(dest => dest.Premio, opt => opt.MapFrom(src => src.Premium))
            .ForMember(dest => dest.FormaPagamento, opt => opt.MapFrom(src => Enum.Parse<MetodoPagamento>(src.PaymentMethod.ToUpper())))
            .ForMember(dest => dest.DataPagamento, opt => opt.MapFrom(src => src.PaymentDate))
            .ForMember(dest => dest.Situacao, opt => opt.Ignore())
            .ForMember(dest => dest.Apolice, opt => opt.Ignore());

        CreateMap<PolicyEntity, PolicyDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.EntityID))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Descricao))
            .ForMember(dest => dest.Cpf, opt => opt.MapFrom(src => src.Cpf))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Situacao))
            .ForMember(dest => dest.PremiumTotal, opt => opt.MapFrom(src => src.PremioTotal.Value))
            .ForMember(dest => dest.Installments, opt => opt.MapFrom(src => src.Parcelas));

        CreateMap<InstallmentEntity, InstallmentDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.EntityID))
            .ForMember(dest => dest.Premium, opt => opt.MapFrom(src => src.Premio.Value))
            .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.FormaPagamento.ToString().ToUpper()))
            .ForMember(dest => dest.PaymentDate, opt => opt.MapFrom(src => src.DataPagamento.Value));

        CreateMap<IEnumerable<PolicyDTO>, Collection<PolicyEntity>>();
        CreateMap<IEnumerable<PolicyEntity>, Collection<PolicyDTO>>();
        CreateMap<IEnumerable<InstallmentDTO>, Collection<InstallmentEntity>>();
        CreateMap<IEnumerable<InstallmentEntity>, Collection<InstallmentDTO>>();
    }
}
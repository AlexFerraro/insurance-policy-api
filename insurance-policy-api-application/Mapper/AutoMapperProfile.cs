using AutoMapper;
using insurance_policy_api_application.DTOs;
using insurance_policy_api_domain.Entities;
using insurance_policy_api_domain.Entities.Installment;
using System.Collections.ObjectModel;

namespace insurance_policy_api_application.Mapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<PolicyDTO, PolicyEntity>()
            .ForMember(dest => dest.EntityID, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Cpf, opt => opt.MapFrom(src => src.Cpf))
            .ForMember(dest => dest.Situation, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.TotalPremium, opt => opt.MapFrom(src => src.PremiumTotal))
            .ForMember(dest => dest.Installments, opt => opt.MapFrom(src => src.Installments));

        CreateMap<InstallmentDTO, InstallmentEntity>()
            .ForMember(dest => dest.EntityID, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.PolicyFK, opt => opt.Ignore())
            .ForMember(dest => dest.Premium, opt => opt.MapFrom(src => src.Premium))
            .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => Enum.Parse<PaymentMethod>(src.PaymentMethod.ToUpper())))
            .ForMember(dest => dest.PaymentDate, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.PaymentDate)))
            .ForMember(dest => dest.Situation, opt => opt.Ignore())
            .ForMember(dest => dest.Policy, opt => opt.Ignore());

        CreateMap<PolicyEntity, PolicyDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.EntityID))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Cpf, opt => opt.MapFrom(src => src.Cpf))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Situation))
            .ForMember(dest => dest.PremiumTotal, opt => opt.MapFrom(src => src.TotalPremium))
            .ForMember(dest => dest.Installments, opt => opt.MapFrom(src => src.Installments));

        CreateMap<InstallmentEntity, InstallmentDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.EntityID))
            .ForMember(dest => dest.Premium, opt => opt.MapFrom(src => src.Premium))
            .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod.ToString().ToUpper()))
            .ForMember(dest => dest.PaymentDate, opt => opt.MapFrom(src => src.PaymentDate.ToString("yyyy-MM-dd")));

        CreateMap<PolicyEntity, PolicyDetailsDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.EntityID))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Cpf, opt => opt.MapFrom(src => long.Parse(src.Cpf)))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Situation))
            .ForMember(dest => dest.PremiumTotal, opt => opt.MapFrom(src => src.TotalPremium))
            .ForMember(dest => dest.DataCriacaoRegistro, opt => opt.MapFrom(src => src.RecordCreationDate.ToString("yyyy-MM-dd")))
            .ForMember(dest => dest.DataAlteracaoRegistro, opt => opt.MapFrom(src => src.RegistrationChangeDate.Value.ToString("yyyy-MM-dd")))
            .ForMember(dest => dest.UsuarioCriacaoRegistro, opt => opt.MapFrom(src => src.UserCreationRecord))
            .ForMember(dest => dest.UsuarioAlteracaoRegistro, opt => opt.MapFrom(src => src.UserRecordChange))
            .ForMember(dest => dest.Installments, opt => opt.MapFrom(src => src.Installments));

        CreateMap<InstallmentEntity, InstallmentDetailsDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.EntityID))
            .ForMember(dest => dest.Premium, opt => opt.MapFrom(src => src.Premium))
            .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod.ToString().ToUpper()))
            .ForMember(dest => dest.PaymentDate, opt => opt.MapFrom(src => src.PaymentDate.ToString("yyyy-MM-dd")))
            .ForMember(dest => dest.PaidDate, opt => opt.MapFrom(src => src.PaidDate.Value.ToString("yyyy-MM-dd")))
            .ForMember(dest => dest.Interest, opt => opt.MapFrom(src => src.Interest.Value))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Situation))
            .ForMember(dest => dest.RecordCreationDate, opt => opt.MapFrom(src => src.RecordCreationDate.ToString("yyyy-MM-dd")))
            .ForMember(dest => dest.RecordModificationDate, opt => opt.MapFrom(src => src.RegistrationChangeDate.Value.ToString("yyyy-MM-dd")))
            .ForMember(dest => dest.RecordCreatedByUser, opt => opt.MapFrom(src => src.UserCreationRecord))
            .ForMember(dest => dest.RecordModifiedByUser, opt => opt.MapFrom(src => src.UserRecordChange));

        CreateMap<IEnumerable<PolicyDTO>, Collection<PolicyEntity>>();
        CreateMap<IEnumerable<PolicyEntity>, Collection<PolicyDTO>>();
        CreateMap<IEnumerable<InstallmentDTO>, Collection<InstallmentEntity>>();
        CreateMap<IEnumerable<InstallmentEntity>, Collection<InstallmentDTO>>();
        CreateMap<IEnumerable<PolicyEntity>, Collection<PolicyDetailsDTO>>();
        CreateMap<IEnumerable<InstallmentEntity>, Collection<InstallmentDetailsDTO>>();
    }
}
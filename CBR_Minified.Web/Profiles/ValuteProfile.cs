using AutoMapper;
using CBR_Minified.Domain.DbModels;
using CBR_Minified.Domain.Models;

namespace CBR_Minified.Web.Profiles;

public class ValuteProfile : Profile
{
    public ValuteProfile()
    {
        CreateMap<Valute, CurrencyCourse>()
            .ForMember(d => d.CurrencyId, opt => opt.MapFrom(s => s.Id));
    }
}

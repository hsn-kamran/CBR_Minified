using AutoMapper;
using CBR_Minified.DbModels;
using CBR_Minified.Models;

namespace CBR_Minified;

public class MapperConfig
{
    public static Mapper InitializeAutomapper()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Valute, CurrencyCourse>()
                .ForMember(d => d.CurrencyId, opt => opt.MapFrom(s => s.Id));
        });
        
        var mapper = new Mapper(config);

        return mapper;
    }
}

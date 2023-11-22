namespace CBR_Minified.Domain.DbModels;

public class CurrencyCourse
{
    public string CurrencyId { get; set; }
    public string NumCode { get; set; }
    public string CharCode { get; set; }
    public string Name { get; set; }
    public int Nominal { get; set; }
    public double Value { get; set; }
    public double VunitRate { get; set; }

    public DateTime Date { get; set; }
}

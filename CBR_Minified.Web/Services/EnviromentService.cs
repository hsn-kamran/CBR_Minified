using System.Text;

namespace CBR_Minified.Web.Services;

public class EnviromentService
{
    public void Initialize()
    {
        // для решения конфликта поля DateTime с колонкой типа timestamp with timezone в Postgres
        // https://ru.stackoverflow.com/questions/1416392/Ошибка-cannot-write-datetime-with-kind-local-to-postgresql-type-timestamp-with
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        // https://stackoverflow.com/questions/33579661/encoding-getencoding-cant-work-in-uwp-app
        // для работы с кириллицей в xml
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        Encoding.GetEncoding("windows-1251");
    }
}

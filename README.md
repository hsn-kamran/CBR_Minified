# CBR_Minified

Для локального запуска, нужно установить docker for desktop.
Выполнить команду в директории репозитория.

```shell
docker compose up -d
```

Также нужно установить [dotnet 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0), рантайм и SDK.

Обновить БД (например через Nuget Package Manager)
В качестве Default Project выбрать CBR_Minified.Domain.

```shell
Update-Database -StartupProject CBR_Minified.Web
```

Альтернативный вариант - выполнить скрипт из файла create.sql
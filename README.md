# CBR_Minified

Для локального запуска, нужно установить docker for desktop.
Выполнить команду в директории репозитория.

```shell
docker compose up -d
```

Также нужно установить [dotnet 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0), рантайм и SDK.

Выполнить миграции БД, но сперва нужно установить инструментарий EF:

```shell
dotnet tool install --global dotnet-ef
```

В директории репозитория выполнить команду

```shell
dotnet ef database update -p CBR_Minified
```
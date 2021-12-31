#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["TOTVS.WebApi/TOTVS.WebApi.csproj", "TOTVS.WebApi/"]
COPY ["TOTVS.DataAccessLayer/TOTVS.Persistence.csproj", "TOTVS.DataAccessLayer/"]
COPY ["TOTVS.Domain/TOTVS.Domain.csproj", "TOTVS.Domain/"]
COPY ["TOTVS.BusinessLogicLayer/TOTVS.Application.csproj", "TOTVS.BusinessLogicLayer/"]
RUN dotnet restore "TOTVS.WebApi/TOTVS.WebApi.csproj"
COPY . .
WORKDIR "/src/TOTVS.WebApi"
RUN dotnet build "TOTVS.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TOTVS.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TOTVS.WebApi.dll"]
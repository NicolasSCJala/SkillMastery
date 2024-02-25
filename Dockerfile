FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /src
COPY Directory.Build.props .
COPY Directory.Packages.props .

COPY ["SkillMasteryAPI/src/Core/SkillMasteryAPI.Domain/SkillMasteryAPI.Domain.csproj", "."]
RUN dotnet restore "SkillMasteryAPI.Domain.csproj"
COPY ["SkillMasteryAPI/src/Core/SkillMasteryAPI.Application/SkillMasteryAPI.Application.csproj", "."]
RUN dotnet restore "SkillMasteryAPI.Application.csproj"

COPY ["SkillMasteryAPI/src/Infrastructure/SkillMasteryAPI.Infrastructure/SkillMasteryAPI.Infrastructure.csproj", "."]
RUN dotnet restore "SkillMasteryAPI.Infrastructure.csproj"


COPY ["SkillMasteryAPI/src/API/SkillMasteryAPI.Api/SkillMasteryAPI.Api.csproj", "."]
RUN dotnet restore "SkillMasteryAPI.Api.csproj"

COPY ["SkillMasteryAPI/src/.", "."]
WORKDIR "/src/Core/SkillMasteryAPI.Models"
RUN dotnet build "SkillMasteryAPI.Models.csproj" -c Release -o /app/build
WORKDIR "/src/Core/SkillMasteryAPI.Application"
RUN dotnet build "SkillMasteryAPI.Application.csproj" -c Release -o /app/build

WORKDIR "/src/Infrastructure/SkillMasteryAPI.Infrastructure"
RUN dotnet build "SkillMasteryAPI.Infrastructure.csproj" -c Release -o /app/build

WORKDIR "/src/API/SkillMasteryAPI.Api"
RUN dotnet build "SkillMasteryAPI.Api.csproj" -c Release -o /app/build

FROM build AS publish
WORKDIR "/src/API/SkillMasteryAPI.Api"
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SkillMasteryAPI.Api.dll"]
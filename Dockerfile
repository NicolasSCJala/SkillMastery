FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
EXPOSE 80
EXPOSE 5432
EXPOSE 5209

# Copy csproj and restore as distinct layers
COPY SkillMasteryAPI/*.sln .
COPY SkillMasteryAPI/src/Core/SkillMasteryAPI.Application/*.csproj src/Core/SkillMasteryAPI.Application/
COPY SkillMasteryAPI/src/Core/SkillMasteryAPI.Domain/*.csproj src/Core/SkillMasteryAPI.Domain/
COPY SkillMasteryAPI/src/Infrastructure/SkillMasteryAPI.Infrastructure/*.csproj src/Infrastructure/SkillMasteryAPI.Infrastructure/
COPY SkillMasteryAPI/src/Presentation/SkillMasteryAPI.Presentation/*.csproj src/Presentation/SkillMasteryAPI.Presentation/

## Template for future testing
##COPY SkillMasteryAPI/tests/SkillMasteryAPI.Application.Tests/*.csproj tests/SkillMasteryAPI.Application.Tests/
##COPY SkillMasteryAPI/tests/SkillMasteryAPI.Infraestructure.Tests/*.csproj tests/SkillMasteryAPI.Infraestructure.Tests/
##COPY SkillMasteryAPI/tests/SkillMasteryAPI.Presentation.Tests/*.csproj tests/SkillMasteryAPI.Presentation.Tests/
##COPY SkillMasteryAPI/tests/SkillMasteryAPI.Integration.Tests/*.csproj tests/SkillMasteryAPI.Integration.Tests/

RUN dotnet restore src/Presentation/SkillMasteryAPI.Presentation/SkillMasteryAPI.Presentation.csproj

# Copy everything else and build
COPY SkillMasteryAPI/src/Core/SkillMasteryAPI.Application/. ./src/Core/SkillMasteryAPI.Application/
COPY SkillMasteryAPI/src/Core/SkillMasteryAPI.Domain/. ./src/Core/SkillMasteryAPI.Domain/
COPY SkillMasteryAPI/src/Infrastructure/SkillMasteryAPI.Infrastructure/. ./src/Infrastructure/SkillMasteryAPI.Infrastructure/
COPY SkillMasteryAPI/src/Presentation/SkillMasteryAPI.Presentation/. ./src/Presentation/SkillMasteryAPI.Presentation/


##COPY SkillMasteryAPI/tests/SkillMasteryAPI.Application.Tests/. ./tests/SkillMasteryAPI.Application.Tests/
##COPY SkillMasteryAPI/tests/SkillMasteryAPI.Infraestructure.Tests/. ./tests/SkillMasteryAPI.Infraestructure.Tests/
##COPY SkillMasteryAPI/tests/SkillMasteryAPI.Presentation.Tests/. ./tests/SkillMasteryAPI.Presentation.Tests/
##COPY SkillMasteryAPI/tests/SkillMasteryAPI.Integration.Tests/. ./tests/SkillMasteryAPI.Integration.Tests/

RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
##
COPY --from=build /app/out/ .
##COPY SkillMasteryAPI/src/SkillMasteryAPI.Presentation/Certificates/. /app/Certificates

ENTRYPOINT ["dotnet", "SkillMasteryAPI.Presentation.dll"]

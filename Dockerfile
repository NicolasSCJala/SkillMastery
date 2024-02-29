FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
EXPOSE 8080
EXPOSE 5432
EXPOSE 5209

# Copy csproj and restore as distinct layers
COPY SkillMasteryAPI/*.sln .
COPY SkillMasteryAPI/src/Core/SkillMasteryAPI.Application/*.csproj src/Core/SkillMasteryAPI.Application/
COPY SkillMasteryAPI/src/Core/SkillMasteryAPI.Domain/*.csproj src/Core/SkillMasteryAPI.Domain/
COPY SkillMasteryAPI/src/Infrastructure/SkillMasteryAPI.Infrastructure/*.csproj src/Infrastructure/SkillMasteryAPI.Infrastructure/
COPY SkillMasteryAPI/src/Presentation/SkillMasteryAPI.Presentation/*.csproj src/Presentation/SkillMasteryAPI.Presentation/

## Template for future testing
#COPY SkillMasteryAPI/test/SkillMasteryAPI.Application.Tests/*.csproj test/SkillMasteryAPI.Application.Tests/
#COPY SkillMasteryAPI/test/SkillMasteryAPI.Infrastructure.Tests/*.csproj test/SkillMasteryAPI.Infrastructure.Tests/
#COPY SkillMasteryAPI/test/SkillMasteryAPI.Presentation.Tests/*.csproj test/SkillMasteryAPI.Presentation.Tests/

RUN dotnet restore src/Presentation/SkillMasteryAPI.Presentation/SkillMasteryAPI.Presentation.csproj

# Copy everything else and build
COPY SkillMasteryAPI/src/Core/SkillMasteryAPI.Application/. ./src/Core/SkillMasteryAPI.Application/
COPY SkillMasteryAPI/src/Core/SkillMasteryAPI.Domain/. ./src/Core/SkillMasteryAPI.Domain/
COPY SkillMasteryAPI/src/Infrastructure/SkillMasteryAPI.Infrastructure/. ./src/Infrastructure/SkillMasteryAPI.Infrastructure/
COPY SkillMasteryAPI/src/Presentation/SkillMasteryAPI.Presentation/. ./src/Presentation/SkillMasteryAPI.Presentation/

#COPY SkillMasteryAPI/test/SkillMasteryAPI.Application.Tests/. ./test/SkillMasteryAPI.Application.Tests/
#COPY SkillMasteryAPI/test/SkillMasteryAPI.Infrastructure.Tests/. ./test/SkillMasteryAPI.Infraestructure.Tests/
#COPY SkillMasteryAPI/test/SkillMasteryAPI.Presentation.Tests/. ./test/SkillMasteryAPI.Presentation.Tests/

RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
##
COPY --from=build /app/out/ .
ENTRYPOINT ["dotnet", "SkillMasteryAPI.Presentation.dll"]
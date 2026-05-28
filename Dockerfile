# ─── Stage 1: Build ───────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
 
COPY ["src/FinalidadeEstudo.API/FinalidadeEstudo.API.csproj",                       "src/FinalidadeEstudo.API/"]
COPY ["src/FinalidadeEstudo.Application/FinalidadeEstudo.Application.csproj",       "src/FinalidadeEstudo.Application/"]
COPY ["src/FinalidadeEstudo.Domain/FinalidadeEstudo.Domain.csproj",                 "src/FinalidadeEstudo.Domain/"]
COPY ["src/FinalidadeEstudo.Infrastructure/FinalidadeEstudo.Infrastructure.csproj", "src/FinalidadeEstudo.Infrastructure/"]
COPY ["src/FinalidadeEstudo.CrossCutting/FinalidadeEstudo.CrossCutting.csproj",     "src/FinalidadeEstudo.CrossCutting/"]
 
RUN dotnet restore "src/FinalidadeEstudo.API/FinalidadeEstudo.API.csproj"
 
COPY . .
 
WORKDIR /src/src/FinalidadeEstudo.API
RUN dotnet publish -c Release -o /app/publish
 
# ─── Stage 2: Runtime ─────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
 
COPY --from=build /app/publish .
 
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080
 
EXPOSE 8080
ENTRYPOINT ["dotnet", "FinalidadeEstudo.API.dll"]

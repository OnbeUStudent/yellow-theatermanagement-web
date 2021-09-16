#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/Dii_TheaterManagement_Web/Dii_TheaterManagement_Web.csproj", "src/Dii_TheaterManagement_Web/"]
COPY ["tobenuget/Common/Common.csproj", "tobenuget/Common/"]
RUN dotnet restore "src/Dii_TheaterManagement_Web/Dii_TheaterManagement_Web.csproj"
COPY . .
WORKDIR "/src/src/Dii_TheaterManagement_Web"
RUN dotnet build "Dii_TheaterManagement_Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Dii_TheaterManagement_Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dii_TheaterManagement_Web.dll"]

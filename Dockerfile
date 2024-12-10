#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/clubpet_backend/clubpet_backend.csproj", "clubpet_backend/"]
RUN dotnet restore "./clubpet_backend/clubpet_backend.csproj"
COPY . .

RUN dotnet build "src/clubpet_backend/clubpet_backend.csproj" -c $BUILD_CONFIGURATION -o /app/build

RUN sed -i -e "s|^MinProtocol = .*|MinProtocol = TLSv1.0|g" "/etc/ssl/openssl.cnf"

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "src/clubpet_backend/clubpet_backend.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "clubpet_backend.dll"]
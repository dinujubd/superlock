#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["SuperLocker.Auth/SuperLocker.Auth.csproj", "SuperLocker.Auth/"]
RUN dotnet restore "SuperLocker.Auth/SuperLocker.Auth.csproj"
COPY . .
WORKDIR "/src/SuperLocker.Auth"
RUN dotnet build "SuperLocker.Auth.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SuperLocker.Auth.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SuperLocker.Auth.dll"]
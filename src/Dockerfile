#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /src
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["SuperLocker.Api/SuperLocker.Api.csproj", "SuperLocker.Api/"]
RUN dotnet restore "SuperLocker.Api/SuperLocker.Api.csproj"
COPY . .
WORKDIR "/src/SuperLocker.Api"
RUN dotnet build "SuperLocker.Api.csproj" -c Release -o /src/build

FROM build AS publish
RUN dotnet publish "SuperLocker.Api.csproj" -c Release -o /src/publish

FROM base AS final
WORKDIR /src
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SuperLocker.Api.dll"]
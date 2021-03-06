#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["SuperLocker.Api/SuperLocker.Api.csproj", "SuperLocker.Api/"]
COPY ["SuperLocker.CommandHalder/SuperLocker.CommandHalder.csproj", "SuperLocker.CommandHalder/"]
COPY ["SuperLocker.Core/SuperLocker.Core.csproj", "SuperLocker.Core/"]
COPY ["SuperLocker.DataContext/SuperLocker.DataContext.csproj", "SuperLocker.DataContext/"]
COPY ["SuperLocker.QueryHandler/SuperLocker.QueryHandler.csproj", "SuperLocker.QueryHandler/"]
RUN dotnet restore "SuperLocker.Api/SuperLocker.Api.csproj"
COPY . .
WORKDIR "/src/SuperLocker.Api"
RUN dotnet build "SuperLocker.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SuperLocker.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SuperLocker.Api.dll"]
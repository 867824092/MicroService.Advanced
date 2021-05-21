#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/4--AuthenticationCenter/MicroService.Authentication.Center/MicroService.Authentication.Center.csproj", "src/4--AuthenticationCenter/MicroService.Authentication.Center/"]
RUN dotnet restore "src/4--AuthenticationCenter/MicroService.Authentication.Center/MicroService.Authentication.Center.csproj"
COPY . .
WORKDIR "/src/src/4--AuthenticationCenter/MicroService.Authentication.Center"
RUN dotnet build "MicroService.Authentication.Center.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MicroService.Authentication.Center.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MicroService.Authentication.Center.dll"]
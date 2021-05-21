#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/2--Service/Order/MicroService.Order/MicroService.Order.csproj", "src/2--Service/Order/MicroService.Order/"]
RUN dotnet restore "src/2--Service/Order/MicroService.Order/MicroService.Order.csproj"
COPY . .
WORKDIR "/src/src/2--Service/Order/MicroService.Order"
RUN dotnet build "MicroService.Order.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MicroService.Order.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MicroService.Order.dll"]
#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["DeliveryBoyApp/DeliveryBoyApp.csproj", "DeliveryBoyApp/"]
COPY ["DeliveryBoy.BusinessLayer/DeliveryBoy.BusinessLayer.csproj", "DeliveryBoy.BusinessLayer/"]
RUN dotnet restore "DeliveryBoyApp/DeliveryBoyApp.csproj"
COPY . .
WORKDIR "/src/DeliveryBoyApp"
RUN dotnet build "DeliveryBoyApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DeliveryBoyApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DeliveryBoyApp.dll"]
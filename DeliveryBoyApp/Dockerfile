#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM node:12.2.0

# install chrome for protractor tests
RUN wget -q -O - https://dl-ssl.google.com/linux/linux_signing_key.pub | apt-key add -
RUN sh -c 'echo "deb [arch=amd64] http://dl.google.com/linux/chrome/deb/ stable main" >> /etc/apt/sources.list.d/google.list'
RUN apt-get update && apt-get install -yq google-chrome-stable

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 443

# add `/app/node_modules/.bin` to $PATH
ENV PATH /app/node_modules/.bin:$PATH

# install and cache app dependencies
COPY package.json /app/package.json
RUN npm install
RUN npm install -g @angular/cli@8.2.12

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
#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM node:12.2.0

#RUN curl -sL https://deb.nodesource.com/setup_10.x | bash -
#RUN apt-get install -y nodejs

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 443

# add `/app/node_modules/.bin` to $PATH
#ENV PATH /app/node_modules/.bin:$PATH

# install and cache app dependencies
#COPY package.json /app/package.json
#RUN npm install
#RUN npm install -g @angular/cli@8.2.12
#RUN apt-get update -yq \
#    && apt-get install curl gnupg -yq \
#    && curl -sL https://deb.nodesource.com/setup_10.x | bash \
#    && apt-get install nodejs -yq
#RUN echo "NODE Version:" && node --version
#RUN echo "NPM Version:" && npm --version

#RUN npm install

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["DeliveryBoyApp/DeliveryBoyApp.csproj", "DeliveryBoyApp/"]
COPY ["DeliveryBoy.BusinessLayer/DeliveryBoy.BusinessLayer.csproj", "DeliveryBoy.BusinessLayer/"]
RUN dotnet restore "DeliveryBoyApp/DeliveryBoyApp.csproj"
COPY . .
WORKDIR "/src/DeliveryBoyApp"
RUN dotnet build "DeliveryBoyApp.csproj" -c Release -o /app/build
RUN apt-get update && \
    apt-get install -y wget && \
    apt-get install -y gnupg2 && \
    wget -qO- https://deb.nodesource.com/setup_10.x | bash - && \
    apt-get install -y build-essential nodejs

FROM build AS publish
RUN dotnet publish "DeliveryBoyApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DeliveryBoyApp.dll"]
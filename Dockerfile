#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base

WORKDIR /app

#FIX for W2012 server
COPY openssl.cnf /etc/ssl/openssl.cnf

RUN apt update -y \
    && apt install ca-certificates gss-ntlmssp libldap-2.4-2 net-tools -y \
    && update-ca-certificates \
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/* 

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY /src .

RUN dotnet nuget add source "https://tb.deloitte.ru/api/v4/groups/119/-/packages/nuget/index.json" --name GitLab --username gitlab_dotnet_read_package --password DczPj981hb22fN6aiyPx --store-password-in-clear-text
RUN dotnet restore "EmployeeApi/EmployeeApi.csproj" 
RUN dotnet build "EmployeeApi/EmployeeApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EmployeeApi/EmployeeApi.csproj" -c Release -o /app/publish

FROM base AS final

ENV ASPNETCORE_URLS=http://+:80
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EmployeeApi.dll"]
EXPOSE 80

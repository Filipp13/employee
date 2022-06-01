#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal AS base

WORKDIR /app

RUN apt-get update -y \
    && apt-get install ca-certificates gss-ntlmssp libldap-2.4-2 net-tools -y \
    && update-ca-certificates \
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/* 

FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /src
COPY /src .

RUN dotnet restore "EmployeeApi/Employee.Api.csproj" 

FROM build AS publish
RUN dotnet publish "EmployeeApi/Employee.Api.csproj" -c Release -o /app/publish

FROM base AS final

ENV ASPNETCORE_URLS=http://+:5000
WORKDIR /app
COPY /src/EmployeeApi/appsettings.Development.json ./appsettings.json
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Employee.dll"]
EXPOSE 5000
EXPOSE 5002

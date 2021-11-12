#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

COPY certs/deloitte-ca.crt /usr/local/share/ca-certificates/deloitte/
#FIX for W2012 server
COPY openssl.cnf /etc/ssl/openssl.cnf
WORKDIR /app
RUN apt update -y \
    && apt install ca-certificates gss-ntlmssp libldap-2.4-2 net-tools -y \
    && update-ca-certificates \
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/* \
    && mkdir tempfiles
    #TEMP should be in code
EXPOSE 5000


FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Employee/Employee.csproj", "Employee/"]
RUN dotnet restore "Employee/Employee.csproj"
COPY . .
WORKDIR "/src/Employee"
RUN dotnet build "Employee.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Employee.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Employee.dll"]
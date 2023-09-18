FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

ENV http_proxy="http://192.168.239.14:8080"
ENV https_proxy="http://192.168.239.14:8080"
ENV no_proxy="localhost,127.0.0.0/8,.grupoaec.com.br,192.168.*,192.168.225.121,192.168.225.157,192.168.225.148,192.168.225.96,192.168.225.167,192.168.225.160,192.168.225.93"

WORKDIR /src
COPY . .
RUN dotnet restore "BuscaBrasilApi.csproj"

FROM build AS publish
RUN dotnet publish "BuscaBrasilApi.csproj" -c Release -o /app/publish

FROM base AS final
ENV http_proxy="http://192.168.239.14:8080"
ENV https_proxy="http://192.168.239.14:8080"
ENV no_proxy="localhost,127.0.0.0/8,.grupoaec.com.br,192.168.*,192.168.225.121,192.168.225.157,192.168.225.148,192.168.225.96,192.168.225.167,192.168.225.160,192.168.225.218,192.168.225.93"
WORKDIR /app
COPY --from=publish /app/publish .
RUN sed -i 's/DEFAULT@SECLEVEL=2/DEFAULT@SECLEVEL=1/g' /etc/ssl/openssl.cnf
ENTRYPOINT ["dotnet", "BuscaBrasilApi.dll"]
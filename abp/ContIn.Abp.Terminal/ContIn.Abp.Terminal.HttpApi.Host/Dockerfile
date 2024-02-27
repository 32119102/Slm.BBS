#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base

RUN sed -i "s@http://deb.debian.org@http://mirrors.aliyun.com@g" /etc/apt/sources.list
RUN apt-get update -y && apt-get install -y libgdiplus && apt-get clean && ln -s /usr/lib/libgdiplus.so /usr/lib/gdiplus.dll

WORKDIR /app
EXPOSE 80
#默认80端口
#ENV ASPNETCORE_URLS http://+:5128

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ContIn.Abp.Terminal.HttpApi.Host/ContIn.Abp.Terminal.HttpApi.Host.csproj", "ContIn.Abp.Terminal.HttpApi.Host/"]
COPY ["ContIn.Abp.Terminal.Swagger/ContIn.Abp.Terminal.Swagger.csproj", "ContIn.Abp.Terminal.Swagger/"]
COPY ["ContIn.Abp.Terminal.HttpApi.Client/ContIn.Abp.Terminal.HttpApi.Client.csproj", "ContIn.Abp.Terminal.HttpApi.Client/"]
COPY ["ContIn.Abp.Terminal.Application.Contracts/ContIn.Abp.Terminal.Application.Contracts.csproj", "ContIn.Abp.Terminal.Application.Contracts/"]
COPY ["ContIn.Abp.Terminal.Domain.Shared/ContIn.Abp.Terminal.Domain.Shared.csproj", "ContIn.Abp.Terminal.Domain.Shared/"]
COPY ["ContIn.Abp.Terminal.HttpApi/ContIn.Abp.Terminal.HttpApi.csproj", "ContIn.Abp.Terminal.HttpApi/"]
COPY ["ContIn.Abp.Terminal.Core/ContIn.Abp.Terminal.Core.csproj", "ContIn.Abp.Terminal.Core/"]
COPY ["ContIn.Abp.Terminal.EntityFrameworkCore/ContIn.Abp.Terminal.EntityFrameworkCore.csproj", "ContIn.Abp.Terminal.EntityFrameworkCore/"]
COPY ["ContIn.Abp.Terminal.Domain/ContIn.Abp.Terminal.Domain.csproj", "ContIn.Abp.Terminal.Domain/"]
COPY ["ContIn.Abp.Terminal.Application/ContIn.Abp.Terminal.Application.csproj", "ContIn.Abp.Terminal.Application/"]
COPY ["ContIn.Abp.Terminal.FreeRedis/ContIn.Abp.Terminal.FreeRedis.csproj", "ContIn.Abp.Terminal.FreeRedis/"]
COPY ["ContIn.Abp.Terminal.Caching.FreeRedis/ContIn.Abp.Terminal.Caching.FreeRedis.csproj", "ContIn.Abp.Terminal.Caching.FreeRedis/"]
RUN dotnet restore "ContIn.Abp.Terminal.HttpApi.Host/ContIn.Abp.Terminal.HttpApi.Host.csproj"
COPY . .
WORKDIR "/src/ContIn.Abp.Terminal.HttpApi.Host"
RUN dotnet build "ContIn.Abp.Terminal.HttpApi.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ContIn.Abp.Terminal.HttpApi.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

#设置时区，默认UTC时间
ENV TZ=Asia/Shanghai
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

ENTRYPOINT ["dotnet", "ContIn.Abp.Terminal.HttpApi.Host.dll"]
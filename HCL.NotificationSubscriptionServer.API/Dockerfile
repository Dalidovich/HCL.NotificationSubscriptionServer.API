#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["HCL.NotificationSubscriptionServer.API/HCL.NotificationSubscriptionServer.API.csproj", "HCL.NotificationSubscriptionServer.API/"]
COPY ["HCL.NotificationSubscriptionServer.API.BLL/HCL.NotificationSubscriptionServer.API.BLL.csproj", "HCL.NotificationSubscriptionServer.API.BLL/"]
COPY ["HCL.NotificationSubscriptionServer.API.DAL/HCL.NotificationSubscriptionServer.API.DAL.csproj", "HCL.NotificationSubscriptionServer.API.DAL/"]
COPY ["HCL.NotificationSubscriptionServer.API.Domain/HCL.NotificationSubscriptionServer.API.Domain.csproj", "HCL.NotificationSubscriptionServer.API.Domain/"]
RUN dotnet restore "HCL.NotificationSubscriptionServer.API/HCL.NotificationSubscriptionServer.API.csproj"
COPY . .
WORKDIR "/src/HCL.NotificationSubscriptionServer.API"
RUN dotnet build "HCL.NotificationSubscriptionServer.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HCL.NotificationSubscriptionServer.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HCL.NotificationSubscriptionServer.API.dll"]
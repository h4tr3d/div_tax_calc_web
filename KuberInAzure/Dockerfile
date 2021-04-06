FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["KuberInAzure/KuberInAzure.csproj", "KuberInAzure/"]

RUN dotnet restore "KuberInAzure/KuberInAzure.csproj"
COPY . .
WORKDIR "/src/KuberInAzure"
RUN dotnet build "KuberInAzure.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "KuberInAzure.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KuberInAzure.dll"]
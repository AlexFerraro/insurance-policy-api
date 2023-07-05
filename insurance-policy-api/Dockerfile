FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["insurance-policy-api/insurance-policy-api.csproj", "insurance-policy-api/"]
RUN dotnet restore "insurance-policy-api/insurance-policy-api.csproj"
COPY . .
WORKDIR "/src/insurance-policy-api"
RUN dotnet build "insurance-policy-api.csproj" -c Release -o /app/build --runtime linux-x64

FROM build AS publish
RUN dotnet publish "insurance-policy-api.csproj" -c Release -o /app/publish --runtime linux-x64

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "insurance-policy-api.dll"]
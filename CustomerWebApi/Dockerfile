FROM microsoft/dotnet:2.1-aspnetcore-runtime-nanoserver-1709 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk-nanoserver-1709 AS build
WORKDIR /src
COPY CustomerWebApi/CustomerWebApi.csproj CustomerWebApi/
RUN dotnet restore CustomerWebApi/CustomerWebApi.csproj
COPY . .
WORKDIR /src/CustomerWebApi
RUN dotnet build CustomerWebApi.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish CustomerWebApi.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "CustomerWebApi.dll"]

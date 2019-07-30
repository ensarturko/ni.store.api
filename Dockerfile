FROM microsoft/dotnet:2.2-sdk AS base
COPY . .

RUN dotnet restore src/Ni.Store.Api/Ni.Store.Api.csproj
RUN dotnet restore tests/Ni.Store.Api.Tests/Ni.Store.Api.Tests.csproj

RUN dotnet test tests/Ni.Store.Api.Tests/Ni.Store.Api.Tests.csproj
RUN dotnet publish src/Ni.Store.Api/Ni.Store.Api.csproj -c Release -o /app

FROM microsoft/dotnet:2.2-aspnetcore-runtime
WORKDIR /app
COPY --from=base /app .
ENTRYPOINT ["dotnet", "Ni.Store.Api.dll"]
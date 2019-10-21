FROM mcr.microsoft.com/dotnet/core/aspnet:3.0.0-alpine3.9 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0.100-alpine3.9 AS build
WORKDIR /src
COPY ["MyAspCoreApp/MyAspCoreApp.csproj", "MyAspCoreApp/"]
RUN dotnet restore "MyAspCoreApp/MyAspCoreApp.csproj"
COPY . .
WORKDIR "/src/MyAspCoreApp"
RUN dotnet build "MyAspCoreApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyAspCoreApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyAspCoreApp.dll"]
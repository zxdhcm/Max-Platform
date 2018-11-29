FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["Max.Platform.Web.Host/Max.Platform.Web.Host.csproj", "src/Max.Platform.Web.Host/"]
COPY ["Max.Platform.Web.Core/Max.Platform.Web.Core.csproj", "src/Max.Platform.Web.Core/"]
COPY ["Max.Platform.Application/Max.Platform.Application.csproj", "src/Max.Platform.Application/"]
COPY ["Max.Platform.Core/Max.Platform.Core.csproj", "src/Max.Platform.Core/"]
COPY ["Max.Platform.EntityFrameworkCore/Max.Platform.EntityFrameworkCore.csproj", "src/Max.Platform.EntityFrameworkCore/"]
RUN dotnet restore "src/Max.Platform.Web.Host/Max.Platform.Web.Host.csproj"
COPY . .
WORKDIR "/src/src/Max.Platform.Web.Host"
RUN dotnet build "Max.Platform.Web.Host.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Max.Platform.Web.Host.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Max.Platform.Web.Host.dll"]



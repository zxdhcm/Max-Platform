FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /app

# copy csproj and restore as distinct layers
#COPY *.sln .
COPY *.csproj ./Max.Platform.Web.Host/
RUN dotnet restore ./Max.Platform.Web.Host/*.csproj

# copy everything else and build app
COPY . . ./Max.Platform.Web.Host/
WORKDIR /app/Max.Platform.Web.Host
RUN dotnet publish -c Release -o out


FROM microsoft/dotnet:2.1-aspnetcore-runtime AS runtime
WORKDIR /app
COPY --from=build /app/Max.Platform.Web.Host/out ./
ENTRYPOINT ["dotnet", "Max.Platform.Web.Host.dll"]
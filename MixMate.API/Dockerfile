#Stage 1: Build Stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src

#Restore
COPY ["MixMate.API/MixMate.API.csproj", "MixMate.API/"]
COPY ["MixMate.Core/MixMate.Core.csproj", "MixMate.Core/"]
COPY ["MixMate.DataAccess/MixMate.DataAccess.csproj", "MixMate.DataAccess/"]
RUN dotnet restore "MixMate.API/MixMate.API.csproj"

#Build
COPY ["MixMate.API/", "MixMate.API/"]
COPY ["MixMate.Core/", "MixMate.Core/"]
COPY ["MixMate.DataAccess/", "MixMate.DataAccess/"]
RUN dotnet build "MixMate.API/MixMate.API.csproj" -o /app/build

#Stage 2: Publish Stage
FROM build AS publish
RUN dotnet publish "MixMate.API/MixMate.API.csproj" -o /app/publish

#Stage 3: Run Stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
ENV ASPNETCORE_HTTP_PORTS=5165
EXPOSE 5165
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT [ "dotnet", "MixMate.API.dll" ]
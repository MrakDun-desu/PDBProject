#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
RUN mkdir "Dal.Mongo"
RUN mkdir "Dal.Common"
COPY ["ReadApi/ReadApi.csproj", "ReadApi/"]
COPY ["Dal.Mongo/Dal.Mongo.csproj", "Dal.Mongo/"]
COPY ["Dal.Common/Dal.Common.csproj", "Dal.Common/"]
RUN dotnet restore "ReadApi/ReadApi.csproj"
COPY ["Dal.Mongo", "Dal.Mongo"]
COPY ["Dal.Common", "Dal.Common"]
WORKDIR "/src/ReadApi"
COPY ReadApi .
RUN dotnet build "ReadApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ReadApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ReadApi.dll"]
# get .NET SDK dockerimage
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
# make a workdirectory called app and expose HTTP/S ports
WORKDIR /app
EXPOSE 80
EXPOSE 443

# get another dockerimage
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
# make a source directory, copy over the project file
WORKDIR /src
RUN mkdir "Dal.Mongo"
RUN mkdir "Dal.Common"
COPY ["WriteApi/WriteApi.csproj", "WriteApi/"]
COPY ["Dal.Mongo/Dal.Mongo.csproj", "Dal.Mongo/"]
COPY ["Dal.Common/Dal.Common.csproj", "Dal.Common/"]
# once the project file is copied, update the dependencies
RUN dotnet restore "WriteApi/WriteApi.csproj"
# after updating dependencies, copy the rest of the code
COPY ["Dal.Mongo", "Dal.Mongo"]
COPY ["Dal.Common", "Dal.Common"]
WORKDIR "/src/WriteApi"
COPY WriteApi .
# go to the project directory and build it
RUN dotnet build "WriteApi.csproj" -c Release -o /app/build

# after building, publish the app
FROM build AS publish
RUN dotnet publish "WriteApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# and finally copy over the built app to the base image and run it
# (we do it like this because then the final image doesn't need to have the source files, just the build)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WriteApi.dll"]

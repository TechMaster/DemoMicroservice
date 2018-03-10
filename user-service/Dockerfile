FROM microsoft/aspnetcore-build:latest AS build-env
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM microsoft/aspnetcore:latest
WORKDIR /app
COPY --from=build-env /app/out .

EXPOSE 5000/tcp

ENTRYPOINT ["dotnet", "user-service.dll"]
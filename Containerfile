FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# get bin
COPY ["swiftuibackend.csproj", "./"]
RUN dotnet restore "swiftuibackend.csproj"

# compile bin
COPY . .
RUN dotnet publish "swiftuibackend.csproj" -c Release -o /app/publish


FROM mcr.microsoft.com/dotnet/sdk:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "swiftuibackend.dll"]

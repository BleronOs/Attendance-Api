FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["AttendanceTracker.Api/AttendanceTracker.Api.csproj", "AttendanceTracker.Api/"]
RUN dotnet restore "AttendanceTracker.Api/AttendanceTracker.Api.csproj"
COPY . .
WORKDIR "/src/AttendanceTracker.Api"
RUN dotnet build "AttendanceTracker.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AttendanceTracker.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AttendanceTracker.Api.dll"]

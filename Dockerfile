# Stage 1: Restore and Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Install git and clone only the latest commit
RUN apt-get update && \
    apt-get install -y --no-install-recommends git && \
    git clone --depth 1 https://github.com/Moustafa-Safwat/PortfolioHub repo && \
    apt-get remove -y git && \
    apt-get autoremove -y && \
    rm -rf /var/lib/apt/lists/*

WORKDIR /src/repo

# Restore and build using the cloned source
RUN dotnet restore
RUN dotnet publish PortfolioHub.Web/PortfolioHub.Web.csproj -c Release -o /app/publish --no-restore

# Optionally generate a dev certificate (if needed)
RUN dotnet dev-certs https -ep /tmp/aspnetapp.pfx -p password

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Copy published output
COPY --from=build /app/publish ./

EXPOSE 8090

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8090

HEALTHCHECK --interval=30s --timeout=5s --start-period=10s --retries=3 \
  CMD curl --fail http://localhost:8090/health || exit 1

ENTRYPOINT ["dotnet", "PortfolioHub.Web.dll"]
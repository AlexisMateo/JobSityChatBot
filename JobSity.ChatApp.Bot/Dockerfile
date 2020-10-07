FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

COPY *.sln .
# Copy csproj and restore as distinct layers
COPY JobSity.ChatApp.IdentityServer/*.csproj ./JobSity.ChatApp.IdentityServer/
COPY JobSity.ChatApp.Core/*.csproj ./JobSity.ChatApp.Core/
COPY JobSity.ChatApp.Infrastructure/*.csproj ./JobSity.ChatApp.Infrastructure/
COPY JobSity.ChatApp.Api/*.csproj ./JobSity.ChatApp.Api/
COPY JobSity.ChatApp.WebApp/*.csproj ./JobSity.ChatApp.WebApp/
COPY JobSity.ChatApp.Bot/*.csproj ./JobSity.ChatApp.Bot/
COPY JobSity.ChatApp.Test/*.csproj ./JobSity.ChatApp.Test/


RUN dotnet restore

# Copy everything else and build
COPY JobSity.ChatApp.IdentityServer/. ./JobSity.ChatApp.IdentityServer/
COPY JobSity.ChatApp.Core/. ./JobSity.ChatApp.Core/
COPY JobSity.ChatApp.Infrastructure/. ./JobSity.ChatApp.Infrastructure/
COPY JobSity.ChatApp.Api/. ./JobSity.ChatApp.Api/
COPY JobSity.ChatApp.WebApp/. ./JobSity.ChatApp.WebApp/
COPY JobSity.ChatApp.Bot/. ./JobSity.ChatApp.Bot/
COPY JobSity.ChatApp.Test/. ./JobSity.ChatApp.Test/

WORKDIR /app/JobSity.ChatApp.Bot
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/JobSity.ChatApp.Bot/out .
ENTRYPOINT ["dotnet", "JobSity.ChatApp.Bot.dll"]
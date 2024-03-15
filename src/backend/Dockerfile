#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Build.props", "."]
COPY ["SomeSandwich.FakeMentorus.Web/SomeSandwich.FakeMentorus.Web.csproj", "SomeSandwich.FakeMentorus.Web/"]
COPY ["SomeSandwich.FakeMentorus.Infrastructure.DataAccess/SomeSandwich.FakeMentorus.Infrastructure.DataAccess.csproj", "SomeSandwich.FakeMentorus.Infrastructure.DataAccess/"]
COPY ["SomeSandwich.FakeMentorus.Infrastructure.Abstractions/SomeSandwich.FakeMentorus.Infrastructure.Abstractions.csproj", "SomeSandwich.FakeMentorus.Infrastructure.Abstractions/"]
COPY ["SomeSandwich.FakeMentorus.Domain/SomeSandwich.FakeMentorus.Domain.csproj", "SomeSandwich.FakeMentorus.Domain/"]
COPY ["SomeSandwich.FakeMentorus.Infrastructure/SomeSandwich.FakeMentorus.Infrastructure.csproj", "SomeSandwich.FakeMentorus.Infrastructure/"]
COPY ["SomeSandwich.FakeMentorus.UseCases/SomeSandwich.FakeMentorus.UseCases.csproj", "SomeSandwich.FakeMentorus.UseCases/"]
COPY ["SomeSandwich.FakeMentorus.UseCases.Common/SomeSandwich.FakeMentorus.UseCases.Common.csproj", "SomeSandwich.FakeMentorus.UseCases.Common/"]
RUN dotnet restore "./SomeSandwich.FakeMentorus.Web/./SomeSandwich.FakeMentorus.Web.csproj"
COPY . .
WORKDIR "/src/SomeSandwich.FakeMentorus.Web"
RUN dotnet build "./SomeSandwich.FakeMentorus.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SomeSandwich.FakeMentorus.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SomeSandwich.FakeMentorus.Web.dll"]

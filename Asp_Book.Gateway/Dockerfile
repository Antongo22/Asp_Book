FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["Directory.Packages.props", "./"]
COPY ["Asp_Book.Gateway/Asp_Book.Gateway.csproj", "Asp_Book.Gateway/"]
RUN dotnet restore "Asp_Book.Gateway/Asp_Book.Gateway.csproj"
COPY Asp_Book.Gateway/ ./Asp_Book.Gateway/
WORKDIR /src/Asp_Book.Gateway
RUN dotnet build "Asp_Book.Gateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Asp_Book.Gateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN mkdir -p /app/keys
ENV ASPNETCORE_URLS=http://+:5000
ENTRYPOINT ["dotnet", "Asp_Book.Gateway.dll"]

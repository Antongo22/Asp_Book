FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["Asp_Book.Gateway.csproj", "./"]
RUN dotnet restore "Asp_Book.Gateway.csproj"
COPY . .
RUN dotnet build "Asp_Book.Gateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Asp_Book.Gateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:5000
ENTRYPOINT ["dotnet", "Asp_Book.Gateway.dll"]

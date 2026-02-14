FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 5009

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["Asp_Book.Chapter09.csproj", "./"]
RUN dotnet restore "Asp_Book.Chapter09.csproj"
COPY . .
RUN dotnet build "Asp_Book.Chapter09.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Asp_Book.Chapter09.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:5009
ENTRYPOINT ["dotnet", "Asp_Book.Chapter09.dll"]

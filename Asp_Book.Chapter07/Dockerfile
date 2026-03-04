FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 5007

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["Directory.Packages.props", "./"]
COPY ["Asp_Book.Chapter07/Asp_Book.Chapter07.csproj", "Asp_Book.Chapter07/"]
RUN dotnet restore "Asp_Book.Chapter07/Asp_Book.Chapter07.csproj"
COPY Asp_Book.Chapter07/ ./Asp_Book.Chapter07/
WORKDIR /src/Asp_Book.Chapter07
RUN dotnet build "Asp_Book.Chapter07.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Asp_Book.Chapter07.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN mkdir -p /app/keys
ENV ASPNETCORE_URLS=http://+:5007
ENTRYPOINT ["dotnet", "Asp_Book.Chapter07.dll"]

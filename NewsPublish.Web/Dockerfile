FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY NewsPublish.Web/NewsPublish.Web.csproj NewsPublish.Web/
COPY NewsPulish.DAL/NewsPulish.DAL.csproj NewsPulish.DAL/
COPY NewsPublish.Model/NewsPublish.Model.csproj NewsPublish.Model/
RUN dotnet restore NewsPublish.Web/NewsPublish.Web.csproj
COPY . .
WORKDIR /src/NewsPublish.Web
RUN dotnet build NewsPublish.Web.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish NewsPublish.Web.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "NewsPublish.Web.dll"]

# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine
WORKDIR /app
EXPOSE 80
COPY published/ .
COPY BanqueTardi.db /app/BanqueTardi.db

#Indicate production environment
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "BanqueTardi.dll"]

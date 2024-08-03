#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
RUN useradd -ms /bin/bash kfaLocalUser
USER kfaLocalUser
WORKDIR /app
EXPOSE 80
EXPOSE 443

USER kfaLocalUser
COPY BackEnd/src/KFA.SupportAssistant.Web/bin/Release/net8.0/linux-x64 ./
RUN ls

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final-env
WORKDIR /app
COPY --from=build-env /app/ .
ENTRYPOINT [ "dotnet", "KFA.SupportAssistant.Web.dll" ]

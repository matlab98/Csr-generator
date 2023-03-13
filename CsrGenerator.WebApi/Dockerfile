FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY /bin/release/net6.0/publish/ ./
EXPOSE 7118
ENV ASPNETCORE_URLS=http://*:7118
RUN ln -fs /usr/share/zoneinfo/America/Bogota /etc/localtime
RUN dpkg-reconfigure -f noninteractive tzdata
ENTRYPOINT ["dotnet", "CsrGenerator.WebApi.dll"]


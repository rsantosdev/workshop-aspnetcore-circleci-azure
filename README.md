# Workshop: Exemplo de Aplicação ASP.NET Core 1.1 (Web API) com EFCore acessando SqlServer rodando no Linux

##.NET Core e Node.js
[Instale o Node.js](https://nodejs.org/en/)

[Instale o .NET Core](http://www.dot.net)

##Yoeman
Para instalar o Yoeman e o Scaffolding do ASP.NET Core (o bower é requerido pelo Scaffolding), utilize o NPM, através dos seguintes comandos:

`npm i -g bower`

`npm i -g yo`

`npm i -g generator-aspnet`


Para criar o projeto do zero, você pode utilizar o comando `yo` e seguir as instruções informadas no terminal.

Neste exemplo utilizamos os projetos: `Web API Application` e `Unit test project (xUnit.net)`

## Migrations
Para criar o primeiro pacote do migration, execute o comando abaixo:

`dotnet ef migrations add MigracaoInicial`

## Docker - Container SqlServer no Linux
[Docker](https://www.docker.com/products/docker)

Para subir a imagem do container SqlServer (linux), execute o comando abaixo:

`sudo docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=Workshop@123' -p 1433:1433 -d microsoft/mssql-server-Linux`

## Visual Studio Code
[Visual Studio Code](https://code.visualstudio.com/)

## Visual Studio Code Extensions
- C#
- C# Extensions
- vscode-icons

##CircleCI
Integração contínua com [CircleCI](http://circleci.com)

## Material desenvolvido pelos participantes do curso
- http://codefc.com.br/workshop-asp-net-core/
- http://codefc.com.br/instalando-o-visual-studio-code/
- http://joseotavio.com/2016/12/01/workshop-net-core-efcore-azure-circleci-xunit.html
- https://www.youtube.com/watch?v=jNPM4FDdXjw&feature=youtu.be
- https://github.com/andreluizsecco/EFCore.Demo
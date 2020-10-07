# JobSityChatBot

## About the project

This project alow people to chat after via web browser using tecnologies like SignalR and RabitMQ.

## Projects

### JobSity.ChatApp.IdentityServer

MVC project that grant users and applicatios to comunicate eachother. Also, is responsible for registering and login users.

### JobSity.ChatApp.Api

This Api host SignalR, who is in charge of the comunication between chat users by the web platform and share messages between 
users and chatbot.

### JobSity.ChatApp.Bot

Worker Service in charge of request information to stock api and send back the information to the chat api hub.

### JobSity.ChatApp.WebApp

Razor Web Page in used for the UI.

## Requirements

* Sql Server https://hub.docker.com/_/microsoft-mssql-server
* RabbitMQ https://www.rabbitmq.com/download.html
* Seq https://hub.docker.com/r/datalust/seq/

## Configuration

* Install local developer certificates "dotnet dev-certs https --trust"
* Run projects with "dotnet run"
    ** JobSity.ChatApp.IdentityServer
    ** JobSity.ChatApp.Api
    ** JobSity.ChatApp.Bot
    ** JobSity.ChatApp.WebApp

"use strict";

function getAccessTockenAndConnectToSignalR()
{
    fetch(`${document.location.origin}/?handler=Token`).then(function(response) {
        if(response.ok) {
          response.json().then(function(token) {
            
            connectToSignalR(token);

          });
        } else {
          console.error('Could not get the token');
        }
      })
      .catch(function(error) {
        console.error('error:' + error.message);
      });
}

function connectToSignalR(token)
{
    var connection = new signalR.HubConnectionBuilder().withUrl(`http://localhost:5001/chatHub?token=${token}`).build();

    connection.on("ReceiveMessage", function(user, message){

        var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
        var li = document.createElement("li");
        li.textContent  = user + " says " + msg;

        document.getElementById("chatMessage").appendChild(li);

    });

    connection.start().then(function(){
        console.log("connection stablished");
    }).catch(function (err){
        return console.err(err.toString());
    });

    document.getElementById("btn-chat").addEventListener("click", function(event){
        var user = document.getElementById("btn-input").value;
        var message = document.getElementById("btn-input").value;

        connection.invoke("SendMessage", user, message).catch(function (err) {
            return console.error(err.toString());
        });
        event.preventDefault();
    });

}

window.onload = getAccessTockenAndConnectToSignalR();
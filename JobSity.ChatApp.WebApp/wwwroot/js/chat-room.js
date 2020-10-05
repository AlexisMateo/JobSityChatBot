"use strict";

function getAccessTockenAndConnectToSignalR()
{
    fetch(`${document.location.origin}/?handler=RetrieveChatApiUrl`).then(function(response) {
        if(response.ok) {
          response.json().then(function(apiUrl) {
            
            connectToSignalR(apiUrl);

          });
        } else {
          console.error('Could not get the token');
        }
      })
      .catch(function(error) {
        console.error('error:' + error.message);
      });
}

function connectToSignalR(apiUrl)
{
    var connection = new signalR.HubConnectionBuilder().withUrl(apiUrl).build();

    connection.on("ReceiveMessage", function(message){

        var currentUser = document.getElementById("UserName").value;
        var msg = message.messageText.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
        var msgDate = new Date(message.sentDate).toLocaleDateString("en-US", {timeZone: "America/New_York"});
        var li = document.createElement("li");
        li.className = 'right clearfix';

        var image = currentUser == message.userName ? 'http://placehold.it/50/FA6F57/fff&text=ME' : 'http://placehold.it/50/55C1E7/fff&text=U';
        var messageTemplate = 
        `<span class="chat-img pull-left">
            <img src="${image}" alt="User Avatar" class="img-circle" />
        </span>
        <div class="chat-body clearfix">
            <div class="header">
                <strong class="primary-font">${message.userName}</strong> <small class="pull-right text-muted">
                    <span class="glyphicon glyphicon-time"></span>${msgDate}</small>
            </div>
            <p>
                ${msg}
            </p>
        </div>`;

        li.innerHTML  = messageTemplate;

        document.getElementById("chatMessage").appendChild(li);

    });

    connection.start().then(function(){
        console.log("connection stablished");
    }).catch(function (err){
        return console.error(err.toString());
    });

    document.getElementById("btn-chat").addEventListener("click", function(event){
      
        var user = document.getElementById("UserName").value;
        var message = document.getElementById("btn-input").value;

        connection.invoke("SendMessage", user, message).catch(function (err) {
            return console.error(err.toString());
        });

        event.preventDefault();
    });

}

window.onload = getAccessTockenAndConnectToSignalR();
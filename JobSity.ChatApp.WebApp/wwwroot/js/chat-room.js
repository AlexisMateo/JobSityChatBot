"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("http://localhost:5001/chatHub").build();

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
})
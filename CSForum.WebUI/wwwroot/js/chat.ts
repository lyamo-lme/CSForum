"use strict";

import {selectedUserId, addNewMessagesToCurrentChat} from "../ts/chat/fetchUsersChat.js"

//@ts-ignore
var connection = new signalR.HubConnectionBuilder().withUrl("/chat").build();


connection.on("ReceiveMessage", function (message) {
    addNewMessagesToCurrentChat(message, false);
});

connection.start().then(function () {

}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    let receiverId = selectedUserId;
    //@ts-ignore
    let message = document.getElementById("messageInput").value;
    console.log(receiverId);
    console.log(message);
    addNewMessagesToCurrentChat(message, true);
    connection.invoke("SendMessage", receiverId, message);
    event.preventDefault();
});
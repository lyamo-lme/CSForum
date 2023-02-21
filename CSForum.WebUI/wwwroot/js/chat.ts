"use strict";

import {selectedUserId, addNewMessage, Message, userChats} from "../ts/chat/fetchUsersChat.js"

//@ts-ignore
var connection = new signalR.HubConnectionBuilder().withUrl("/chat").build();

connection.on("ReceiveMessage", function (message: Message) {
    console.log(message);  
    addNewMessage(message);
});

connection.start().then(function (data) {
    console.log(connection);
}).catch(function (err) {
    return console.error(err.toString());
});


document.getElementById("sendButton").addEventListener("click", function (event) {
    let receiverId = selectedUserId;
    //@ts-ignore
    let message = document.getElementById("messageInput").value;
    console.log(receiverId);
    console.log(message);
    // userChats.find(x => x.userId == message.userId).chat.messages.push(message);
    // addNewMessage({
    //     id: 0,
    //     content: message,
    //     userId: 0,
    //     chatId: 0
    // } as Message );
    connection.invoke("SendMessage", receiverId, message);
    event.preventDefault();
});
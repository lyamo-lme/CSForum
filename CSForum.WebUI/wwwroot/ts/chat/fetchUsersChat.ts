import {request} from "./fetch.js";
import {createElement} from "../htmlLib/htmlLib.js";

let userChats: UsersChats[];
let selectedUserId = 0;
let listOfUserChatsHtml = document.querySelector("#userChats");
let chatHistory = document.querySelector("#chatHistory");

request(webUrl + "/web/chat/user", "GET").then(async (data) => {
    console.log(data);
    userChats = await data.json();
    console.log(userChats);
    selectedUserId = userChats[0].userId;

    console.log(selectedUserId);
    userChats.map(userChat => {
        let chatElement = createElement("li", "clearfix", "");
        let contentChat = createElement("div", "about", "");
        let nameUser = createElement("div", "name", userChat.user.userName);
        contentChat.appendChild(nameUser);
        chatElement.appendChild(contentChat);
        listOfUserChatsHtml.appendChild(chatElement);
        return userChat;
    });
    Messages(userChats[0]);
});

const Messages = (userChat: UsersChats) => {
    let userId = userChat.userId;
    let listMessages = userChat.chat.messages.map(messages => {
        let elementMessage = createElement("li", "lol",
            messages.userId == userId ? userChat.user.userName : "you");
        chatHistory.appendChild(elementMessage);
        return elementMessage;
    });
}


interface Message {
    id: number,
    chatId: number,
    userId: number,
    content: string,
    created: Date,
    user: User | null,
    chat: Chat | null
}

interface User {
    id: number,
    userName: string,
    email: string,
    messages: Message[] | null,
    chats: Chat[] | null,
    usersChats: UsersChats[] | null,
}

interface Chat {
    chatId: number,
    messages: Message[] | null,
    usersChats: UsersChats[] | null,
    users: User[] | null
}

interface UsersChats {
    id: number,
    chatId: number,
    userId: number,
    user: User | null,
    chat: Chat | null
}
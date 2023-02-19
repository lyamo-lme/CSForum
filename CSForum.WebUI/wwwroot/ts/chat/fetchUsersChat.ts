import {request} from "./fetch.js";
import {createElement} from "../htmlLib/htmlLib.js";

let userChats: UsersChats[];
let listOfUserChatsHtml = document.querySelector("#userChats");

request(webUrl + "/web/chat/user", "GET").then(async (data) => {
    console.log(data);
    userChats = await data.json();
    console.log(userChats);
    userChats.map(userChat => {
        let chatElement = createElement("li", "clearfix", "");
        let contentChat = createElement("div", "about", userChat.user.userName);
        chatElement.appendChild(contentChat);
        listOfUserChatsHtml.appendChild(chatElement);
    });
});


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
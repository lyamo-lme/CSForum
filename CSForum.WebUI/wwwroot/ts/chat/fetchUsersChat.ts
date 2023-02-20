import {request} from "./fetch.js";
import {createElement} from "../htmlLib/htmlLib.js";

export let userChats: UsersChats[];
export let selectedUserId = 0;
export let listOfUserChatsHtml = document.querySelector("#userChats");
export let chatHistory = document.querySelector("#chatHistory");

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

export const Messages = (userChat: UsersChats) => {
    let userId = userChat.userId;
    let listMessages = userChat.chat.messages.map(messages => {
        let elementMessage = createElement(
            "li",
            "lol",
            messages.userId == userId ? userChat.user.userName + ` ${messages.content}` : "you" + ` ${messages.content}`);
        chatHistory.appendChild(elementMessage);
        return elementMessage;
    });
}


export const addNewMessagesToCurrentChat = (message: string, own:boolean) => {
    let messageHtml = createElement("li", "lol", `${own?"you":"he"} ${message}`);
    chatHistory.appendChild(messageHtml);
}

export const ChangeChat = (id: number): void => {
    selectedUserId = id;
}

export interface Message {
    id: number,
    chatId: number,
    userId: number,
    content: string,
    created: Date,
    user: User | null,
    chat: Chat | null
}

export interface User {
    id: number,
    userName: string,
    email: string,
    messages: Message[] | null,
    chats: Chat[] | null,
    usersChats: UsersChats[] | null,
}

export interface Chat {
    chatId: number,
    messages: Message[] | null,
    usersChats: UsersChats[] | null,
    users: User[] | null
}

export interface UsersChats {
    id: number,
    chatId: number,
    userId: number,
    user: User | null,
    chat: Chat | null
}
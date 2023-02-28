import {request} from "./fetch.js";
import {createElement} from "../htmlLib/htmlLib.js";

export let userChats: UsersChats[];
export let selectedUserId = 0;
export let selectedChatId = 0;
export let userId = request(webUrl + "/user/id", "GET").then(data => {
    console.log(data);
});
export let listOfUserChatsHtml = document.querySelector("#userChats");
export let chatHistory = document.querySelector("#chatHistory");

//int chat
request(webUrl + "/web/chat/user", "GET").then(async (data) => {
    userChats = await data.json();
    console.log(userChats);
    if (userChats.length > 0) {
        selectedUserId = userChats[0].userId;
        selectedChatId = userChats[0].chatId;

        userChats.map(userChat => {
            let chatElement = createElement("li", "clearfix", "");
            let contentChat = createElement("div", "about", "");
            let nameUser = createElement("div", "name", userChat.user.userName);

            chatElement.addEventListener("click", () => {
                console.log(userChat.userId, userChat.chatId)
                ChangeChat(userChat.userId, userChat.chatId);
            });
            contentChat.appendChild(nameUser);
            chatElement.appendChild(contentChat);
            listOfUserChatsHtml.appendChild(chatElement);
            return userChat;
        });
        Messages(userChats[0]);
    }
});

export const Messages = (userChat: UsersChats) => {
    // let userId = userChat.userId;
    chatHistory.innerHTML = "";
    let listMessages = userChat.chat.messages.map(messages => {
        let elementMessage = createElement(
            "li",  messages.userId == selectedUserId ? "not-own" : "own", "");
        let messageOwner = createElement("p", "user", messages.userId == selectedUserId ? userChat.user.userName : "you")
        let contentMessaage = createElement("div", "content", messages.content);
        elementMessage.appendChild(messageOwner);
        elementMessage.appendChild(contentMessaage);
        chatHistory.appendChild(elementMessage);
        return elementMessage;
    });
}


export const addNewMessage = (message: Message) => {
    console.log("add message if");
    console.log(selectedUserId);
    console.log(selectedChatId);
    if (selectedChatId == message.chatId) {
        console.log("inside if");
        let elementMessage = createElement(
            "li",
            "lol",
            message.userId == selectedUserId ? "he" + ` ${message.content}` : "you" + ` ${message.content}`);

        // userChats.find(x => x.userId == message.userId).chat.messages.push(message);

        console.log(chatHistory);
        chatHistory.appendChild(elementMessage);
        return;
    } else {
        userChats.find(x => x.userId == message.userId).chat.messages.push(message);
        return;
    }
}

export const ChangeChat = (userId: number, chatId: number): void => {
    selectedUserId = userId;
    selectedChatId = chatId;

    Messages(userChats.find(x => x.chatId == chatId));
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
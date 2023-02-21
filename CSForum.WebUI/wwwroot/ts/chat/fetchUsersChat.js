var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
import { request } from "./fetch.js";
import { createElement } from "../htmlLib/htmlLib.js";
export let userChats;
export let selectedUserId = 0;
export let selectedChatId = 0;
export let userId = request(webUrl + "/user/id", "GET").then(data => {
    console.log(data);
});
export let listOfUserChatsHtml = document.querySelector("#userChats");
export let chatHistory = document.querySelector("#chatHistory");
//int chat
request(webUrl + "/web/chat/user", "GET").then((data) => __awaiter(void 0, void 0, void 0, function* () {
    userChats = yield data.json();
    console.log(userChats);
    if (userChats.length > 0) {
        selectedUserId = userChats[0].userId;
        selectedChatId = userChats[0].chatId;
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
    }
}));
export const Messages = (userChat) => {
    // let userId = userChat.userId;
    let listMessages = userChat.chat.messages.map(messages => {
        let elementMessage = createElement("li", "lol", messages.userId == selectedUserId ? userChat.user.userName + ` ${messages.content}` : "you" + ` ${messages.content}`);
        chatHistory.appendChild(elementMessage);
        return elementMessage;
    });
};
export const addNewMessage = (message) => {
    console.log("add message if");
    console.log(selectedUserId);
    console.log(selectedChatId);
    if (selectedChatId == message.chatId) {
        console.log("inside if");
        let elementMessage = createElement("li", "lol", message.userId == selectedUserId ? "he" + ` ${message.content}` : "you" + ` ${message.content}`);
        // userChats.find(x => x.userId == message.userId).chat.messages.push(message);
        console.log(chatHistory);
        chatHistory.appendChild(elementMessage);
        return;
    }
    else {
        userChats.find(x => x.userId == message.userId).chat.messages.push(message);
        return;
    }
};
export const ChangeChat = (id) => {
    selectedUserId = id;
};
//# sourceMappingURL=fetchUsersChat.js.map
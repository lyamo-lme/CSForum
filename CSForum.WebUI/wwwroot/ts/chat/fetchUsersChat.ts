import {request} from "./fetch.js";

const chats =  request(webUrl+"/web/chat/user", "GET").then(data=>{
    console.log(data);
});
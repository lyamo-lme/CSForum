import {createElement} from "../htmlLib/htmlLib.js";

const tagInput = document.querySelector("#tagInput");
const tagButton = document.querySelector("#tagButton");
const tagList = document.querySelector("#tagsList");
console.log("here tag Find")

tagButton.addEventListener("click", () => {
    // @ts-ignore
    tags(tagInput.value);
});

async function tags(name: string) {
    try {
        tagList.innerHTML = "";
        const response = await fetch(urlOrigin + `/api/tags/${name}`, {
            method: "GET"
        });
        let data = await response.json() as Tags[];
        data.map(tag=>{
            let hrefTag  = createElement("a", "tag", "");
            let tagElement = createElement("div","href",tag.name);
            const response = fetch(urlOrigin + `/api/tags/count/${tag.id}`, {
                method: "GET"
            });
            response.then(async (data)=>{
                console.log();
                // @ts-ignore
                hrefTag.href = `/tag/${tag.id}`;
                let countElement = createElement("p", "count", await data.json()+" posts")
                hrefTag.appendChild(tagElement);
                tagElement.appendChild(countElement);
                tagList.appendChild(hrefTag);
            });
        });
    } catch {

    }
}
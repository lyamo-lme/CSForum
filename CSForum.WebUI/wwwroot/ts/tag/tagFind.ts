import {createElement} from "../htmlLib/htmlLib.js";

const tagInput = document.querySelector("#tagInput");
const tagList = document.querySelector("#tagsList");
console.log("here tag Find")

tagInput.addEventListener("change", () => {
    console.log("here");
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
            let tagElement = createElement("p","tag","");
            let hrefTag  = createElement("a", "href", tag.name);
            const response = fetch(urlOrigin + `/api/tags/count/${tag.id}`, {
                method: "GET"
            });
            response.then(async (data)=>{
                console.log();
                // @ts-ignore
                hrefTag.href = `/tag/${tag.id}`;
                let countElement = createElement("p", "count", await data.json()+" posts")
                tagElement.appendChild(hrefTag);
                tagElement.appendChild(countElement);
                tagList.appendChild(tagElement);
            });
        });
    } catch {

    }
}
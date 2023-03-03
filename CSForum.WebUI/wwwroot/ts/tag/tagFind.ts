import {createElement} from "../htmlLib/htmlLib.js";

const tagInput = document.querySelector("#tagInput");
const tagList = document.querySelector("#tagsList");
console.log("here tag Find")

tagInput.addEventListener("change", () => {
    console.log("here");
    // @ts-ignore
    tags(tagInput.value);
});
2
async function tags(name: string) {
    try {
        const response = await fetch(urlOrigin + `/api/tags/${name}`, {
            method: "GET"
        });
        let data = await response.json() as Tags[];
        data.map(tag=>{
            let tagElement = createElement("p","tag","");
            let hrefTag  = createElement("a", "href", tag.name);
            // @ts-ignore
            hrefTag.href = `/tag/${tag.id}`;
            tagElement.appendChild(hrefTag);
            tagList.appendChild(tagElement);
        });
    } catch {

    }
}
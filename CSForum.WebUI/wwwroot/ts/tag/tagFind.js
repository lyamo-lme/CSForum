var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
import { createElement } from "../htmlLib/htmlLib.js";
const tagInput = document.querySelector("#tagInput");
const tagButton = document.querySelector("#tagButton");
const tagList = document.querySelector("#tagsList");
console.log("here tag Find");
tagButton.addEventListener("click", () => {
    // @ts-ignore
    tags(tagInput.value);
});
function tags(name) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            tagList.innerHTML = "";
            const response = yield fetch(urlOrigin + `/api/tags/${name}`, {
                method: "GET"
            });
            let data = yield response.json();
            data.map(tag => {
                let hrefTag = createElement("a", "tag", "");
                let tagElement = createElement("div", "href", tag.name);
                const response = fetch(urlOrigin + `/api/tags/count/${tag.id}`, {
                    method: "GET"
                });
                response.then((data) => __awaiter(this, void 0, void 0, function* () {
                    console.log();
                    // @ts-ignore
                    hrefTag.href = `/tag/${tag.id}`;
                    let countElement = createElement("p", "count", (yield data.json()) + " posts");
                    hrefTag.appendChild(tagElement);
                    tagElement.appendChild(countElement);
                    tagList.appendChild(hrefTag);
                }));
            });
        }
        catch (_a) {
        }
    });
}
//# sourceMappingURL=tagFind.js.map
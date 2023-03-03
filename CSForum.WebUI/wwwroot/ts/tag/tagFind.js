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
const tagList = document.querySelector("#tagsList");
console.log("here tag Find");
tagInput.addEventListener("change", () => {
    console.log("here");
    // @ts-ignore
    tags(tagInput.value);
});
2;
function tags(name) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const response = yield fetch(urlOrigin + `/api/tags/${name}`, {
                method: "GET"
            });
            let data = yield response.json();
            data.map(tag => {
                let tagElement = createElement("p", "tag", "");
                let hrefTag = createElement("a", "href", tag.name);
                // @ts-ignore
                hrefTag.href = `/tag/${tag.id}`;
                tagElement.appendChild(hrefTag);
                tagList.appendChild(tagElement);
            });
        }
        catch (_a) {
        }
    });
}
//# sourceMappingURL=tagFind.js.map
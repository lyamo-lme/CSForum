var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
const urlOrigin = "https://localhost:5000";
const selectTag = document.querySelector("#selectTag");
const selectedTags = document.querySelector("#selectedTag");
const inputTag = document.querySelector("#tagFind");
let selectedTagIds = [];
console.log("here");
console.log(inputTag);
inputTag.addEventListener("change", () => {
    console.log("here");
    // @ts-ignore
    getTagsByName(inputTag.value);
});
function getTagsByName(name) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const response = yield fetch(urlOrigin + `/api/tags/${name}`, {
                method: "GET"
            });
            let data = yield response.json();
            console.log(data);
            selectTag.innerHTML = "";
            //need to set onchange on select tag
            data.map((tag) => {
                let option = document.createElement("option");
                option.textContent = tag.name;
                option.value = `${tag.id}`;
                selectTag.appendChild(option);
                selectTag.addEventListener("change", () => {
                    // @ts-ignore
                    let id = parseInt(selectTag.value);
                    if (!containValue(selectedTagIds, id)) {
                        selectedTagIds.push(id);
                        let div = document.createElement("div");
                        div.textContent = getNameOption(id);
                        selectedTags.appendChild(div);
                    }
                });
            });
        }
        catch (_a) {
        }
    });
}
const getNameOption = (id) => {
    const options = document.querySelectorAll("select > option");
    for (let i = 0; options.length > i; i++) {
        // @ts-ignore
        if (parseInt(options[i].value) == id) {
            return options[i].textContent;
        }
    }
    return undefined;
};
const containValue = (array, value) => {
    for (let i = 0; array.length > i; i++) {
        if (array[i] == value) {
            return true;
        }
    }
    return false;
};
//# sourceMappingURL=loadTag.js.map
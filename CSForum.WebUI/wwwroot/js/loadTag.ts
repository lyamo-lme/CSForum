const urlOrigin = "https://localhost:5000";
const selectTag = document.querySelector(".selectTag");
const selectedTags = document.querySelector("#selectedTag");
const inputTag = document.querySelector(".tag-find");
let selectedTagIds: number[] = [];


console.log(inputTag);

inputTag.addEventListener("change", () => {
    console.log("here");
    // @ts-ignore
    getTagsByName(inputTag.value);
});

async function getTagsByName(name: string) {
    try {
        const response = await fetch(urlOrigin + `/api/tags/${name}`, {
            method: "GET"
        });
        let data = await response.json() as Tags[];

        console.log(data);

        selectTag.innerHTML = "";
        let option = document.createElement("option");
        option.value = "0";
        option.style.display = "none";
        selectTag.appendChild(option);

        //need to set onchange on select tag
        data.map((tag) => {
            let option = document.createElement("option");
            option.textContent = tag.name;
            option.value = `${tag.id}`;
            selectTag.appendChild(option);
            console.log("here");
            selectTag.addEventListener("change", () => {
                console.log("change here")
                //@ts-ignore
                let id = parseInt(selectTag.value);
                if (id != 0) {
                    if (!containValue(selectedTagIds, id)) {
                        selectedTagIds.push(id);
                        let div = document.createElement("div");
                        div.textContent = getNameOption(id);
                        selectedTags.appendChild(div);
                    }
                }
            });
        })
    } catch {
    }
}

const getNameOption = (id: number) => {
    const options = document.querySelectorAll("select > option");
    for (let i = 0; options.length > i; i++) {
        // @ts-ignore
        if (parseInt(options[i].value) == id) {
            return options[i].textContent;
        }
    }
    return undefined;
}


const containValue = (array: number[], value: number): boolean => {
    for (let i = 0; array.length > i; i++) {
        if (array[i] == value) {
            return true;
        }
    }
    return false;
}

interface Tags {
    id: number,
    name: string
}


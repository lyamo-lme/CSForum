
export const createElement = (tag:string,classList:string,text:string) => {
    let element = document.createElement(tag);
    element.textContent = text;
    element.classList.add(classList);
    return element;
}

export const createElement = (tag, classList, text) => {
    let element = document.createElement(tag);
    element.textContent = text;
    element.classList.add(classList);
    return element;
};
//# sourceMappingURL=htmlLib.js.map
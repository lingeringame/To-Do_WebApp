let checkboxArr = document.querySelectorAll("input[type=checkbox]");
let todoTextArr = document.querySelectorAll(".todoText");

checkboxArr.forEach(checkbox => checkbox.addEventListener("change", function () {
    let textToStrikethrough = findMatchingInputs(checkbox, todoTextArr);
    if (checkbox.checked) {
        for (item of textToStrikethrough) {
            item.style.textDecoration = "line-through";
            item.style.color = "gray";
            item.style.backgroundColor = "rgb(250,250,250)";
        }
    } else {
        for (item of textToStrikethrough) {
            item.style.textDecoration = "none";
            item.style.color = "black";
            item.style.backgroundColor = "white";
        }
    }
}))

function findMatchingInputs(currCheckbox, textArr) {
    let matchingTexts = [];
    for (let i = 0; i < textArr.length; i++) {
        if (currCheckbox.dataset.row === textArr[i].dataset.row)
            matchingTexts.push(textArr[i])
    }
    return matchingTexts;
}
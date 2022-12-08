//Gets checkboxes and their corresponding rows
let checkboxArr = document.querySelectorAll("input[type=checkbox]");
let todoTextArr = document.querySelectorAll(".todoText");

//Check if each checkbox is checked so that we can cross out their rows if so
checkboxArr.forEach(checkbox => checkbox.addEventListener("change", function () {
    let textToStrikethrough = findMatchingInputs(checkbox, todoTextArr);
    if (checkbox.checked) {
        for (item of textToStrikethrough) {
            item.style.textDecoration = "line-through";
            item.style.color = "gray";
            item.style.backgroundColor = "rgb(200,200,200)";
        }
    } else {
        for (item of textToStrikethrough) {
            item.style.textDecoration = "none";
            item.style.color = "black";
            item.style.backgroundColor = "white";
        }
    }
}))

//Function to get matching row for a checkbox
function findMatchingInputs(currCheckbox, textArr) {
    let matchingTexts = [];
    for (let i = 0; i < textArr.length; i++) {
        if (currCheckbox.dataset.row === textArr[i].dataset.row)
            matchingTexts.push(textArr[i])
    }
    return matchingTexts;
}

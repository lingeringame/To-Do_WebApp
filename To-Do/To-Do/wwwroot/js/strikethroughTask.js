//Gets checkboxes and their corresponding rows
let checkboxArr = document.querySelectorAll("input[type=checkbox]");
let todoTextArr = document.querySelectorAll(".todoText");

//Check if each checkbox is checked so that we can cross out their rows if so
checkboxArr.forEach(checkbox => checkbox.addEventListener("change", function () {
    let textToStrikethrough = findMatchingInputs(checkbox, todoTextArr);
    if (checkbox.checked) {
        for (item of textToStrikethrough) {
            item.classList.add("strikethrough")
        }
    } else {
        for (item of textToStrikethrough) {
            item.classList.remove("strikethrough")
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

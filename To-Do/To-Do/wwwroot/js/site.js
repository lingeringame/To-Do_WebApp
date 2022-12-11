
var availableTasks = document.getElementsByClassName("theTasks");
var taskDescriptions = document.getElementsByClassName("taskDesc");

Array.from(availableTasks).forEach(task => task.addEventListener("click", function () {

    for (desc of taskDescriptions) {
        if (desc.style.display == "block") {
            desc.style.display = "none";
        }
    }
    let taskId = task.dataset.row;
    let matchingDesc = Array.from(taskDescriptions).find((node) => node.dataset.row === taskId);
    matchingDesc.style.display = "block";
}));

var titles = document.querySelectorAll(".taskTitle");
var previousClickedTitle = null;
titles.forEach(title => title.addEventListener("click", function () {
    if (previousClickedTitle === null) {
        previousClickedTitle = title;
        previousClickedTitle.classList.add("todoTitleClicked");
    }
    if (title !== previousClickedTitle) {
        previousClickedTitle.classList.remove("todoTitleClicked")
    }
    previousClickedTitle = title;
    title.classList.add("todoTitleClicked");
}));
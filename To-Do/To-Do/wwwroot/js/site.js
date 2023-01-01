//when a task is clicked, the contents will be shown to the left of the list
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

//when a task is clicked, the title of the selected task will be purple, then it will change depending on which task is clicked, resetting the previous title's color
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

//var tasks = document.querySelectorAll(".theTasks");
//when a task is hovered on, the edit and delete functions will appear. They are hidden by default and will be hidden once the user leaves the row
Array.from(availableTasks).forEach(task => task.addEventListener("mouseover", function () {
    let taskChildNodes = task.childNodes;
    let links = Array.from(taskChildNodes).filter((node) => node.className === "EditDeleteLinks")
    console.log(links)
    for (link of links) {
        link.childNodes[1].style.visibility = "visible"; 
    }
}))

Array.from(availableTasks).forEach(task => task.addEventListener("mouseout", function () {
    let taskChildNodes = task.childNodes;
    let links = Array.from(taskChildNodes).filter((node) => node.className === "EditDeleteLinks")
    for (link of links) {
        link.childNodes[1].style.visibility = "hidden";
    }
}))

//selects/deselects all checkboxes in BulkDelete View
document.querySelector("#selectBtn").addEventListener("click", function () {
    let checkboxes = document.querySelectorAll('input[type="checkbox"]');
    checkboxes.forEach(cb => cb.checked = true);
});

document.querySelector("#deselectBtn").addEventListener("click", function () {
    let checkboxes = document.querySelectorAll('input[type="checkbox"]');
    checkboxes.forEach(cb => cb.checked = false);
})
$('a.history-link').on('click', (e) => e.stopPropagation());

function hideEmptyDays() {
    var days: HTMLCollection = document.getElementsByClassName('category-change-day');
    for (let i = 0; i < days.length; i++) {
        var currentElement = days[i];
        if (currentElement.children.length <= 1)
            currentElement.classList.add('hide-row');
    }
}
hideEmptyDays();

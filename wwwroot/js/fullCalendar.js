document.addEventListener('DOMContentLoaded', function () {
    const calendarEl = document.getElementById('calendar');

    function initializeCalendar() {
        const calendar = new FullCalendar.Calendar(calendarEl, {
            initialView: 'dayGridWeek',
            height: '50vh'
        });

        calendar.render();
    }

    function checkContainerSize() {
        // If the calendar's width is 0, wait until the next frame to try again
        if (calendarEl.offsetWidth === 0) {
            window.requestAnimationFrame(checkContainerSize);
        } else {
            initializeCalendar();
        }
    }

    // Call the function to check container size and initialize the calendar
    checkContainerSize();

    // Optionally, add a resize event listener to handle window resizing
    window.addEventListener('resize', function () {
        if (calendar) {
            calendar.updateSize();
        }
    });
});
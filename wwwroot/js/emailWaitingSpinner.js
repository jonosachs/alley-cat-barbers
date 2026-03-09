document.addEventListener('DOMContentLoaded', function () {
    var form = document.querySelector('form[data-email-sent]');

    form.addEventListener('submit', function (event) {
        
        // Show spinner and hide content
        document.getElementById('spinner').style.display = 'block';
        document.getElementById('main-content').style.display = 'none';

    });
});


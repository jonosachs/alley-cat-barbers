document.addEventListener("DOMContentLoaded", function () {

    var userTable = document.getElementById('userTable');
    var reviewTable = document.getElementById('reviewTable');
  

    if (userTable) {
        $(window).on('load', function () {
            var userTable = $('#userTable').DataTable({
                responsive: true,
                paging: true,
                searching: true,
                info: true,
                autoWidth: false,
                
            });

            userTable.columns.adjust().responsive.recalc(); // Trigger size recalc to adjust to window
        });
    }

    if (reviewTable) {
        $(window).on('load', function () {
            var userTable = $('#reviewTable').DataTable({
                responsive: true,
                paging: true,
                searching: true,
                info: true,
                autoWidth: false,
            });

            userTable.columns.adjust().responsive.recalc();
        });

    }



});

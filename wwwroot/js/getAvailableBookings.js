$(function () {
    $("#bookingDate").datepicker({
        dateFormat: 'yy-mm-dd',
        onSelect: function (dateText) {
            fetchAvailableSlots(dateText);
        }
    });

    function fetchAvailableSlots(date) {
        $.ajax({
            url: '/Bookings/GetAvailableSlots',
            data: { date: date },
            success: function (slots) {
                $('#timeSlot').empty();
                $('#timeSlot').append('<option value="">Select a time slot</option>');
                slots.forEach(function (slot) {
                    $('#timeSlot').append('<option value="' + slot + '">' + slot + '</option>');
                });
            }
        });
    }
});
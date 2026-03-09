// Set actions for Booking fields
    var serviceIdDropdown = $('#serviceId');
    var datePicker = $('#bookingDate');
    var timeSlotDropdown = $('#timeSlot');
    var formMode = $('#editBookingForm').data('mode');

    // Check if editing an existing booking
    if (formMode === 'edit') {
        console.log("Edit mode");

        // Get existing booking date and time values from hidden fields
        var hiddenDate = $('#hiddenDate').val();
        var hiddenTimeSlot = $('#hiddenTimeSlot').val();

        if (hiddenDate) {
            datePicker.val(hiddenDate);
            datePicker.datepicker("setDate", hiddenDate);  
        }

        if (hiddenTimeSlot) {
            timeSlotDropdown.val(hiddenTimeSlot);
            fetchTimeSlots(hiddenDate)
  
        }

        datePicker.prop('disabled', false);
        timeSlotDropdown.prop('disabled', false);
    } else {    // Otherwise enable selections sequentially for new booking
        datePicker.val('Choose..');
        datePicker.prop('disabled', true);
        timeSlotDropdown.prop('disabled', true);
    }

    // Enable date picker after service selection
    serviceIdDropdown.change(function () {

        var selectedService = $(this).val();

        if (selectedService) {
            datePicker.prop('disabled', false);
        } else {
            datePicker.prop('disabled', true);
        }

    });

    // Set date picker restrictions
    datePicker.datepicker({
        dateFormat: 'yy-mm-dd',
        minDate: 0, 
        maxDate: '+30D',
        beforeShowDay: function (date) {
            var today = new Date();
            today.setHours(0, 0, 0, 0);
            var maxDate = new Date();
            maxDate.setDate(today.getDate() + 30);

            if (date < today || date > maxDate) {
                return [false, "ui-state-disabled", "Unavailable"];
            } else {
                return [true, "", ""];
            }
        },

        // Get available time slots after date selected
        onSelect: function (dateText) {
            fetchTimeSlots(dateText);
        }
    });

    // Get available time slots
    function fetchTimeSlots(dateText) {

        $.ajax({
            url: '/Bookings/GetAvailableTimeSlots', // Booking controller method
            type: 'GET',
            data: { date: dateText, currentBookingTimeSlot: hiddenTimeSlot },
            success: function (data) {
                    
                timeSlotDropdown.empty();
                timeSlotDropdown.append('<option value="">Choose..</option>'); 

                // Populate time dropdown with available times
                $.each(data, function (index, value) {
                    timeSlotDropdown.append('<option value="' + value + '">' + value + '</option>');
                });

                if (formMode === 'edit') {
                    timeSlotDropdown.val(hiddenTimeSlot); // Set to existing booking time if editing
                }

                timeSlotDropdown.prop('disabled', false); // Enable dropdown after populating
            },
            error: function (xhr, status, error) {
                console.error("Error fetching time slots: ", error);
                timeSlotDropdown.prop('disabled', true); // Keep disabled if error
            }
        });
    }




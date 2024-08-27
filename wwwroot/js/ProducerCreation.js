$(document).ready(function () {
    $('#createProducerForm').on('submit', function (event) {
        event.preventDefault();

        var formData = new FormData(this);

        $.ajax({
            url: $(this).attr('action'), // Use the form's action URL
            type: 'POST',
            data: formData,
            contentType: false,
            processData: false,
            success: function (response) {
                // Redirect to the index page on successful creation
                window.location.href = '/Producers/Index';
            },
            error: function (response) {
                var errors = response.responseJSON.errors;
                var errorSummary = $('#validationSummary');
                errorSummary.html("");
                errorSummary.show();
                for (var key in errors) {
                    errorSummary.append("<p>" + errors[key] + "</p>");
                }
            }
        });
    });
});
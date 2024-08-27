$(document).ready(function () {
    $('#createActorForm').on('submit', function (event) {
        event.PreventDefault();

        var formData = new FormData(this);

        $.ajax({
            type: 'POST',
            data: 'formData',
            contentType: false,
            processData: false,


            success: function (response) {
                window.location.href = '/Actors/Index';
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
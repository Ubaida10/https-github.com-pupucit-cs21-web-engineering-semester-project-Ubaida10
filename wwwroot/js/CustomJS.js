$(document).ready(function () {
    $('#search-form').on('submit', function (event) {
        event.preventDefault();           //Prevent default form submission

        let searchString = $('input[name="searchString"]').val();         // Get the search input value

        $.ajax({
            url: '/Movies/Search',            // URL to send the request to
            method: 'GET',           
            data: { searchString: searchString },     // Data to send to the server
            success: function (response) {
                $('#search-results').html(response)    // Update the search results container with the response
            },

            error: function () {
                $('#search-results').html('<p>An error occurred while processing your request.</p>'); // Handle errors
            }
        });
    });
});
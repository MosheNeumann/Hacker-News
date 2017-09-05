$(function () {
    console.log('in js file');

    $('#confirm-password').keyup(validatePasswords);
    $('#password').keyup(validatePasswords);

    function validatePasswords() {
        var password = $('#password').val();
        var confirmedPassword = $('#confirm-password').val();

        if (password != confirmedPassword) {
            $('.btn').prop("disabled", true);
            $('#label').text('Passwords do not match');
        }
        else {
            $('.btn').prop("disabled", false);
            $('#label').text('');
        }
    }


});
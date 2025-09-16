$(document).ready(function () {
    $('#imagen').change(function () {
        var file = this.files[0];
        if (file) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $('#preview').attr('src', e.target.result).show();
            }
            reader.readAsDataURL(file);
        } else {
            $('#preview').attr('src', '#').hide();
        }
    });
});
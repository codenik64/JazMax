$(document).ready(function () {
    $('.progressWrapper progress').each(function () {
        var prgsVal = $(this).data('value');
        var maxN = $(this).attr('max');
        var pop = prgsVal / maxN * 100

        $(this).prev().css('left', pop + '%').text(prgsVal);
        $(this).val(prgsVal);
    });
});
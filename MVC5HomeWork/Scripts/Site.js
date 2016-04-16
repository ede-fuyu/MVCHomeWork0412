function ResetForm(formName) {
    var form = "#" + formName;
    $('.formError').remove();
    $(form)[0].reset();
    $(form).find('select').selectpicker('render');
}
var fnBlaBla = function () {
    $("#EditCategoryForm").validate({
            rules: {
                Description: {
                    required: true,
                    email: true
                }
            },
            //onfocusout: false
        });
    $("[name='Name']").validate({
        //onfocusout: true
    });
};



//<span class="help-block">Ups, keine gültige Kategorie ausgewählt. Bitte suchen und aus der Liste auswählen oder <a>Kategorie in neuem Tab anlegen</a>.</span>

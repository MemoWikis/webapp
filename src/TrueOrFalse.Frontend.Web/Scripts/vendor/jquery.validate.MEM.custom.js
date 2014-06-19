/* Translated default messages for the jQuery validation plugin.
 * Locale: DE (German, Deutsch)
 * Customized for MEMuchO
 */




(function($) {
	$.extend($.validator.messages, {
    required: "Hoppla, das ist ein Pflichtfeld.",
    maxlength: $.validator.format("Gib bitte maximal {0} Zeichen ein."),
    minlength: $.validator.format("Gib bitte mindestens {0} Zeichen ein."),
    rangelength: $.validator.format("Gib bitte mindestens {0} und maximal {1} Zeichen ein."),
    email: "Gib bitte eine gültige E-Mail Adresse ein.",
    url: "Gib bitte eine gültige URL ein.",
    date: "Bitte gib ein gültiges Datum ein.",
    number: "Gib bitte eine Zahl ein.",
    digits: "Verwende bitte nur Ziffern.",
    equalTo: "Ups, der Inhalt der beiden Felder stimmt nicht überein.",
    range: $.validator.format("Gib bitte einen Wert zwischen {0} und {1} ein."),
    max: $.validator.format("Gib bitte einen Wert kleiner oder gleich {0} ein."),
    min: $.validator.format("Gib bitte einen Wert größer oder gleich {0} ein."),
    creditcard: "Gib bitte eine gültige Kreditkarten-Nummer an."
});
}(jQuery));
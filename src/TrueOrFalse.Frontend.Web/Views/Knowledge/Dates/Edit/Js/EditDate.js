Vue.component('user-message', {
    props: ['text'],
    //template: "<div style='background-color:green;'>{{text}}</div>"
    template: "<div class=\"fade in\">\n            <a class=\"close\" data-dismiss=\"alert\" href=\"#\">\u00D7</a>\n            {{text}}\n        </div>"
});
var safeDates = new Vue({
    el: '#vueSafeDates',
    data: {
        isEditing: false,
        title: "Termin erstellen",
        userMessage: "d3",
        name: "",
        date: null,
    },
    methods: {
        save: function () {
            //$.ajax({
            //    url: "/EditDate/CreateNew",
            //    type: "POST",
            //    data: {
            //        //'setIdsArray': sets,
            //        //'timeVar': timeVar,
            //        'dateVar': this.date,
            //        'nameOfDate': this.name
            //    },
            //    dataType: "json",
            //    success: function (result) {
            //        this.userMessage = "hurray gespeichert";
            //    }
            //});
            this.userMessage = "hurray gespeichert";
        },
        setDate: function () {
            event.preventDefault();
            var nameOfDate = $("#Details").val();
            var dateVar = $("#Date").val();
            console.log(dateVar);
            var timeVar = $("#Time").val();
            var sets = $('.JS-Sets input').map(function () {
                return $(this).val();
            }).toArray();
            console.log(sets);
            $.ajax({
                url: "/EditDate/CreateNew",
                type: "POST",
                data: {
                    'setIdsArray': sets,
                    'timeVar': timeVar,
                    'dateVar': dateVar,
                    'nameOfDate': nameOfDate
                },
                dataType: "json",
                success: function (result) {
                    $("#test").html(result);
                }
            });
            var network = $("[name=Visibility]").val();
        }
    }
});
//# sourceMappingURL=EditDate.js.map
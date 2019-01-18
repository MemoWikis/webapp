declare var Vue: any;


Vue.component('user-message', {
    props: ['text'],
    template: 
        `<div class="fade in">
            <a class="close" data-dismiss="alert" href="#">×</a>
            {{text}}
        </div>` 
});

var safeDates = new Vue({
    el: '#vueSafeDates',

    data: {
        isEditing: false,
        title: "Termin erstellen", //bearbeiten
        userMessage: "d3",
        name: "",
        date: null,
    },

    methods: {

        save: function(){
            this.userMessage = "hurray gespeichert";
        },

        setDate: function () {
            event.preventDefault();
            let nameOfDate = $("#Details").val();
            let dateVar = $("#Date").val();
            console.log(dateVar);
            let timeVar = $("#Time").val();
            let sets = $('.JS-Sets input').map(function () {
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
            let network = $("[name=Visibility]").val();
            
        }
    }
});
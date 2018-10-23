var safeDates = new Vue({
    el: '#safeDates',
    data: {
        message: 'Hello Vue!'
    },
    methods: {
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
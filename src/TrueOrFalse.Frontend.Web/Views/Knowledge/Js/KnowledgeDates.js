var addDates = new Vue({
    el: '#addDates',
    data: {
        message: 'Hello Vue!'
    },
    methods: {
        getEditDate: function() {
            $.ajax({
                url: "/EditDate/Create",
                dataType: "text",
                type: "POST",
                success: function(result) {
                    $(".content").html(result);
                }
            });
        }
    }
})
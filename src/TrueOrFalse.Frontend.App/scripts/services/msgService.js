app.factory('$msgService', function ($localstorage) {
    return {
        deleteAll : function() {
            $localstorage.setMessages([]);
        },
        getAll: function () {
            return $localstorage.getMessages();
        },
        add: function(msg) {
            var msgs = $localstorage.getMessages();
            msgs.push(msg);
            $localstorage.setMessages(msgs);
        }
    }
});
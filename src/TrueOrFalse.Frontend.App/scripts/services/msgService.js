app.factory('$msgService', function ($rootScope, $localstorage) {
    return {
        deleteAll : function() {
            $localstorage.setMessages([]);
            $rootScope.msgs = [];
        },
        getAll: function () {
            return $localstorage.getMessages();
        },
        add: function(msg) {
            var msgs = $localstorage.getMessages();
            msgs.push(msg);
            $localstorage.setMessages(msgs);

            $rootScope.msgs = msgs;
        }
    }
});
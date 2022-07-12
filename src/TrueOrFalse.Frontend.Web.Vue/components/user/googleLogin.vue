<script setup lang="ts">
import { Google } from './Google'
import { onMounted } from 'vue'

async function init() {
    window.fbAsyncInit = () => {
        FB.init({
            appId: '1789061994647406',
            cookie: true,  // enable cookies to allow the server to access 
            // the session
            xfbml: true,  // parse social plugins on this page
            version: 'v2.8' // use graph api version 2.8
        });

        init();
    };

    // Load the Facebook-SDK asynchronously
    (function (d, s, id) {
        var js, fjs = d.getElementsByTagName(s)[0];
        if (d.getElementById(id)) return;
        js = d.createElement(s); js.id = id;
        js.src = "//connect.facebook.net/en_US/sdk.js";
        fjs.parentNode.insertBefore(js, fjs);
    }(document, 'script', 'facebook-jssdk'));

    login()
}

function login() {
}

onMounted(async () => {

    await Google.AttachClickHandler('GoogleLogin')
})
defineExpose({
    login,
})

</script>

<template>
</template>
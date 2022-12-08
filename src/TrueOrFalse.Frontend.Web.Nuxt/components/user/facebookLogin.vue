<script setup lang="ts">
import { FacebookMemuchoUser } from './FacebookMemuchoUser'

function login() {
    FacebookMemuchoUser.LoginOrRegister(/*stayOnPage*/true, /*dissalowRegistration*/ false)
}

function loadPlugin(toLogin = false) {
    const fbScriptElement = document.getElementById('facebook-jssdk')
    if (fbScriptElement == null) {

        window.fbAsyncInit = function () {
            FB.init({
                appId: '1789061994647406',
                xfbml: true,
                version: 'v2.8'
            })
        };

        (function (d, s, id) {
            var js, fjs = d.getElementsByTagName(s)[0]
            if (d.getElementById(id)) { return }
            js = d.createElement(s) as HTMLScriptElement
            js.id = id
            js.src = "//connect.facebook.net/de_DE/sdk.js"
            fjs.parentNode?.insertBefore(js, fjs)
        }(document, 'script', 'facebook-jssdk'))

        if (toLogin) {
            setTimeout(() => {
                FacebookMemuchoUser.LoginOrRegister(/*stayOnPage*/true, /*dissalowRegistration*/ false)
            }, 500);
        }
    }
}

defineExpose({
    loadPlugin,
    login
})

</script>

<template>
</template>
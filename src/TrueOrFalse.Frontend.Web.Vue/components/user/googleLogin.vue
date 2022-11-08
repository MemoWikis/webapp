<script setup lang="ts">
import { Google } from './Google'

function loadGooglePlugin(loadLogin = false) {
    console.log(window.gapi)
    const gapiScript = document.createElement('script')
    gapiScript.src = 'https://apis.google.com/js/api:client.js'
    gapiScript.onload = () => {
        const jsApi = document.createElement('script')
        jsApi.onload = () => {
            var g = new Google()

            setTimeout(() => {
                if (loadLogin)
                    login()
            }, 500)
        }
        jsApi.src = 'https://www.google.com/jsapi'
        document.head.appendChild(jsApi)
    }
    document.head.appendChild(gapiScript)
}

function login() {
    Google.SignIn()
    console.log(window.gapi)
}

</script>

<template>
</template>
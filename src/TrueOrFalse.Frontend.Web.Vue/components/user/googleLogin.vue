<script setup lang="ts">
import { Google } from './Google'

function loadPlugin(toLogin = false) {
    const gapiClientElement = document.getElementById('gapiClient')

    if (gapiClientElement == null) {
        const gapiScript = document.createElement('script')
        gapiScript.setAttribute('id', 'gapiClient')
        gapiScript.src = 'https://apis.google.com/js/api:client.js'
        gapiScript.onload = () => {
            loadGapiLoader(toLogin)
        }
        document.head.appendChild(gapiScript)
    } else if (toLogin)
        loadGapiLoader(toLogin)

}

function loadGapiLoader(toLogin) {
    const gapiLoaderElement = document.getElementById('gapiLoader')

    if (gapiLoaderElement == null) {
        const jsApi = document.createElement('script')
        jsApi.setAttribute('id', 'gapiLoader')
        jsApi.onload = () => {
            var g = new Google()

            setTimeout(() => {
                if (toLogin)
                    login()
            }, 500)
        }
        jsApi.src = 'https://www.google.com/jsapi'
        document.head.appendChild(jsApi)
    } else if (toLogin)
        login()
}

function login() {
    Google.SignIn()
}

defineExpose({
    loadPlugin,
    login
})
</script>

<template>
</template>
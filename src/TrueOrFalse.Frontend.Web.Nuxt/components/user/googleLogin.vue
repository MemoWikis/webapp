

<script setup lang="ts">
import { Google } from './Google'

function loadPlugin(toLogin = false) {
    Google.loadGsiClient(toLogin)
}

function login() {
    Google.SignIn()
}

defineExpose({
    loadPlugin,
    login
})
</script>

<template></template>
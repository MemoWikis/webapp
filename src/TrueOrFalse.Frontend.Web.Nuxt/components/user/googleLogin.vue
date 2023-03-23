

<script setup lang="ts">
import { Google } from './Google'

function loadPlugin() {
    Google.loadGsiClient()
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
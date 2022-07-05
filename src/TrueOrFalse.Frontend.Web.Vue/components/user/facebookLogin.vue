<script setup lang="ts">
import { FacebookMemuchoUser } from './FacebookMemuchoUser'
import { onMounted } from 'vue'

function login() {
    FacebookMemuchoUser.LoginOrRegister(/*stayOnPage*/true, /*dissalowRegistration*/ false)
}
onMounted(() => login())

</script>

<template>
</template>
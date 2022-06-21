<script setup lang="ts">
import {ref} from 'vue'

const email = ref('')
const pw = ref('')
const config = useRuntimeConfig()

function login() {

    var data = {
        EmailAddress: email.value,
        Password: pw.value,
        PersistentLogin: ""
        }

    fetch(config.apiBase + '/VueLogin/Login', { method: 'POST', body: JSON.stringify(data) })
}

</script>

<template>
    <div>
        <input v-model="email" placeholder="email">
        <input v-model="pw" placeholder="pw" type="password">

        <div id="lbtn" @click="login()">
            Login
        </div>
    </div>

</template>

<style>
#lbtn {
    padding: 10px;
    cursor: pointer;
    background: lightskyblue;
    color: white;
    font-size: larger;
    font-weight: 600;
}
</style>
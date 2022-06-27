<script setup lang="ts">
import {ref} from 'vue'

const email = ref('')
const pw = ref('')
const config = useRuntimeConfig()

async function login() {

    var data = {
        EmailAddress: email.value,
        Password: pw.value,
        PersistentLogin: true
        }

    await $fetch('/api/Login/Login', { method: 'POST', body: data, mode: 'cors',
        headers: {
            'Access-Control-Allow-Origin':'*'
        }
    //   async onResponse({ request, response, options }) {
    //     // Log response
    //     console.log('[fetch response]', request, response.status, response.body, response.headers)
    // } 
  }).catch((error) => console.log(error.data))

//   fetch(config.apiBase + 'Login/Login', { method: 'POST', body: JSON.stringify(data) })        
}

function getLoginState() {
    var result = fetch(config.apiBase + '/SessionUser/GetLoginState')
    console.log(result)
}

</script>

<template>
    <div>   
        <input v-model="email" placeholder="email">
        <input v-model="pw" placeholder="pw" type="password">

        <div id="lbtn" @click="login()">
            Login
        </div>

        <br/>

        <div id="lbtn" @click="getLoginState()">
            getLoginState
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
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

    await $fetch('/api/Login/Login', { method: 'POST', body: data, mode: 'cors', credentials: 'include'
  }).catch((error) => console.log(error.data))
}

async function getLoginState() {

    var data = {
        EmailAddress: email.value,
        Password: pw.value,
        PersistentLogin: true
        }

    var result = await $fetch('/api/SessionUser/GetLoginState', { mode: 'cors', credentials: 'include'
  }).catch((error) => console.log(error.data))

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
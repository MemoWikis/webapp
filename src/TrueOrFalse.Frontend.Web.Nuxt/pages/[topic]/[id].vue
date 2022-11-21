<script setup lang="ts">
import { useSpinnerStore } from '~~/components/spinner/spinnerStore'
const spinnerStore = useSpinnerStore()
const route = useRoute()
const { data: topic } = await useFetch<any>(`/apiVue/Topic/GetTopic/${route.params.id}`,
    {
        baseURL: process.client ? 'http://memucho.local:3000' : 'http://memucho.local',
        credentials: 'include',
        mode: 'no-cors',
        server: true,
    })

function refreshPage() {
    var randomTopic = Math.floor(Math.random() * 1000)
    navigateTo({ path: `/random/${randomTopic}`, replace: true })
}

</script>

<template>
    <div>
        <div v-if="topic">{{ topic.Id }}</div>
        <div @click="refreshPage()" class="button">NewTopic</div>
        <button @click="spinnerStore.showSpinner()">ShowSpinner</button>
        <button @click="spinnerStore.hideSpinner()">hideSpinner</button>

        <Topic />
    </div>
</template>

<style scoped>
.button {
    background: darkcyan;
    color: whitesmoke;
    font-size: 14px;
    font-weight: 600;
    padding: 20px;
    border: solid 2px grey;
}
</style>
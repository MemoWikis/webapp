<script lang="ts" setup>
const route = useRoute()
const { data: topic } = await useFetch<any>(`/apiVue/Topic/GetTopic/${route.params.id}`,
    {
        baseURL: process.client ? 'http://memucho.local:3000/' : 'http://memucho.local/',
        credentials: 'include',
        mode: 'no-cors',
        server: true,
    })

</script>

<template>

    <div v-if="topic">
        <div> TopicComponent: {{ topic.Id }}</div>
    </div>
</template>
<script lang="ts" setup>
import Tabs from './tabs.vue';
const config = useRuntimeConfig();

const { data: topic, pending, refresh, error } =
     await useFetch(() => `Topic/21`, { baseURL: config.apiBase });

</script>

<template>
    <div>
        <Tabs />
        {{topic}}
    </div>
</template>

<style scoped></style>
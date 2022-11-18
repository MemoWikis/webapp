<script lang="ts" setup>
import { ImageStyle } from '../image/imageStyleEnum';
import { Author } from './author'
const props = defineProps(['id'])

const data = {
    id: props.id
}
const author = ref(null as Author)
// if (process.client) {
author.value = await $fetch<Author>('/api/Author/GetAuthor/', { method: 'POST', body: data, mode: 'cors', credentials: 'include' })
// } else if (process.server) {
//     const config = useRuntimeConfig()
//     author.value = await $fetch<Author>('/Author/GetAuthor/', { method: 'POST', baseURL: config.apiBase, body: data, mode: 'cors', credentials: 'include' })
// }
</script>

<template>
    <LazyNuxtLink :to="`/Nutzer/${author.Name}/${props.id}`" v-tooltip="author.Name">
        <Image :src="author.ImageUrl" :style="ImageStyle.Author" class="header-author-icon" />
    </LazyNuxtLink>
</template>

<style lang="less" scoped>
.header-author-icon {
    height: 20px;
    width: 20px;
    min-height: 20px;
    min-width: 20px;
    margin: 0 4px;
}
</style>
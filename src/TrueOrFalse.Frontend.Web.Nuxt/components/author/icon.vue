<script lang="ts" setup>
import { ImageStyle } from '../image/imageStyleEnum';
import { Author } from './author'
const props = defineProps(['id'])

const data = {
    id: props.id
}

const { data: author } = await useFetch<Author>(`/apiVue/Author/GetAuthor/`,
    {
        baseURL: process.client ? 'http://memucho.local:3000' : 'http://memucho.local',
        method: 'POST',
        body: data,
        credentials: 'include',
        mode: 'no-cors',
        server: true,
    })
</script>

<template>
    <LazyNuxtLink :to="`http://memucho.local/Nutzer/${author.Name}/${props.id}`" v-tooltip="author.Name" v-if="author">
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
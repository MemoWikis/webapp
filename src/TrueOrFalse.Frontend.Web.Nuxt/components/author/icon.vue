<script lang="ts" setup>
import { ImageStyle } from '../image/imageStyleEnum';
import { Author } from './author'
const props = defineProps(['id'])

const data = {
    id: props.id
}
const author = ref(null as unknown as Author)
const result = await $fetch<Author>(`/apiVue/Author/GetAuthor/`,
    {
        method: 'POST',
        body: data,
        credentials: 'include',
        mode: 'no-cors',
        onRequest({ request }) {
            console.log("authorRequest---" + request)
        }
    })
author.value = result
</script>

<template>
    <LazyNuxtLink v-if="author" :to="`/Nutzer/${author.Name}/${props.id}`" v-tooltip="author.Name">
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
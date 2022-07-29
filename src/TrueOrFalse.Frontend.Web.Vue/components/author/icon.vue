<script lang="ts" setup>
import { ImageStyle } from '../image/imageStyleEnum';
const props = defineProps(['id'])

class Author {
    ImageUrl: string
    Reputation: number
    Name: string
    ReputationPos: number
}
const data = {
    id: props.id
}
const config = useRuntimeConfig()
const author = ref(null as Author)

if (process.client) {
    author.value = await $fetch<Author>('/api/Author/GetAuthor/', { method: 'POST', body: data, mode: 'cors', credentials: 'include' })
} else if (process.server) {
    author.value = await $fetch<Author>('/Author/GetAuthor/', { method: 'POST', baseURL: config.apiBase, body: data, mode: 'cors', credentials: 'include' })
}

</script>

<template>
    <div v-tooltip="author.Name">
        <NuxtLink :to="`/Nutzer/${author.Name}/${props.id}`">
            <Image :src="author.ImageUrl" :style="ImageStyle.Author" class="header-author-icon" />
        </NuxtLink>
    </div>
</template>

<style lang="less" scoped>
.header-author-icon {
    width: 20px;
    height: 20px;
    margin: 0 4px;
}
</style>
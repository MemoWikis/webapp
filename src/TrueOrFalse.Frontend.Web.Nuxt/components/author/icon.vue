<script lang="ts" setup>
import { ImageStyle } from '../image/imageStyleEnum';
import { Author } from './author'
const props = defineProps({
    author: Object as () => Author,
})

</script>

<template>
    <LazyNuxtLink :to="`/Nutzer/${author.Name}/${author.Id}`" v-tooltip="author.Name">
        <Image :src="author.ImgUrl" :style="ImageStyle.Author" class="header-author-icon" />
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
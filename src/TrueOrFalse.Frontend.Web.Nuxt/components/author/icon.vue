<script lang="ts" setup>
import { ImageStyle } from '../image/imageStyleEnum';
import { Author } from './author'
interface Props {
    author: Author
}
const props = defineProps<Props>()

</script>

<template>
    <LazyNuxtLink :to="`/Nutzer/${props.author.Name}/${props.author.Id}`" v-tooltip="props.author.Name">
        <Image :url="props.author.ImgUrl" :style="ImageStyle.Author" class="header-author-icon" />
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
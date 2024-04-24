<script lang="ts" setup>
import { ImageFormat } from '../image/imageFormatEnum'
import { Author } from './author'
interface Props {
    author: Author
}
const props = defineProps<Props>()

</script>

<template>
    <LazyNuxtLink :to="`/Nutzer/${props.author.name}/${props.author.id}`" v-tooltip="props.author.name"
        class="header-autho" v-if="props.author.id > 0">
        <Image :src="props.author.imgUrl" :format="ImageFormat.Author" class="header-author-icon"
            :alt="`${props.author.name}'s profile picture'`" />
    </LazyNuxtLink>
</template>

<style lang="less">
.header-author-icon {
    height: 20px;
    width: 20px;
    min-height: 20px;
    min-width: 20px;
    margin: 0 4px;
    max-height: 20px;
    max-width: 20px;

    .img {
        object-fit: cover;
    }
}
</style>
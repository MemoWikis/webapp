<script lang="ts" setup>
import { ImageStyle } from '../image/imageStyleEnum';
import { Author } from './author'
const props = defineProps({
    author: { type: Object as () => Author, required: true },
})

</script>

<template>
    <LazyNuxtLink :to="`/Nutzer/${props.author.Name}/${author.Id}`" v-tooltip="props.author.Name">
        <Image :src="props.author.ImgUrl" :style="ImageStyle.Author" class="header-author-icon" />
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
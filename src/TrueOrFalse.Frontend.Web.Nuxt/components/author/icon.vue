<script lang="ts" setup>
import { ImageFormat } from '../image/imageFormatEnum';
import { Author } from './author'
interface Props {
    author: Author
}
const props = defineProps<Props>()

</script>

<template>
    <LazyNuxtLink :to="`/Nutzer/${props.author.Name}/${props.author.Id}`" v-tooltip="props.author.Name" class="header-autho"
        v-if="props.author.Id > 0">
        <Image :src="props.author.ImgUrl" :format="ImageFormat.Author" class="header-author-icon"
            :alt="`${props.author.Name}'s profile picture'`" />
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
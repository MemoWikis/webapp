<script setup lang="ts">
import {PropType} from 'vue'
import {LinkType} from './linkTypeEnum'
// const props = defineProps({
//     id: Number,
//     type: String as PropType<LinkType>,
//     blank: Boolean
// })

// const props = defineProps<{
//     id: Number,
//     type: LinkType
//     blank?: Boolean}>()

</script>

<template>
    <a href="">
        <slot></slot>
    </a>
</template>
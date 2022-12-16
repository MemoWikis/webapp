<script lang="ts" setup>
import { ref } from 'vue'
import { ImageStyle } from './imageStyleEnum'

const props = defineProps({
  url: String,
  square: Boolean,
  class: { type: String, required: false, default: '' },
  style: { type: String as () => ImageStyle, required: false, default: ImageStyle.Topic }
})

const styleClass = ref('')

const config = useRuntimeConfig();
let type = ''
switch (props.style) {
  case ImageStyle.Topic:
    type = ' topic'
    break
  case ImageStyle.Author:
    type = ' author'
    break
}
styleClass.value = props.class + type
</script>

<template>
  <img :src="config.public.serverBase + props.url" :class="styleClass" />
</template>

<style scoped lang="less">
.topic {
  border-radius: 0;
}

.author {
  border-radius: 50%;
}
</style>
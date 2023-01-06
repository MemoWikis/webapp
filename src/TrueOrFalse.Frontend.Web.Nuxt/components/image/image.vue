<script lang="ts" setup>
import { ref } from 'vue'
import { ImageStyle } from './imageStyleEnum'

interface Props {
  url: string,
  alt?: string,
  square?: boolean,
  class?: string,
  style?: ImageStyle,
  showLicense?: boolean
}

const props = defineProps<Props>()

const cssClass = ref('')

const config = useRuntimeConfig()

onBeforeMount(() => {
  switch (props.style) {
    case ImageStyle.Topic:
      cssClass.value = 'topic'
      break
    case ImageStyle.Author:
      cssClass.value = 'author'
      break
    default: cssClass.value = 'topic'
  }

})

</script>

<template>
  <div class="img-container" :class="props.class">
    <img :src="config.public.serverBase + props.url" :class="cssClass" :alt="props.alt" />
    <div v-if="props.showLicense" class="license-btn">Lizenzinfos</div>
  </div>
</template>

<style scoped lang="less">
@import (reference) '~~/assets/includes/imports.less';

.img-container {
  display: flex;
  flex-direction: column;
  align-items: center;

  img {
    height: 100%;
    width: 100%;
  }

  .license-btn {
    cursor: pointer;
    color: @memo-grey-dark;
    line-height: 18px;
    font-size: 10px;
    text-align: center;

    &:hover {
      color: @memo-blue-lighter;
    }
  }
}

.topic {
  border-radius: 0;
}

.author {
  border-radius: 50%;
}
</style>
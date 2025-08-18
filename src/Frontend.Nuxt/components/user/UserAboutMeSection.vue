<script lang="ts" setup>
import { useUserStore } from './userStore'

const props = defineProps<{
    userId?: number,
    aboutMe?: string
}>()

const userStore = useUserStore()
const { t } = useI18n()

const aboutMeModel = ref<string>('')
const readonly = computed(() => props.userId != null && props.userId !== userStore.id)

onMounted(() => {
    aboutMeModel.value = props.aboutMe || ''
})

</script>

<template>
    <div class="about-me-section">
        <p>Passionate about learning and sharing knowledge.</p>
        <p>I enjoy helping others discover new things.</p>
    </div>



</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.about-me-section {
    font-size: 18px;
    margin-bottom: 10px;
    color: @memo-grey;

    p {
        max-width: 500px;
        line-height: 1.4;
        margin: 0;
    }

    .star {
        color: @memo-yellow;
    }

    .info-icon {
        color: @memo-grey-light;
        margin-right: 4px;
    }

}
</style>
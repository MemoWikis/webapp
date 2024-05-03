<script lang="ts" setup>
import { MoveTopicTransferData, useDragStore } from '~/components/shared/dragStore'
import { useUserStore } from '~/components/user/userStore'

const userStore = useUserStore()
const dragStore = useDragStore()

const { isDesktop } = useDevice()
const style = computed(() => {

    const x = isDesktop ? dragStore.x : dragStore.x
    const y = isDesktop ? dragStore.y : dragStore.y - 85

    const str = `top:${y - (userStore.showBanner ? 96 : 0)}px; left:${x}px; position: fixed; z-index: 2000 !important;`
    return str
})

const topicName = ref('')

watch(() => dragStore.transferData, (t) => {
    if (dragStore.isMoveTopicTransferData) {
        const m = t as MoveTopicTransferData
        topicName.value = m.topic.name

    }
}, { deep: true })

</script>

<template>
    <div class="ghost-container" :style="style">
        <div class="ghost-body">
            <div class="name">
                {{ topicName }}
            </div>
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.ghost-container {
    background: rgba(99, 199, 238, 0.555);
    padding: 12px 24px;
    border: solid 1px @memo-grey-light;
    border-radius: 4px;
    box-shadow: 0 2px 6px rgb(0 0 0 / 16%);
    pointer-events: none;
    margin-top: 50px;

    .ghost-body {
        // display: flex;
        // flex-wrap: nowrap;
        // justify-content: center;
        // align-items: center;
    }
}
</style>
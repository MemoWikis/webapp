<script lang="ts" setup>
import { MovePageTransferData, useDragStore } from '~/components/shared/dragStore'
import { useUserStore } from '~/components/user/userStore'

const userStore = useUserStore()
const dragStore = useDragStore()

const ghostContainer = ref()

const coordinates = reactive({
    x: 0,
    y: 0
})
const style = computed(() => {

    if (ghostContainer.value == null)
        return

    const ghostWidthOffset = ghostContainer.value.clientWidth / 2
    const newX = dragStore.x - ghostWidthOffset

    const topOffset = 85
    const ghostHeightOffset = ghostContainer.value.clientHeight / 2
    const newY = dragStore.y - topOffset - ghostHeightOffset


    if (newX > 0)
        coordinates.x = newX
    if (newY > 0)
        coordinates.y = newY

    const str = `top:${coordinates.y - (userStore.showBanner ? 96 : 0)}px; left:${coordinates.x}px; position: fixed; z-index: 2000 !important;`
    return str
})

const pageName = ref('')

watch(() => dragStore.transferData, (t) => {
    if (dragStore.isMovePageTransferData) {
        const m = t as MovePageTransferData
        pageName.value = m.page.name

    }
}, { deep: true })

</script>

<template>
    <div class="ghost-container" :style="style" ref="ghostContainer">
        <div class="ghost-body">
            <div class="name">
                {{ pageName }}
            </div>
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.ghost-container {
    background: white;
    padding: 12px 24px;
    border: solid 1px @memo-grey-light;
    border-radius: 4px;
    box-shadow: 0 2px 6px rgb(0 0 0 / 16%);
    pointer-events: none;
    margin-top: 50px;
    max-width: 260px;
    height: 56px;

    .ghost-body {
        display: flex;
        flex-wrap: nowrap;
        justify-content: center;
        align-items: center;
        height: 100%;
        width: 100%;

        .name {
            max-width: 212px;
            text-overflow: ellipsis;
            text-wrap: nowrap;
            overflow: hidden;
        }
    }
}
</style>
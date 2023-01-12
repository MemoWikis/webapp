<script lang="ts" setup>
import { usePinStore, PinState } from './pinStore'
const pinStore = usePinStore()

interface Props {
    isInWishknowledge: boolean,
    questionId: number
}

const props = defineProps<Props>()

const state = ref<PinState>(PinState.Loading)
watch(() => props.isInWishknowledge, (val) => {
    if (val)
        state.value = PinState.Added
    else state.value = PinState.NotAdded
})

onBeforeMount(() => {
    if (props.isInWishknowledge)
        state.value = PinState.Added
    else state.value = PinState.NotAdded
})

const showLabel = ref(false)

onMounted(() => {
    pinStore.$onAction(({ after }) => {
        after((result) => {
            if (result != null && result.id == props.questionId) {
                state.value = result.state
            }
        })
    })
})

function pin() {
    state.value = PinState.Loading
    pinStore.pin(props.questionId)
}

function unpin() {
    state.value = PinState.Loading
    pinStore.unpin(props.questionId)
}
</script>

<template>
    <div>

        <span v-if="state == PinState.Added" @click="unpin()" v-tooltip="'Aus deinem Wunschwissen entfernen'">
            <font-awesome-icon icon="fa-solid fa-heart" class="pin-icon" />
        </span>
        <span v-else-if="state == PinState.Loading">
            <font-awesome-icon icon="fa-solid fa-spinner fa-spin" class="pin-icon" />
        </span>
        <span v-else v-tooltip="'Zu deinem Wunschwissen hinzuzufügen'" @click="pin()">
            <font-awesome-icon icon="fa-regular fa-heart" class="pin-icon" />
            <span v-if="showLabel" class="pin-label">Hinzufügen</span>
        </span>
    </div>

</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.pin-icon {
    color: @memo-wuwi-red;
}

.pin-label {
    padding: 0 3px;
    font-size: 8px;
    line-height: 14px;
    font-weight: 600;
    text-transform: uppercase;
    letter-spacing: 0.1em;
    color: @memo-wuwi-red;
}
</style>
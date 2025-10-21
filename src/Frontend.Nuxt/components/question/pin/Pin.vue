<script lang="ts" setup>
import { usePinStore, PinState } from './pinStore'
import { useUserStore } from '~~/components/user/userStore'
import { PinData } from '~~/components/question/pin/pinStore'
import { AlertType, useAlertStore } from '~/components/alert/alertStore'

const pinStore = usePinStore()
const userStore = useUserStore()
const { t } = useI18n()

interface Props {
    isInWishKnowledge: boolean,
    questionId: number,
}

const isActivePin = ref(false)

const props = defineProps<Props>()

const pinState = ref<PinState>(PinState.Loading)
watch(() => props.isInWishKnowledge, (val) => {
    if (val)
        pinState.value = PinState.Added
    else pinState.value = PinState.NotAdded
})

onBeforeMount(() => {
    if (props.isInWishKnowledge)
        pinState.value = PinState.Added
    else pinState.value = PinState.NotAdded
})

const showLabel = ref(false)
const emit = defineEmits(['set-wuwi-state'])

onMounted(() => {
    pinStore.$onAction(({ after }) => {
        after((result: FetchResult<PinData>) => {
            if (result.data?.id != props.questionId) return
            if (result.success) {
                pinState.value = result.data?.state
                    ; (function emitToParentQuestionRow() {
                        emit('set-wuwi-state', result.data?.state)
                    })()
            } else {
                if (isActivePin.value) {
                    const alertStore = useAlertStore()
                    alertStore.openAlert(AlertType.Error, { text: t(result.messageKey) })
                        ; (function resetToInitialPinState() {
                            pinState.value = props.isInWishKnowledge ? PinState.Added : PinState.NotAdded
                        })()
                }

            }
            isActivePin.value = false
        })
    })
})

function pin() {
    if (userStore.isLoggedIn) {
        isActivePin.value = true
        pinState.value = PinState.Loading
        pinStore.pin(props.questionId)
    } else {
        userStore.openLoginModal()
    }
}

function unpin() {
    if (userStore.isLoggedIn) {
        isActivePin.value = true
        pinState.value = PinState.Loading
        pinStore.unpin(props.questionId)
    } else {
        userStore.openLoginModal()
    }
}

</script>

<template>
    <div>
        <span v-if="pinState === PinState.Added" @click="unpin()" v-tooltip="t('pin.tooltips.removeFromWishKnowledge')">
            <font-awesome-icon icon="fa-solid fa-heart" class="pin-icon" />
        </span>
        <span v-else-if="pinState === PinState.Loading">
            <font-awesome-icon icon="fa-solid fa-spinner fa-spin" class="pin-icon" />
        </span>
        <span v-else v-tooltip="t('pin.tooltips.addToWishKnowledge')" @click="pin()">
            <font-awesome-icon icon="fa-regular fa-heart" class="pin-icon" />
            <span v-if="showLabel" class="pin-label">{{ t('pin.labels.add') }}</span>
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
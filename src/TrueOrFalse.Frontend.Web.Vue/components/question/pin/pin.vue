<script lang="ts" setup>
import { ref, watch, onMounted } from 'vue'
import { usePinStore, PinAction } from './pinStore'
const props = defineProps(['isInWishknowledge', 'questionId'])
enum PinState {

    Added,
    Loading,
    NotAdded
}
const stateLoad = ref(PinState.NotAdded)
const showAddTxt = ref(false)
const type = ref('default')

watch(() => props.isInWishknowledge.value, (val) => {
    if (val)
        stateLoad.value = PinState.Added
    else
        stateLoad.value = PinState.NotAdded
})

onMounted(() => {
    if (props.isInWishknowledge.value)
        stateLoad.value = PinState.Added
    else
        stateLoad.value = PinState.NotAdded
})
const pinStore = usePinStore()
pinStore.$onAction(({ name, after }) => {
    after(() => {
        if (name == 'update' && pinStore.id == props.questionId.value) {
            if (pinStore.action == PinAction.Add)
                stateLoad.value = PinState.Added
            else
                stateLoad.value = PinState.NotAdded
        }
    })
})
async function pin() {
    stateLoad.value = PinState.Loading
    var result = await $fetch<boolean>(`/api/Api/Questions/Pin/${props.questionId.value}`, {
        method: 'POST', mode: 'cors', credentials: 'include'
    })
    if (result) {
        stateLoad.value = PinState.Added
        pinStore.update(props.questionId.value, PinAction.Add)
    }
    else stateLoad.value = PinState.NotAdded
}

async function unpin() {
    stateLoad.value = PinState.Loading
    var result = await $fetch<boolean>(`/api/Api/Questions/UnPin/${props.questionId.value}`, {
        method: 'POST', mode: 'cors', credentials: 'include'
    })
    if (result) {
        stateLoad.value = PinState.NotAdded
        pinStore.update(props.questionId.value, PinAction.Remove)
    }
    else stateLoad.value = PinState.Added
}


</script>

<template>

    <div>
        <div v-if="type == 'full'" class="pin-container">
            <input id="toggle-heart" type="checkbox" />
            <label for="toggle-heart">❤</label>
        </div>
        <div v-else-if="type == 'long'">

        </div>
        <div v-else>
            <span v-if="stateLoad == PinState.Added" @click="unpin()">
                <font-awesome-icon icon="fa-solid fa-heart" class="pinIcon"
                    v-tooltip="'Aus deinem Wunschwissen entfernen'" />
            </span>
            <span v-else-if="stateLoad == PinState.NotAdded">
                <font-awesome-icon icon="fa-solid fa-spinner" class="pinIcon" />
            </span>
            <span v-else title="Zu deinem Wunschwissen hinzuzufügen" @click="pin()">
                <font-awesome-icon icon="fa-regular fa-heart" class="pinIcon" />
                <span v-if="showAddTxt" class="Text">Hinzufügen</span>
            </span>
        </div>
    </div>


</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.pinIcon {
    color: @memo-red;
}
</style>

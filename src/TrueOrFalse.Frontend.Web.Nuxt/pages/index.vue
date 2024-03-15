<script lang="ts" setup>
import { Page } from '~~/components/shared/pageEnum'
import { useUserStore } from '~~/components/user/userStore'
const userStore = useUserStore()

if (userStore.isLoggedIn) {
    await navigateTo(`${userStore.personalWiki?.name}/${userStore.personalWiki?.id}`)
} else {
    await navigateTo(`/Globales-Wiki/1`)
}

const emit = defineEmits(['setPage'])
onBeforeMount(() => {
    emit('setPage', Page.Welcome)
})
</script>

<template>
    <div></div>
</template>
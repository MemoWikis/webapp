<script lang="ts" setup>
import { Site } from '~/components/shared/siteEnum'
import { useUserStore } from '~~/components/user/userStore'
const userStore = useUserStore()

if (userStore.isLoggedIn) {
    await navigateTo(`${userStore.personalWiki?.name}/${userStore.personalWiki?.id}`)
} else {
    await navigateTo(`/Globales-Wiki/1`)
}

const emit = defineEmits(['setPage'])
onBeforeMount(() => {
    emit('setPage', Site.Welcome)
})
</script>

<template>
    <div></div>
</template>
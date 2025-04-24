<script lang="ts" setup>
import { SiteType } from '~/components/shared/siteEnum'
import { useUserStore } from '~~/components/user/userStore'
const userStore = useUserStore()

if (userStore.isLoggedIn) {
    await navigateTo(`${userStore.personalWiki?.name}/${userStore.personalWiki?.id}`)
} else {
    await navigateTo(`/Globales-Wiki/1`)
}

const emit = defineEmits(['setPage'])
onBeforeMount(() => {
    emit('setPage', SiteType.Welcome)
})
</script>

<template>
    <div></div>
</template>
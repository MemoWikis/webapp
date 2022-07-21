<script lang="ts" setup>
import { useUserStore } from './user/userStore'
const userStore = useUserStore()
</script>

<template>
  <div>
    <LazyNavigationGuest v-if="!userStore.isLoggedIn" />
    <NavigationMain />
  </div>
</template>

<style scoped>
</style>

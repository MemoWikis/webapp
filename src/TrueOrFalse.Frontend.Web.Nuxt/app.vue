<script lang="ts" setup>
import { useUserStore } from './components/user/userStore'
const userStore = useUserStore()
const route = useRoute()
</script>

<template>
  <LazyHeaderGuest v-if="!userStore.isLoggedIn" />
  <HeaderMain :route="route" />
  <NuxtPage />
  <LazyClientOnly>
    <LazyUserLogin v-if="!userStore.isLoggedIn" />
    <LazySpinner />
    <LazyAlert />
  </LazyClientOnly>
  <Footer />
</template>

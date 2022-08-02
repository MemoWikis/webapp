<script lang="ts" setup>
import { useUserStore, LoginState } from './components/user/userStore';
const loginState = useState<LoginState>('loginState')
const route = useRoute()
const userStore = useUserStore()
if (loginState.value != null && loginState.value != undefined)
  userStore.initUserStore(loginState.value)

useHead({
  link: [
    { rel: 'icon', type: 'image/x-icon', href: 'http://localhost:5211/Images/Logo/LogoMemoWiki.svg' }
  ]
})
</script>


<template>
  <LazyHeaderGuest v-if="!userStore.isLoggedIn" />
  <HeaderMain :route="route" />
  <NuxtPage />
  <LazyUserLogin v-if="!userStore.isLoggedIn" />
  <LazySpinner />
  <LazyAlert />
  <Footer />
</template>

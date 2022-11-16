<script lang="ts" setup>
import { useUserStore, LoginState } from './components/user/userStore'

const loginState = useState<LoginState>('loginState')
const route = useRoute()
const userStore = useUserStore()
if (loginState.value != null && loginState.value != undefined)
  userStore.initUserStore(loginState.value)
const config = useRuntimeConfig()


// onMounted(async () => {
//   const result = await $fetch<LoginState>(`/SessionUser/GetCurrentUser/`, {
//     baseURL: config.apiBase,
//     credentials: 'include',
//     headers: useRequestHeaders(['cookie']),
//     mode: 'no-cors'
//   })
//   console.log(result)
// })
useHead({
  link: [
    { rel: 'icon', type: 'image/x-icon', href: 'http://memucho.local/Images/Logo/LogoMemoWiki.svg' }
  ]
})
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

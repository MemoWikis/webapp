<script lang="ts" setup>
import { useUserStore, LoginState } from './components/user/userStore';
const loginState = useState<LoginState>('loginState')
const userStore = useUserStore()
userStore.initUserStore(loginState.value)


useHead({
  link: [
    { rel: 'icon', type: 'image/x-icon', href: 'http://localhost:5211/Images/Logo/LogoMemoWiki.svg' }
  ]
})
</script>


<template>
  <div>
    <Header />
    <NuxtPage />
    <LazyUserLogin v-if="!userStore.isLoggedIn" />
    <LazySpinner />
    <LazyAlert />
    <Footer />
  </div>
</template>

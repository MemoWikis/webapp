<script lang="ts" setup>
const isLoggedIn = useState('isLoggedIn')

</script>


<template>
  <div>
    <Header/>
    <NuxtPage />
    <UserLogin v-if="!isLoggedIn" />
    <Spinner/>
    <Footer/>
  </div>
</template>

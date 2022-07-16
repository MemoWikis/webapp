<script lang="ts" setup>
const isLoggedIn = useState('isLoggedIn')
</script>


<template>
  <div>
    <Header />
    <NuxtPage />
    <LazyUserLogin v-if="!isLoggedIn" />
    <LazySpinner />
    <LazyAlert />
    <Footer />
  </div>
</template>

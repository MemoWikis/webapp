<script lang="ts" setup>
const isLoggedIn = useState('isLoggedIn')

</script>

<template>
  <div>
    <LazyNavigationGuest v-if="!isLoggedIn" />
    <NavigationMain />
  </div>
</template>

<style scoped>
</style>

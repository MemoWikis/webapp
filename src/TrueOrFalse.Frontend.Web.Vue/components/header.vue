<script lang="ts" setup>
const isLoggedIn = useState('isLoggedIn')

</script>

<template>
  <div>
    <NavBarSubNavBar v-if="!isLoggedIn"/>
    <NavBar/>
  </div>
</template>

<style scoped></style>

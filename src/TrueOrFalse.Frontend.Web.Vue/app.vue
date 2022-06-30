<script lang="ts" setup>
  import {useUtilsStore} from '~~/components/utils/utilsStore'
  const utilsStore = useUtilsStore()
</script>

<template>
  <div>
    <Header/>
    <div class="spinner" v-show="utilsStore.showSpinner">
      <div class="double-bounce1"></div>
      <div class="double-bounce2"></div>
    </div>
    <!-- <UserLogin /> -->
    <NuxtPage />
    <Footer/>
  </div>
</template>

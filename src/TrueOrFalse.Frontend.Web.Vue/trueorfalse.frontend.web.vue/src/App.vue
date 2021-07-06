<template>
  <div id="app">
    <AppHeader />
    <Dashboard msg="Welcome to Your Vue.js App" />
  </div>
</template>

<script>
import Dashboard from "./components/Dashboard.vue";
import AppHeader from "./components/AppHeader.vue";
export default {
  name: "App",
  components: {
    Dashboard,
    AppHeader
  },
  props: {
    cookie: String
  }
};
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
}
</style>

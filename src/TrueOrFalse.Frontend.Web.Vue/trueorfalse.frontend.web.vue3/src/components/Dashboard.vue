<template>
  <div>
    <h1>You are logged in</h1>
    <h1>Total: {{ questionsTotal }}</h1>
  </div>
</template>

<script>
import axios from "axios";

export default {
  name: "CookieHead",
  props: {
    msg: String,
  },

  data: function() {
    return {
      cookie: Boolean,
      questionsTotal: 0,
    };
  },

  // Server-side only
  // This will be called by the server renderer automatically
  serverPrefetch() {
    // return the Promise from the action
    // so that the component waits before rendering
    return this.fetchData();
  },

  // Client-side only
  mounted() {
    // If we didn't already do it on the server
    // we fetch the item (will first show the loading text)
    if (this.questionsTotal == 0) {
      this.fetchData();
    }
  },

  methods: {
    fetchData() {
      axios
        .get("http://localhost:26590/EduSharingApi/Statistics")
        .then((response) => (this.questionsTotal = response.data.overall.count))
        .catch((error) => {
          this.errorMessage = error.message;
          console.error("There was an error!", error);
        });
      // return the Promise from the action
      //return this.$store.dispatch('fetchItem', this.$route.params.id)
    },
  },

  async beforeCreate() {
    await axios
      .get("http://localhost:26590/EduSharingApi/Statistics")
      .then((response) => (this.questionsTotal = response.data.overall.count))
      .catch((error) => {
        this.errorMessage = error.message;
        console.error("There was an error!", error);
      });
  },
};
</script>

<template>
  <div style="padding: 0 80px 0 80px">
    <h1>Dashboard</h1>
    <h2>neue Themen und neue Fragen im Vergleich</h2>
    <div style="padding: 0px 0px 16px 0px">
      <label>Der letzten </label>
      <input style="width:40px" v-model="message" placeholder="x" />
      <span> Tage</span>
    </div>
    <div>
      <canvas
        id="questionStats"
        style="max-width:50%; border: 5px solid;"
      ></canvas>
    </div>
  </div>
</template>

<script>
import axios from "axios";
import Chart from "chart.js";
import memuchoStats from "../memuchoStats.js";

export default {
  name: "CookieHead",
  props: {
    msg: String,
  },

  data: function() {
    return {
      cookie: Boolean,
      questionsTotal: 0,
      memuchoStats: memuchoStats,
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
    const ctx = document.getElementById("questionStats");
    new Chart(ctx, this.memuchoStats);
    // If we didn't already do it on the server
    // we fetch the item (will first show the loading text)
    if (this.questionsTotal == 0) {
      this.fetchData();
    }
  },

  methods: {
    fetchData() {
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

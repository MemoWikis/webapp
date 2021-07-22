<template>
  <div id="dashboard" style="padding: 0 80px 0 80px display: flex;">
    <h1>Dashboard</h1>
    <div style="padding: 0 16px 0 16px; width:40%;">
      <chartOverTime
        headerText="Neue Fragen"
        :chartData="questionsData"
        chartId="questions"
        lineLabel="erstellte Fragen"
      ></chartOverTime>
    </div>
    <div style="padding: 0 16px 0 16px; width:40%;">
      <chartOverTime
        headerText="Neue Themen"
        :chartData="themeData"
        chartId="themes"
        lineLabel="erstellte Themen"
      ></chartOverTime>
    </div>
  </div>
</template>

<script>
import axios from "axios";
import moment from "moment";
import chartOverTime from "./ChartOverTime.vue";

export default {
  components: {
    chartOverTime,
  },
  props: {
    msg: String,
  },

  data: function() {
    return {
      cookie: Boolean,
      themeData: [19, 12, 14, 16, 18, 21, 11, 19, 22, 17],
      questionsData: [12, 14, 20, 15, 13, 17, 10, 19],
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
      // return the Promise from the action
      //return this.$store.dispatch('fetchItem', this.$route.params.id)
    },
  },

  // async beforeCreate() {
  //   await axios
  //     .get("http://localhost:26590/EduSharingApi/Statistics")
  //     .then((response) => (this.questionsTotal = response.data.overall.count))
  //     .catch((error) => {
  //       this.errorMessage = error.message;
  //       console.error("There was an error!", error);
  //     });
  // },
};
</script>

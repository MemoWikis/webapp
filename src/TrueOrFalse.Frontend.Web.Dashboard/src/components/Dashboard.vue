<template>
  <div
    id="dashboard"
    style="display: flex; flex-wrap: wrap; justify-content: space-evenly"
  >
    <h1 style="width: 100%">Dashboard</h1>
    <div style="flex-basis: 40%">
      <chartOverTime
        headerText="Neue Fragen"
        :chartURL="'http://localhost:26590/StatisticsDashboard/GetCreatedQuestionsInTimeWindow?'"
        chartId="questions"
        lineLabel="erstellte Fragen"
      ></chartOverTime>
    </div>
    <div style="flex-basis: 40%">
      <chartOverTime
        headerText="Neue Nutzer"
        :chartURL="'http://localhost:26590/api/StatisticsDashboard/GetCreatedUsersInTimeWindow?'"
        chartId="user"
        lineLabel="neu registrierte Nutzer"
      ></chartOverTime>
    </div>
    <div style="flex-basis: 40%">
      <chartOverTime
        headerText="Neue Themen"
        :chartURL="'http://localhost:26590/api/StatisticsDashboard/GetCreatedCategoriesInTimeWindow?'"
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

  data: function () {
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

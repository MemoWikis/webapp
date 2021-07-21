<template>
  <div style="padding: 0 80px 0 80px">
    <h1>Dashboard</h1>
    <h2>neue Themen und neue Fragen im Vergleich</h2>
    <div>
      <label>Der letzten </label>
      <input
        style="width:56px"
        v-model="goBackDays"
        placeholder="8"
        type="number"
      />
      <span> Tage. Bestimme den Zeitintervall im Graphen </span>
      <select v-model="selectedLabel">
        <option
          v-for="option in optionsLabel"
          :value="option.value"
          :key="option.id"
        >
          {{ option.text }}
        </option>
      </select>
    </div>
    <div>
      <button style="margin: 8px 0px 16px 0px" v-on:click="calculateChart">
        Berechnen
      </button>
    </div>
    <div>
      <canvas
        id="questionStats"
        style="max-width:50%; border: 4px solid;"
      ></canvas>
    </div>
  </div>
</template>

<script>
import axios from "axios";
import Chart from "chart.js";
import moment from "moment";
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
      selectedLabel: "day",
      optionsLabel: [
        { text: "Tage", value: "day" },
        { text: "Wochen", value: "week" },
        { text: "Monate", value: "month" },
        { text: "Jahre", value: "year" },
      ],
      memuchoStats: memuchoStats,
      goBackDays: 8,
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
    // this.memuchoStats.data.datasets[0].data = [12, 14, 20, 15, 13, 17, 10, 19];
    // var fromDate = moment().subtract(this.goBackDays, "day");
    // var toDate = moment();
    // this.memuchoStats.data.labels = this.enumerateTimeBetweenDates(
    //   fromDate,
    //   toDate
    // );
    // const ctx = document.getElementById("questionStats");
    // new Chart(ctx, this.memuchoStats);
    this.calculateChart();
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

    calculateChart() {
      this.memuchoStats.data.datasets[0].data = [
        12,
        14,
        20,
        15,
        13,
        17,
        10,
        19,
      ];
      var fromDate = moment().subtract(this.goBackDays, "day");
      var toDate = moment();
      this.memuchoStats.data.labels = this.enumerateTimeBetweenDates(
        fromDate,
        toDate,
        this.selectedLabel
      );
      const ctx = document.getElementById("questionStats");
      new Chart(ctx, this.memuchoStats);
    },

    enumerateTimeBetweenDates(startDate, endDate, interval) {
      var dates = [];

      var currDate = moment(startDate).startOf(interval);
      var lastDate = moment(endDate).startOf(interval);

      while (currDate.add(1, this.selectedLabel).diff(lastDate) < 0) {
        console.log(currDate.toDate());
        dates.push(currDate.clone().format("DD/MM/YYYY"));
      }
      dates.push(lastDate.clone().format("DD/MM/YYYY"));
      return dates;
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

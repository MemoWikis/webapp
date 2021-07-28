<template>
  <h2>{{ headerText }}</h2>
  <div>
    <label>Der letzten </label>
    <input
      style="width:56px"
      v-model="goBackDays"
      placeholder="8"
      type="number"
    />
    <span> Tage. Bestimme den Zeitintervall im Graphen </span>
    <select v-model="selectedInterval">
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
      Abfragen
    </button>
  </div>
  <div>
    <canvas :id="chartId" style="border: 4px solid;"></canvas>
  </div>
</template>

<script>
import Chart from "chart.js";
import moment from "moment";
import axios from "axios";

export default {
  props: {
    headerText: String,
    chartData: Array,
    chartId: String,
    lineLabel: String,
  },

  data: function() {
    return {
      selectedInterval: "day",
      optionsLabel: [
        { text: "Tage", value: "day" },
        { text: "Wochen", value: "week" },
        { text: "Monate", value: "month" },
        { text: "Jahre", value: "year" },
      ],
      goBackDays: 8,

      memuchoStats: {
        type: "line",
        data: {
          labels: [
            "Januar",
            "Februar",
            "März",
            "April",
            "Mai",
            "Juni",
            "Juli",
            "August",
          ],
          datasets: [
            {
              label: "neue",
              data: [],
              backgroundColor: "rgba(54,73,93,.5)",
              borderColor: "#36495d",
              borderWidth: 3,
            },
          ],
        },
        options: {
          responsive: true,
          lineTension: 1,
          scales: {
            yAxes: [
              {
                ticks: {
                  beginAtZero: true,
                  padding: 25,
                },
              },
            ],
          },
        },
      },
    };
  },

  mounted() {
    this.memuchoStats.data.datasets[0].label = this.lineLabel;
    //this.calculateChart();
  },

  methods: {
    calculateChart() {
      var url =
        "http://localhost:26590/StatisticsDashboard/GetCreatedQuestionsInTimeWindow?amount=30&interval=month"; //"http://localhost:26590/StatisticsDashboard/GetCreatedQuestionsInTimeWindow?amount=" +
      //this.goBackDays + "&interval=" + this.selectedInterval;
      axios
        .get(url)
        .then(
          (response) =>
            (this.memuchoStats.data.datasets[0].data = response.data)
        )
        .catch((error) => {
          this.errorMessage = error.message;
          console.error("There was an error!", error);
        });
      console.log(this.memuchoStats.data.datasets[0].data);

      //this.memuchoStats.data.datasets[0].data = this.chartData;
      var fromDate = moment().subtract(this.goBackDays, "day");
      var toDate = moment();
      this.memuchoStats.data.labels = this.enumerateTimeBetweenDates(
        fromDate,
        toDate,
        this.selectedInterval
      );
      const ctx = document.getElementById(this.chartId);
      new Chart(ctx, this.memuchoStats);
    },

    enumerateTimeBetweenDates(startDate, endDate, interval) {
      var dates = [];

      var currDate = moment(startDate).startOf(interval);
      var lastDate = moment(endDate).startOf(interval);

      while (currDate.add(1, this.selectedInterval).diff(lastDate) < 0) {
        //console.log(currDate.toDate());
        dates.push(currDate.clone().format("DD/MM/YYYY"));
      }
      dates.push(lastDate.clone().format("DD/MM/YYYY"));
      return dates;
    },
  },
};
</script>

<style></style>

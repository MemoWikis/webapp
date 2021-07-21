export const memuchoStatsData = {
  type: "line",
  data: {
    labels: ["Januar", "Februar", "März", "April", "Mai", "Juni", "Juli", "August"],
    datasets: [
      {
        label: "neue Themen",
        data: [],
        backgroundColor: "rgba(54,73,93,.5)",
        borderColor: "#36495d",
        borderWidth: 3
      },
      {
        label: "neue Fragen",
        data: [56, 45, 53, 43, 58, 39, 43, 51],
        backgroundColor: "rgba(71, 183,132,.5)",
        borderColor: "#47b784",
        borderWidth: 3
      }
    ]
  },
  options: {
    responsive: true,
    lineTension: 1,
    scales: {
      yAxes: [
        {
          ticks: {
            beginAtZero: true,
            padding: 25
          }
        }
      ]
    }
  }
};

export default memuchoStatsData;
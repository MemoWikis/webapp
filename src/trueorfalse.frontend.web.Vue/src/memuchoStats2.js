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
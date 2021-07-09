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
  async beforeCreate() {
    await axios
      .get("http://localhost:26590/EduSharingApi/Statistics")
      .then((response) => console.log(response.data.overall.count))
      .catch((error) => {
        this.errorMessage = error.message;
        console.error("There was an error!", error);
      });
  },
};
</script>

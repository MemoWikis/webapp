<template>
  <div>
    <h1>You are logged in</h1>
    <h1>Total: {{ questionsTotal }}</h1>
  </div>
</template>

<script>
//import VueCookies from "vue-cookies";
import axios from "axios";

export default {
  name: "CookieHead",
  props: {
    msg: String
  },
  data: function() {
    return {
      cookie: Boolean,
      questionsTotal: 0
    };
  },
  async created() {
    //this.cookie = VueCookies.isKey("memucho");
    await axios
      .get("http://localhost:26590/EduSharingApi/Statistics")
      .then(response => (this.questionsTotal = response.data.overall.count))
      .catch(error => {
        this.errorMessage = error.message;
        console.error("There was an error!", error);
      });
  }
};
</script>
<style scoped>
h3 {
  margin: 40px 0 0;
}
ul {
  list-style-type: none;
  padding: 0;
}
li {
  display: inline-block;
  margin: 0 10px;
}
a {
  color: #42b983;
}
</style>

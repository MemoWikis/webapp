<template>
  <div>
    <h1 v-if="cookie">You are logged in</h1>
    <h1 v-else>You are not logged in</h1>
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
      cookie: Boolean
      // questionsTotal: JSON
    };
  },
  created: function() {
    //this.cookie = VueCookies.isKey("memucho");
    axios
      .get("http://localhost:26590/EduSharingApi/Statistics", { mode: "cors" })
      .then(response => console.log(response.data))
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

// import the package
import VueAwesomePaginate from "vue-awesome-paginate"

// import the necessary css file
import "vue-awesome-paginate/dist/style.css"

// Register the package
export default defineNuxtPlugin((nuxtApp) => {
    nuxtApp.vueApp.use(VueAwesomePaginate)
})
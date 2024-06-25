<script lang="ts" setup>
var error = useError();
console.log(error)
throw createError({ statusCode: 404, message: "Die Route existiert nicht" });

</script>
<template>
    <div></div>
</template>
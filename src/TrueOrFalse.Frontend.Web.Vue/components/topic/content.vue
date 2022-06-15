<script setup>
const props = defineProps({
  category: Object
})
</script>

<template>
    <div>
        {{props.category.text}}
    </div>
</template>
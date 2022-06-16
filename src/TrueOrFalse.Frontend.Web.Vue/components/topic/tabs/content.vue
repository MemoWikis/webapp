<script setup>
const props = defineProps({
  categoryId: Number
})
</script>

<template>
    <div>
      {{props.categoryId}}
        <!-- {{props.category.text}} -->
    </div>
</template>
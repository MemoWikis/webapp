<script lang="ts" setup>
const learningSessionResult = ref<any>(null)
const emit = defineEmits(['startNewSession'])

onBeforeMount(() => {
    learningSessionResult.value = $fetch<any>('/apiVue/VueLearningSessionResult/Get', {
        credentials: 'include',
        mode: 'cors'
    })
})
</script>

<template>
    <div if="learningSessionResult">
        <button @click="emit('startNewSession')">Neue Lernsitzung</button>
    </div>
</template>
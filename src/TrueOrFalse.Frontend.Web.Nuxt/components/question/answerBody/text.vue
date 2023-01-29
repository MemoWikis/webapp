<script lang="ts" setup>

const answer = ref('')
</script>

<template>

    <textarea id="txtAnswer" class="form-control " rows="1" placeholder="Gib hier bitte deine Antwort ein."
        v-model="answer">
    </textarea>

</template>

<style lang="less" scoped>
textarea {
    width: 100%;
}
</style>
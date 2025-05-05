<script lang="ts" setup>
const { t } = useI18n()
throw createError({ statusCode: 404, statusMessage: t('error.route.notFound') })
</script>
<template>
    <div></div>
</template>
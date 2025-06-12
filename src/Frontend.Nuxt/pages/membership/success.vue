<script lang="ts" setup>
const { t } = useI18n()
</script>

<template>
    <div class="main-content">
        <h1>{{ t('success.membership.thankYou') }}</h1>
        <p>
            {{ t('success.membership.congratulations') }}
        </p>
    </div>
</template>

<style lang="less" scoped>
p {
    font-size: 16px;
    padding-top: 24px;
    font-size: 16px;
    white-space: pre-wrap;
}
</style>

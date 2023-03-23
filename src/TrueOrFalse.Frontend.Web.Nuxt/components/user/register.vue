<script lang="ts" setup>
import { Google } from '~~/components/user/Google'
import { FacebookMemuchoUser } from '~~/components/user/FacebookMemuchoUser'
import { AlertType, useAlertStore, messages } from '~~/components/alert/alertStore'
import { useUserStore } from '~~/components/user/userStore'
import { useSpinnerStore } from '~~/components/spinner/spinnerStore'

const alertStore = useAlertStore()
const userStore = useUserStore()
const spinnerStore = useSpinnerStore()

</script>

<template></template>


<style lang="less" scoped>
@import '~~/assets/shared/register.less';
</style>
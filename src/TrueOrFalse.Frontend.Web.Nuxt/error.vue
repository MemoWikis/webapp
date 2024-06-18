<script lang="ts" setup>
// import { useUserStore } from '~/components/user/userStore'
import { Page } from './components/shared/pageEnum'
import { ErrorCode } from './components/shared/errorCodeEnum'
import type { NuxtError } from '#app'

const props = defineProps({
    error: Object as () => NuxtError
})


// const props = defineProps<{ errorCode: ErrorCode }>();

// const userStore = useUserStore()
const emit = defineEmits(['setPage'])
emit('setPage', Page.Error)

function handleError() {
    clearError({ redirect: '/' })
}
// watch(() => userStore.isLoggedIn, (val) => {
//     if (val)
//         handleError()
// })
// const route = useRoute();
// console.log(route);


onMounted(() => {
    if (props.error?.statusCode)
        setErrorData(props.error?.statusCode)
})

function setErrorData(statusCode: number) {
    errorImgSrc.value = {
        [ErrorCode.NotFound]: '/Images/memo-404_german_600.png',
        [ErrorCode.Unauthorized]: '/Images/Error/memo-401_german_600.png',
        [ErrorCode.Error]: '/Images/Error/memo-500_german_600.png'
    }[statusCode]

    description.value = {
        [ErrorCode.NotFound]: 'Hups diese Seite existiert nicht',
        [ErrorCode.Unauthorized]: 'Dir fehlen die Rechte um diese Seite anzuzeigen'
    }[statusCode]
}

const errorImgSrc = ref<string | undefined>('')
const description = ref<string | undefined>('')

</script>

<template>
    <div class="col-xs-12 container">
        <div class="error-page">
            <Image v-if="errorImgSrc" :src="errorImgSrc" class="error-image" />
            <div class="error-message">
                <p>Oder schicke eine E-Mail an team@memucho.de.</p>
                <strong>{{ description }}</strong>

                <ul>
                    <li>Wir wurden per E-Mail informiert.</li>
                    <li>Bei dringenden Fragen kannst du Robert unter 0178-1866848 erreichen.</li>
                    <li>Oder schicke eine E-Mail an team@memucho.de.</li>
                </ul>
            </div>
        </div>
    </div>
</template>


<style lang="less" scoped>
.error-page {
    display: flex;
    justify-content: center;
    align-items: center;
    padding-top: 60px;
    padding-bottom: 60px;
    flex-direction: column;

    .error-image {
        max-width: 600px;
        margin-bottom: 60px;
    }

    .error-message {
        width: 100%;
    }
}
</style>
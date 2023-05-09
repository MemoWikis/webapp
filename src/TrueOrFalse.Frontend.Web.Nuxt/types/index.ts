import { Logger } from 'winston';
export { }

declare global {
    // type SuccessResult<T> = {
    //     success: true
    //     data: T
    // }

    // type FailureResult = {
    //     success: false
    //     key: string
    // }

    // type FetchResult<T> = SuccessResult<T>

    type FetchResult<T> = {
        success: boolean
        data: T
        key: string
    }
}

declare module '#app' {
    interface NuxtApp {
        $logger: Logger
    }
}
declare module 'vue' {
    interface ComponentCustomProperties {
        $logger: Logger
    }
}
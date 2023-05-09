import { Logger } from 'winston';
export { }

declare global {
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
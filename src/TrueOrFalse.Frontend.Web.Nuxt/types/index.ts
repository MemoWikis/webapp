
export { }

declare global {
    type FetchResult<T> = {
        success: boolean
        data: T
        messageKey: string
    }
}

// declare module '#app' {
//     interface NuxtApp {
//         $log: LogLogger
//     }
// }
// declare module 'vue' {
//     interface ComponentCustomProperties {
//         $log: LogLogger
//     }
// }
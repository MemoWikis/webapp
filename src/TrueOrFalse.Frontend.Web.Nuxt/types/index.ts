
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
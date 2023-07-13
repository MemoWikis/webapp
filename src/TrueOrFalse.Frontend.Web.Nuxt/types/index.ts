
export { }

declare global {
    type FetchResult<T> = {
        success: boolean
        data: T
        messageKey: string
    }

    type IndexPath = number[]

    type NestedArray<T> = T | NestedArray<T>[]

    type ElementAndNestedArray<T> = { element: T, array: NestedArray<T> }
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
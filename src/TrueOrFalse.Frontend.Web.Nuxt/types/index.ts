export { }

declare global {
    // interface Fetch<T> {
    //     success: boolean
    //     data?: T
    //     key?: string
    // }
    type SuccessResult<T> = {
        success: true
        data: T
    }

    type FailureResult = {
        success: false
        key: string
    }

    type FetchResult<T> = SuccessResult<T> | FailureResult
}
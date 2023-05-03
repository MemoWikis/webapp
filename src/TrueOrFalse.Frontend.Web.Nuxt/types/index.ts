export { }

declare global {
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
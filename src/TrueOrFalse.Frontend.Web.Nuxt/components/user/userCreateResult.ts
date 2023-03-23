import { CurrentUser } from '../user/userStore'

export interface UserCreateResult {
    Success: boolean,
    EmailAlreadyInUse: boolean,
    currentUser?: CurrentUser
}
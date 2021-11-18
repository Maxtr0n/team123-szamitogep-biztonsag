export class LoginResponse {
    isSuccess: boolean;
    role: UserRole;
    token?: string;
}

export enum UserRole {
    ADMIN = 0,
    STANDARD = 1,
}
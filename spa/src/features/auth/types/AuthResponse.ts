import { User } from "@/types/User";

export type AuthResponse = {
    token: string;
    user: User;
    expiresIn: number;
};
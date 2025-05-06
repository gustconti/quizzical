import { User } from "@/types/User";

export type AuthState = {
    token: string | null;
    user: User | null;
    expiresAt: number;
    isLoading: boolean;
    error: string | undefined;
};

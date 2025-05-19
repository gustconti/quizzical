import { User } from "@/types/User";

export type AuthState = {
    user: User | null;
    token: string | null;
    expiresAt: number;
    guestName: string | undefined;
    isLoading: boolean;
    error: string | undefined;
};

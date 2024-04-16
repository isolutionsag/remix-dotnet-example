import type { PropsWithChildren } from "react";
import React, { useContext } from "react";
import type { User } from "~/models/User";

export const UserContext = React.createContext<User | null>(null);

interface ContextProps {
	user: User | null;
}
export function UserContextProvider({
	user,
	children,
}: PropsWithChildren<ContextProps>) {
	return <UserContext.Provider value={user}>{children}</UserContext.Provider>;
}

export const useUserContext = () => {
	return useContext(UserContext);
};
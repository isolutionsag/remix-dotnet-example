import React from "react";

// The default value (`undefined`) will be used on the client, the component will only be used
// on the server side to provide the nonce to components that need it.
export const NonceContext = React.createContext<string | undefined>(undefined);

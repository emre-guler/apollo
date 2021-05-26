import React, { useState } from 'react';

export const TokenContext = React.createContext("Context");

const Context = ({children}) => {
    const [token, setToken] = useState("");

    return (
        <TokenContext.Provider value={[token, setToken]}>
            {children}
        </TokenContext.Provider>
    )
};

export default Context;
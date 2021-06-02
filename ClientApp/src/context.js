import React, { useState } from 'react';

export const TokenContext = React.createContext();
export const DomainContext = React.createContext();

const Context = ({children}) => {
    const [token, setToken] = useState("");
    const [domain, setDomain] = useState("http://localhost:5000/");

    return (
        <TokenContext.Provider value={[token, setToken]}>
            <DomainContext.Provider value={[domain, setDomain]}>
                {children}
            </DomainContext.Provider>
        </TokenContext.Provider>
    )
};

export default Context;
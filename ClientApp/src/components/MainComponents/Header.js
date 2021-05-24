import React, { Component } from 'react';
import '../assets/header.scss';

class Header extends Component 
{
    render () {
        return (
            <header className="mainHeader">
                <div className="logoContainer">
                    <img src={require('./assets/apolloLogo.png')} />
                </div>
                <div className="headerContentContainer"></div>
            </header>            
        )
    }
}
export default Header;
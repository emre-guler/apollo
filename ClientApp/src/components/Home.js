import React, { Component } from 'react';
import Header from './MainComponents/Header';

export class Home extends Component {
  static displayName = Home.name;

  render () {
    return (
      <div>
        <Header />
      </div>
    );
  }
}
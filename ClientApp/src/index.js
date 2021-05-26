import React from 'react';
import ReactDOM from "react-dom";
import { BrowserRouter } from "react-router-dom";
import { Route } from "react-router";
import 'bootstrap/dist/css/bootstrap.min.css';

import Context from './context';
import Home  from "./pages/Home/Home";

const rootElement = document.getElementById("root");

const App = () => {
  return (
    <BrowserRouter>
      <Route path="/" component={Home} />
    </BrowserRouter>
  )
}

ReactDOM.render(
  <Context>
    <App />
  </Context>,
  rootElement
);
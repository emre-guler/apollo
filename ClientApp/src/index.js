import React from 'react';
import Home  from "./pages/Home/Home";
import ReactDOM from "react-dom";
import { BrowserRouter } from "react-router-dom";
import { Route } from "react-router";

const baseUrl = document.getElementsByTagName("base")[0].getAttribute("href");
const rootElement = document.getElementById("root");

ReactDOM.render(
  <BrowserRouter basename={baseUrl}>
    <div>
      <Route exact path="/" component={Home} />
    </div>
  </BrowserRouter>,
  rootElement
);

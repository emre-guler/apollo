import React from "react";
import ReactDOM from "react-dom";
import { BrowserRouter } from "react-router-dom";
import { Route } from "react-router";

import i18n from "i18next";
import { initReactI18next } from "react-i18next";
import LanguageDetector from "i18next-browser-languagedetector";
import HttpApi from "i18next-http-backend";

import "bootstrap/dist/css/bootstrap.min.css";

import Context from "./context";
import Home from "./pages/Home/Home";

const rootElement = document.getElementById("root");

i18n
  .use(initReactI18next)
  .use(LanguageDetector)
  .use(HttpApi)
  .init({
    fallbackLng: "en",
    detection: {
      order: ["cookie", "htmlTag", "localStorage", "path", "subdomain"],
      caches: ["cookie"],
    },
    interpolation: {
      escapeValue: false,
    },
    backend: {
      loadPath: "/assets/locals/{{lng}}.json",
    },
    react: {
      useSuspense: false,
    },
  });

const App = () => {
  return (
    <BrowserRouter>
      <Route path="/" component={Home} />
    </BrowserRouter>
  );
};

ReactDOM.render(
  <Context>
    <App />
  </Context>,
  rootElement
);
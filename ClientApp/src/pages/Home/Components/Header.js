import React, { useState } from "react";
import { Container, Row, Col, Button } from "react-bootstrap";
import { useTranslation } from "react-i18next";
import { AiOutlineUserAdd } from "react-icons/ai";
import { BiLogIn } from "react-icons/bi";
import Cookies from "js-cookie";
import "../assets/header.scss";

const Header = () => {
  const selectedLanguage = Cookies.get("i18next");
  const [language, setLanguage] = useState(selectedLanguage);
  const { t } = useTranslation();
  return (
    <header>
      <Container className="container">
        <Row>
          <Col md xl={4}>
            <img
              src={require("../assets/apolloLogo.png")}
              alt="Apollo's Logo"
            />
          </Col>
          <Col md xl={8} className={"headerContent"}>
            <div className={"languageOptions"}>
              <ul>
                <li
                  style={language === "tr" ? { opacity: 0.3 } : {}}
                  onClick={() => {
                    if (language === "en") {
                      Cookies.set("i18next", "tr");
                      setLanguage("en");
                      window.location.href = "/";
                    }
                  }}
                >
                  TR
                </li>
                <li
                  style={language === "en" ? { opacity: 0.3 } : {}}
                  onClick={() => {
                    if (language === "tr") {
                      Cookies.set("i18next", "en");
                      setLanguage("tr");
                      window.location.href = "/";
                    }
                  }}
                >
                  EN
                </li>
              </ul>
            </div>
            <div>
              <Button variant="outline-light">
                <BiLogIn />
                {t("Login")}
              </Button>
            </div>
            <div>
              <Button variant="light">
                <AiOutlineUserAdd />
                {t("Register")}
              </Button>
            </div>
          </Col>
        </Row>
      </Container>
    </header>
  );
};

export default Header;

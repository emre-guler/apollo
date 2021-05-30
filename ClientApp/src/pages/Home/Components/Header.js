import React, { useState, useLayoutEffect } from "react";
import { Container, Row, Col, Button } from "react-bootstrap";
import { useTranslation } from "react-i18next";
import { AiOutlineUserAdd, AiOutlineLogin } from "react-icons/ai";
import { FaBars } from "react-icons/fa";
import { Link } from "react-router-dom";
import Cookies from "js-cookie";
import "./assets/Header.scss";

const Header = () => {
  const [language, setLanguage] = useState();
  const { t } = useTranslation();

  useLayoutEffect(() => {
    setLanguage(Cookies.get("i18next"));
  }, []);

  return (
    <header>
      <Container>
        <Row>
          <Col xs={4} md={4} xl={4} className="logoContent">
            <Link to="/">
              <img
                src={require("./assets/apolloLogo.png")}
                alt="Apollo's Logo"
              />
            </Link>
          </Col>
          <Col xs={8} md={8} xl={8} className={"headerContent"}>
            <div className={"languageOptions"}>
              <ul>
                <li
                  style={language === "tr" ? { opacity: 0.3 } : {}}
                  onClick={() => {
                    if (language === "en") {
                      Cookies.set("i18next", "tr");
                      setLanguage("en");
                      window.location.reload();
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
                      window.location.reload();
                    }
                  }}
                >
                  EN
                </li>
              </ul>
            </div>
            <div>
              <Button variant="outline-light">
                <AiOutlineLogin />
                {t("Login")}
              </Button>
            </div>
            <div>
              <Button
                variant="outline-light"
                onClick={() => {
                  window.location.href = "/register";
                }}
              >
                <AiOutlineUserAdd />
                {t("Register")}
              </Button>
            </div>
          </Col>
          <Col xs md xl={8} className={"mobileHeaderContent"}>
            <FaBars size={28} />
          </Col>
        </Row>
      </Container>
    </header>
  );
};

export default Header;

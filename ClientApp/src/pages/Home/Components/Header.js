import React from "react";
import { Container, Row, Col, Button } from "react-bootstrap";
import { useTranslation } from "react-i18next";
import { AiOutlineUserAdd } from "react-icons/ai";
import "../assets/header.scss";

const Header = () => {
  const { t } = useTranslation();
  return (
    <header>
      <Container className="container">
        <Row>
          <Col>
            <img
              src={require("../assets/apolloLogo.png")}
              alt="Apollo's Logo"
            />
          </Col>
          <Col>
            <div className="navElementContainer">
              <Button variant="light">
                <AiOutlineUserAdd /> Kayıt Ol (Takım)
              </Button>
              <Button variant="light">
                <AiOutlineUserAdd /> {t("RegisterAsUser")}
              </Button>
            </div>
          </Col>
        </Row>
      </Container>
    </header>
  );
};

export default Header;

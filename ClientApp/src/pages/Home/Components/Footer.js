import React from "react";
import { Container, Row, Col } from "react-bootstrap";
import { useTranslation } from "react-i18next";
import "./assets/Footer.scss";

const Footer = () => {
  const { t } = useTranslation();
  return (
    <>
      <footer>
        <Container className={"footerContainer"}>
          <Row>
            <Col xs={12} md={5} xl={5}>
              <div className={"footerLeftContainer"}>
                <img src={require("./assets/apolloLogo.png")} />
                <p>{t("FooterText")}</p>
              </div>
            </Col>
            <Col xs={12} md={7} xl={7}>
                <Container>
                    <Row>
                        <Col ></Col>
                        <Col></Col>
                    </Row>
                </Container>
            </Col>
          </Row>
        </Container>
      </footer>
    </>
  );
};

export default Footer;

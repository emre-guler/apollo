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
                <img
                  src={require("./assets/apolloLogo.png")}
                  alt="Apollo logo"
                />
                <p>{t("FooterText")}</p>
              </div>
            </Col>
            <Col xs={12} md={7} xl={7}>
              <Container className={"footerRightContainer"}>
                <Row>
                  <Col xs={12} md={6} xl={6}>
                    <h5>Apollo</h5>
                    <ul>
                      <li>
                        <a href="#">{t("HomePage")}</a>
                      </li>
                      <li>
                        <a href="#">{t("Contact")}</a>
                      </li>
                      <li>
                        <a href="#">{t("AboutUs")}</a>
                      </li>
                    </ul>
                  </Col>
                  <Col xs={12} md={6} xl={6}>
                    <h5>{t("FooterTitle")}</h5>
                    <ul>
                      <li>
                        <a href="#">{t("GPDR")}</a>
                      </li>
                      <li>
                        <a href="#">{t("TermsOfService")}</a>
                      </li>
                      <li>
                        <a href="#">{t("PrivacyAgreement")}</a>
                      </li>
                    </ul>
                  </Col>
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

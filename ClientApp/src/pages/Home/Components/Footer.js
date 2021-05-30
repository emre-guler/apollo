import React from "react";
import { Container, Row, Col } from 'react-bootstrap';
import { useTranslation } from 'react-i18next';
import './assets/Footer.scss'; 

const Footer = () => {
  return (
    <>
      <footer>
          <Container>
              <Row>
                  <Col xs={12} md={5} xl={5}>
                      <div className={"footerLeftContainer"}>
                          <img src={""} />
                      </div>
                  </Col>
                  <Col xs={12} md={7} xl={7}></Col>
              </Row>
          </Container>
      </footer>
    </>
  );
};

export default Footer;
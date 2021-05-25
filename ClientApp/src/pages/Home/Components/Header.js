import React from "react";
import { Container, Row, Col } from "react-bootstrap";
import "../assets/header.scss";

const Header = () => {
  return (
    <header>
      <Container className="container">
          <Row>
            <Col>
                <img src={require('../assets/apolloLogo.png')} alt="Apollo's Logo" />
            </Col>
            <Col>
            </Col>
          </Row>
      </Container>
    </header>
  );
};

export default Header;

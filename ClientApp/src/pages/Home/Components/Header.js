import React from "react";
import { Container, Row, Col, Button } from "react-bootstrap";

import { AiOutlineUserAdd } from 'react-icons/ai';
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
              <div className="navElementContainer">
                <Button variant="light" ><AiOutlineUserAdd / > Kayıt Ol (Takım)</Button>
                <Button variant="light" ><AiOutlineUserAdd / > Kayıt Ol (Oyuncu)</Button>
              </div>
            </Col>
          </Row>
      </Container>
    </header>
  );
};

export default Header;

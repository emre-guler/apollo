import React, { useState } from "react";
import { Container, Row, Col, Button } from "react-bootstrap";
import { useTranslation } from "react-i18next";
import "./assets/Content.scss";

const Content = () => {
  const { t } = useTranslation();
  const [words, setWords] = useState(["CS:GO", "Valorant", "LOL"]);
  const [datePeriod, setDatePeriod] = useState(2000);
  return (
    <section>
      <Container>
        <Row className={"contentContainer"}>
          <Col xs={12} md={12} xl={6}>
            <div className={"mainContentContainer"}>
              <h1>{t("HeyYou")}</h1>
              <h1 className={"gameName"}>CSGO</h1>
            </div>
          </Col>
          <Col xs={12} md={12} xl={6}>
            asd
          </Col>
        </Row>
      </Container>
    </section>
  );
};

export default Content;

import React from "react";
import { Container, Row, Col, Form, Button } from "react-bootstrap";
import { useTranslation } from "react-i18next";
import { Link } from "react-router-dom";

import "./assets/RegisterTeam.scss";

const RegisterTeam = () => {
  const { t } = useTranslation();
  return (
    <section className={"registerTeamContainer"}>
      <Container>
        <Row>
          <Col xs={12} md={12} xl={12} className={"container"}>
            <div className={"registerTeamBox"}>
              <h4>{t("RegisterWelcome")}</h4>
              <span>{t("RegisterWelcomeAlt")}</span>
              <Form>
                <Form.Group controlId="formGridTeamName">
                  <Form.Label>{t("TeamName")}</Form.Label>
                  <Form.Control placeholder={t("TeamNamePlaceHolder")} />
                </Form.Group>
                <Form.Row>
                  <Form.Group as={Col} controlId="formGridEmail">
                    <Form.Label>{t("Email")}</Form.Label>
                    <Form.Control
                      type="email"
                      placeholder={t("EmailPlaceHolder")}
                    />
                  </Form.Group>
                  <Form.Group as={Col} controlId="formGridPhoneNumber">
                    <Form.Label>{t("PhoneNumber")}</Form.Label>
                    <Form.Control placeholder={t("PhoneNumberPlaceHolder")} />
                  </Form.Group>
                </Form.Row>
                <Form.Row>
                  <Form.Group as={Col} controlId="formGridPassword">
                    <Form.Label>{t("Password")}</Form.Label>
                    <Form.Control
                      type="password"
                      placeholder={t("PasswordPlaceHolder")}
                    />
                  </Form.Group>
                  <Form.Group as={Col} controlId="formGridPasswordAgain">
                    <Form.Label>{t("PasswordAgain")}</Form.Label>
                    <Form.Control
                      type="password"
                      placeholder={t("PasswordPlaceHolder")}
                    />
                  </Form.Group>
                </Form.Row>
                <Form.Group id="formGridCheckbox">
                  <Form.Check type="checkbox" label={t("AccepContracts")} />
                  <br />
                  <Button variant="outline-dark" type="submit">
                    {t("Register")}
                  </Button>
                </Form.Group>
              </Form>
              <hr />
              <div style={{ textAlign: "left", marginLeft: 25 }}>
                <span>
                  {t("DoYouHaveAccount")} <Link to="/">{t("Login")}</Link>{" "}
                </span>
              </div>
            </div>
          </Col>
        </Row>
      </Container>
    </section>
  );
};

export default RegisterTeam;

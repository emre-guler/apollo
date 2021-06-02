import React, { useRef, useContext } from "react";
import { Container, Row, Col, Form, Button } from "react-bootstrap";
import { useTranslation } from "react-i18next";
import { Link } from "react-router-dom";
import { useToasts } from "react-toast-notifications";
import { DomainContext }  from "../../../context";
import ErrorHandler from '../../../Methods/ErrorHandler';
import axios from "axios";

import "./assets/RegisterTeam.scss";

const RegisterTeam = () => {
  const { t } = useTranslation();

  const [domain, setDomain] = useContext(DomainContext);

  const teamNameRef = useRef();
  const eMailRef = useRef();
  const phoneNumberRef = useRef();
  const passwordRef = useRef();
  const passwordAgainRef = useRef();
  const checkBoxRef = useRef();

  const { addToast } = useToasts();

  const SendRegisterRequest = () => {
    const emailRegex = new RegExp(
      "^([\\w\\.\\-]+)@([\\w\\-]+)((\\.(\\w){2,3})+)$"
    );
    const phoneRegex = new RegExp("\\(?\\d{3}\\)?-? *\\d{3}-? *-?\\d{4}");

    if (teamNameRef.current.value.length === 0) {
      addToast(t("TeamNameMustBeFilled"), {
        appearance: "error",
        autoDismiss: true,
      });
    } else if (
      eMailRef.current.value.length === 0 ||
      !emailRegex.test(eMailRef.current.value)
    ) {
      addToast(t("MailMustBeFilled"), {
        appearance: "error",
        autoDismiss: true,
      });
    } else if (
      phoneNumberRef.current.value.length === 0 ||
      !phoneRegex.test(phoneNumberRef.current.value)
    ) {
      addToast(t("PhoneNumberMustBeFilled"), {
        appearance: "error",
        autoDismiss: true,
      });
    } else if (passwordRef.current.value.length < 7) {
      addToast(t("PasswordMustBeFilled"), {
        appearance: "error",
        autoDismiss: true,
      });
    } else if (passwordAgainRef.current.value.length === 0) {
      addToast(t("PasswordMustBeFilled"), {
        appearance: "error",
        autoDismiss: true,
      });
    } else if (passwordRef.current.value !== passwordAgainRef.current.value) {
      addToast(t("MustBeSamePassword"), {
        appearance: "error",
        autoDismiss: true,
      });
    } else if (!checkBoxRef.current.checked) {
      addToast(t("MustBeChecked"), {
        appearance: "error",
        autoDismiss: true,
      });
    } else {
      const requestUrl = domain + "team-register";
      axios
        .post(requestUrl)
        .then((response) => {

        })
        .catch((error) => {
          const response = new ErrorHandler().handler(error);
          addToast(response, {
            appearance: "error",
            autoDismiss: true
          });
        });
    }
  };
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
                  <Form.Control
                    ref={teamNameRef}
                    placeholder={t("TeamNamePlaceHolder")}
                  />
                </Form.Group>
                <Form.Row>
                  <Form.Group as={Col} controlId="formGridEmail">
                    <Form.Label>{t("Email")}</Form.Label>
                    <Form.Control
                      type="email"
                      ref={eMailRef}
                      placeholder={t("EmailPlaceHolder")}
                    />
                  </Form.Group>
                  <Form.Group as={Col} controlId="formGridPhoneNumber">
                    <Form.Label>{t("PhoneNumber")}</Form.Label>
                    <Form.Control
                      ref={phoneNumberRef}
                      placeholder={t("PhoneNumberPlaceHolder")}
                    />
                  </Form.Group>
                </Form.Row>
                <Form.Row>
                  <Form.Group as={Col} controlId="formGridPassword">
                    <Form.Label>{t("Password")}</Form.Label>
                    <Form.Control
                      type="password"
                      ref={passwordRef}
                      placeholder={t("PasswordPlaceHolder")}
                    />
                  </Form.Group>
                  <Form.Group as={Col} controlId="formGridPasswordAgain">
                    <Form.Label>{t("PasswordAgain")}</Form.Label>
                    <Form.Control
                      type="password"
                      ref={passwordAgainRef}
                      placeholder={t("PasswordPlaceHolder")}
                    />
                  </Form.Group>
                </Form.Row>
                <Form.Group id="formGridCheckbox">
                  <Form.Check
                    type="checkbox"
                    label={t("AccepContracts")}
                    ref={checkBoxRef}
                  />
                  <br />
                  <Button
                    variant="outline-dark"
                    type="button"
                    onClick={SendRegisterRequest}
                  >
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

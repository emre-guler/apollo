import React, {
  useState,
  useContext,
  useEffect,
  useLayoutEffect,
  useRef,
} from "react";
import { Container, Row, Col, Form, Button } from "react-bootstrap";
import { Link } from "react-router-dom";
import { DomainContext } from "../../../context";
import { useTranslation } from "react-i18next";
import { useToasts } from "react-toast-notifications";

import ErrorHandler from "../../../Methods/ErrorHandler";
import Gender from "../../../Enums/Gender";
import axios from "axios";

import "./assets/Register.scss";

const RegisterPlayer = () => {
  const [cities, setCities] = useState();

  const { t } = useTranslation();

  const [domain, setDomain] = useContext(DomainContext);

  const { addToast } = useToasts();

  const playerName = useRef();
  const playerSurname = useRef();
  const playerNickName = useRef();
  const playerPhoneNumber = useRef();
  const playerMailAddress = useRef();
  const playerPassword = useRef();
  const playerPasswordAgain = useRef();
  const playerCity = useRef();
  const playerBirthDate = useRef();
  const playerGender = useRef();
  const checkBox = useRef();

  useLayoutEffect(() => {
    GetCities();
  }, []);

  const GetCities = () => {
    const requestUrl = domain + "get-cities";
    axios
      .get(requestUrl)
      .then((response) => {
        const citiyOptions = response.data.map((city) => {
          return <option value={city.Id}>{city.Name}</option>;
        });
        setCities(citiyOptions);
      })
      .catch((error) => {
        addToast(t("SomethingWentWrong"), {
          appearance: "error",
          autoDismiss: true,
        });
      });
  };

  const SendRegisterRequest = () => {
    const emailRegex = new RegExp(
      "^([\\w\\.\\-]+)@([\\w\\-]+)((\\.(\\w){2,3})+)$"
    );
    const phoneRegex = new RegExp("\\(?\\d{3}\\)?-? *\\d{3}-? *-?\\d{4}");

    if (playerNickName.current.value.length === 0) {
      addToast(t("PlayerNicknameMustBeFilled"), {
        appearance: "error",
        autoDismiss: true,
      });
    } else if (playerName.current.value.length === 0) {
      addToast(t("PlayerNameMustBeFilled"), {
        appearance: "error",
        autoDismiss: true,
      });
    } else if (playerSurname.current.value.length === 0) {
      addToast(t("PlayerSurnameMustBeFilled"), {
        appearance: "error",
        autoDismiss: true,
      });
    } else if (
      playerMailAddress.current.value.length === 0 ||
      !emailRegex.test(playerMailAddress.current.value)
    ) {
      addToast(t("MailMustBeFilled"), {
        appearance: "error",
        autoDismiss: true,
      });
    } else if (
      playerPhoneNumber.current.value.length === 0 ||
      !phoneRegex.test(playerPhoneNumber.current.value)
    ) {
      addToast(t("PhoneNumberMustBeFilled"), {
        appearance: "error",
        autoDismiss: true,
      });
    } else if (playerPassword.current.value.length < 7) {
      addToast(t("PasswordMustBeFilled"), {
        appearance: "error",
        autoDismiss: true,
      });
    } else if (playerPasswordAgain.current.value.length < 7) {
      addToast(t("PasswordMustBeFilled"), {
        appearance: "error",
        autoDismiss: true,
      });
    } else if (
      playerPassword.current.value !== playerPasswordAgain.current.value
    ) {
      addToast(t("MustBeSamePassword"), {
        appearance: "error",
        autoDismiss: true,
      });
    } else if (playerGender.current.value.length === 0) {
      addToast(t("GenderMustBeSelected"), {
        appearance: "error",
        autoDismiss: true,
      });
    } else if (playerBirthDate.current.value === "") {
      addToast(t("BirthDateMustBeSelected"), {
        appearance: "error",
        autoDismiss: true,
      });
    } else if (playerCity.current.value.length === 0) {
      addToast(t("CityMustBeSelected"), {
        appearance: "error",
        autoDismiss: true,
      });
    } else if (!checkBox.current.checked) {
      addToast(t("MustBeChecked"), {
        appearance: "error",
        autoDismiss: true,
      });
    } else {
      const requestUrl = domain + "player-register";
      let formData = new FormData();
      formData.append("Name", playerName.current.value);
      formData.append("Surname", playerSurname.current.value);
      formData.append("Nickname", playerNickName.current.value);
      formData.append("PhoneNumber", playerPhoneNumber.current.value);
      formData.append("MailAddress", playerMailAddress.current.value);
      formData.append("Password", playerPassword.current.value);
      formData.append("CityId", playerCity.current.value);
      formData.append("BirthDate", playerBirthDate.current.value);
      formData.append("Gender", playerGender.current.value);

      axios
        .post(requestUrl, formData, {
          headers: {
            "content-type": "multipart/form-data",
          },
        })
        .then((response) => {
          if (response.status === 200) {
            if (response.data == true) {
              addToast(t("RegisterSuccess"), {
                appearance: "success",
                autoDismiss: true,
              });
            } else {
              addToast(t(ErrorHandler(response.data)), {
                appearance: "error",
                autoDismiss: true,
              });
            }
          } else {
            addToast(t("SomethingWentWrong"), {
              appearance: "error",
              autoDismiss: true,
            });
          }
        })
        .catch((error) => {
          addToast(t("SomethingWentWrong"), {
            appearance: "error",
            autoDismiss: true,
          });
        });
    }
  };

  return (
    <section className={"registerContainer"}>
      <Container>
        <Row>
          <Col xs={12} md={12} xl={12} className={"container"}>
            <div className={"registerBox"}>
              <h4>{t("RegisterWelcome")}</h4>
              <span>{t("RegisterWelcomeAlt")}</span>
              <Form>
                <Form.Group controlId="formGridNickname">
                  <Form.Label>{t("Nickname")}</Form.Label>
                  <Form.Control
                    ref={playerNickName}
                    placeholder={t("NicknamePlaceHolder")}
                  />
                </Form.Group>
                <Form.Row>
                  <Form.Group as={Col} controlId="formGridPlayerName">
                    <Form.Label>{t("Name")}</Form.Label>
                    <Form.Control
                      ref={playerName}
                      placeholder={t("NamePlaceHolder")}
                    />
                  </Form.Group>
                  <Form.Group as={Col} controlId="formGridPlayerSurname">
                    <Form.Label>{t("Surname")}</Form.Label>
                    <Form.Control
                      ref={playerSurname}
                      placeholder={t("SurnamePlaceHolder")}
                    />
                  </Form.Group>
                </Form.Row>
                <Form.Row>
                  <Form.Group as={Col} controlId="formGridEmail">
                    <Form.Label>{t("Email")}</Form.Label>
                    <Form.Control
                      type="email"
                      ref={playerMailAddress}
                      placeholder={t("EmailPlaceHolder")}
                    />
                  </Form.Group>
                  <Form.Group as={Col} controlId="formGridPhoneNumber">
                    <Form.Label>{t("PhoneNumber")}</Form.Label>
                    <Form.Control
                      ref={playerPhoneNumber}
                      placeholder={t("PhoneNumberPlaceHolder")}
                    />
                  </Form.Group>
                </Form.Row>
                <Form.Row>
                  <Form.Group as={Col} controlId="formGridPassword">
                    <Form.Label>{t("Password")}</Form.Label>
                    <Form.Control
                      type="password"
                      ref={playerPassword}
                      placeholder={t("PasswordPlaceHolder")}
                    />
                  </Form.Group>
                  <Form.Group as={Col} controlId="formGridPasswordAgain">
                    <Form.Label>{t("PasswordAgain")}</Form.Label>
                    <Form.Control
                      type="password"
                      ref={playerPasswordAgain}
                      placeholder={t("PasswordPlaceHolder")}
                    />
                  </Form.Group>
                </Form.Row>
                <Form.Row>
                  <Form.Group as={Col} controlId="formGridGender">
                    <Form.Label>{t("Gender")}</Form.Label>
                    <Form.Control as="select" ref={playerGender}>
                      <option value={""}>{t("GenderPlaceHolder")}</option>
                      <option value={Gender.Man}>{t("Man")}</option>
                      <option value={Gender.Woman}>{t("Woman")}</option>
                    </Form.Control>
                  </Form.Group>
                  <Form.Group as={Col} controlId="formGridBirthDate">
                    <Form.Label>{t("BirthDate")}</Form.Label>
                    <Form.Control
                      type="date"
                      ref={playerBirthDate}
                    ></Form.Control>
                  </Form.Group>
                </Form.Row>
                <Form.Group controlId="formGridCity">
                  <Form.Label>{t("City")}</Form.Label>
                  <Form.Control as="select" ref={playerCity}>
                    <option value={""}>{t("CityPlaceHolder")}</option>
                    {cities}
                  </Form.Control>
                </Form.Group>
                <Form.Group controlId="formGridCheckbox" id="formGridCheckbox">
                  <Form.Check
                    type="checkbox"
                    label={t("AcceptContracts")}
                    ref={checkBox}
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
                  {t("DoYouHaveAccount")} <Link to="/">{t("Login")}</Link>
                </span>
              </div>
            </div>
          </Col>
        </Row>
      </Container>
    </section>
  );
};

export default RegisterPlayer;

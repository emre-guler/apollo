import React, { useState } from "react";

import Header from "../Home/Components/Header";
import Footer from "../Home/Components/Footer";

import RegisterPlayer from "./Components/RegisterPlayer";
import RegisterTeam from "./Components/RegisterTeam";

import { Container, Row, Col, Button } from "react-bootstrap";
import { useTranslation } from "react-i18next";
import { Link } from "react-router-dom";

import "./Components/assets/SelectUserType.scss";

const Register = () => {
  const { t } = useTranslation();
  const userTypeEnum = Object.freeze({
    None: 0,
    Player: 1,
    Team: 2,
  });
  const [userType, setUserType] = useState(userTypeEnum.None);
  const renderRegister = () => {
    if (userTypeEnum.Team === userType) {
      return (
        <>
          <RegisterTeam />
        </>
      );
    } else if (userTypeEnum.Player === userType) {
      return (
        <>
          <RegisterPlayer />
        </>
      );
    } else {
      return (
        <>
          <section className={"selectUserTypeContainer"}>
            <Container>
              <Row>
                <Col xs={12} md={6} xl={6} className={"container"}>
                  <div className={"selectUserTypeBox"}>
                    <h4>{t("SelectUserTypeTitle")}</h4>
                    <p>{t("SelectUserTypeContent")}</p>
                    <div className={"registerButtonContainer"}>
                      <Button
                        variant="outline-dark"
                        onClick={() => setUserType(userTypeEnum.Player)}
                      >
                        {t("RegisterAsPlayer")}
                      </Button>
                      <Button
                        variant="outline-dark"
                        onClick={() => setUserType(userTypeEnum.Team)}
                      >
                        {t("RegisterAsTeam")}
                      </Button>
                    </div>
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
        </>
      );
    }
  };
  return (
    <>
      <Header />
      {renderRegister()}
      <Footer />
    </>
  );
};

export default Register;

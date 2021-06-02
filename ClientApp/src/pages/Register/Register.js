import React, { useState } from "react";

import Header from "../Home/Components/Header";
import Footer from "../Home/Components/Footer";

import RegisterPlayer from "./Components/RegisterPlayer";
import RegisterTeam from "./Components/RegisterTeam";

import { Container, Row, Col, Button } from "react-bootstrap";
import { useTranslation } from "react-i18next";
import { Link } from "react-router-dom";
import UserType from "../../Enums/UserType";

import "./Components/assets/SelectUserType.scss";

const Register = () => {
  const { t } = useTranslation();
  
  const [userType, setUserType] = useState(UserType.None);
  const renderRegister = () => {
    if (UserType.Team === userType) {
      return (
        <>
          <RegisterTeam />
        </>
      );
    } else if (UserType.Player === userType) {
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
                        onClick={() => setUserType(UserType.Player)}
                      >
                        {t("RegisterAsPlayer")}
                      </Button>
                      <Button
                        variant="outline-dark"
                        onClick={() => setUserType(UserType.Team)}
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

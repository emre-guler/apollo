import React, { useState } from "react";
import { Container, Row, Col, Card } from "react-bootstrap";
import { useTranslation } from "react-i18next";
import Typed from "react-typed";
import AwesomeSlider from "react-awesome-slider";
import withAutoplay from "react-awesome-slider/dist/autoplay";
import "./assets/Content.scss";
import "react-awesome-slider/dist/styles.css";

const Content = () => {
  const { t } = useTranslation();
  const AutoplaySlider = withAutoplay(AwesomeSlider);
  const [words] = useState(["CS:GO", "Valorant", "LOL"]);
  return (
    <>
      <section>
        <Container>
          <Row className={"contentContainer"}>
            <Col xs={12} md={12} xl={6}>
              <div className={"mainContentContainer"}>
                <h1>{t("HeyYou")}</h1>
                <Typed
                  strings={words}
                  typeSpeed={100}
                  backSpeed={100}
                  loop
                  className={"gameName"}
                />
                <h1 className={"altMessage"}>{t("TeamsAreHere")}</h1>
                <h1 className={"altMessage"}>{t("WannaToJoinUs")}</h1>
              </div>
            </Col>
            <Col xs={12} md={12} xl={6}>
              <div className={"bannerContentContainer"}>
                <AutoplaySlider
                  play={true}
                  cancelOnInteraction={false}
                  organicArrows={false}
                  interval={2000}
                  bullets={false}
                >
                  <div data-src="/assets/teamLogosCSGO.png" />
                  <div data-src="/assets/teamLogosLOL.png" />
                  <div data-src="/assets/teamLogosValorant.png" />
                </AutoplaySlider>
              </div>
            </Col>
          </Row>
        </Container>
      </section>
      <section className={"secondSection"}>
        <Container>
          <Row>
            <Col xs={12} md={12} xl={12}>
              <div className={"altContentContainer"}>
                <div>
                  <h3>{t("HowIsItWork")}</h3>
                </div>
                <div className={"listContainer"}>
                  <div>
                    <Card style={{ width: "18rem" }}>
                      <Card.Body>
                        <Card.Title>{t("RegisterApollo")}</Card.Title>
                        <Card.Text>{t("RegisterToApolloContent")}</Card.Text>
                      </Card.Body>
                    </Card>
                  </div>
                  <div>
                    <Card style={{ width: "18rem" }}>
                      <Card.Body>
                        <Card.Title>{t("ShowYourSkills")}</Card.Title>
                        <Card.Text>{t("ShowYourSkillsContent")}</Card.Text>
                      </Card.Body>
                    </Card>
                  </div>
                  <div>
                    <Card style={{ width: "18rem" }}>
                      <Card.Body>
                        <Card.Title>{t("FindTeam")}</Card.Title>
                        <Card.Text>{t("FindTeamContent")}</Card.Text>
                      </Card.Body>
                    </Card>
                  </div>
                </div>
              </div>
            </Col>
          </Row>
        </Container>
      </section>
      <section className={"thirdSection"}>
        <Container>
          <Row>
            <Col xs={12} md={12} xl={12}>
              <div className={"altContentContainer"}>
                <div>
                  <h3 className={"lightTitle"}>{t("AltTitle")}</h3>
                </div>
                <div className={"paragraphContainer"}>
                  <p>{t("AltContent")}</p>
                  <br />
                  <p>{t("AltContent")}</p>
                </div>
              </div>
            </Col>
          </Row>
        </Container>
      </section>
    </>
  );
};

export default Content;

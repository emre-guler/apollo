import React, { useState } from "react";
import { Container, Row, Col } from "react-bootstrap";
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
    </>
  );
};

export default Content;

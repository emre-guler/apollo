import React, { useContext, useRef } from "react";
import { Container, Row, Col, Form, Button } from "react-bootstrap";
import { Link } from "react-router-dom";
import { DomainContext } from "../../../context";
import { useTranslation } from "react-i18next";
import { useToasts } from "react-toast-notifications";

import ErrorHandler from "../../../Methods/ErrorHandler";
import axios from "axios";

import "./assets/Register.scss";

const RegisterPlayer = () => {
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

  const SendRegisterRequest = () => {
    const emailRegex = new RegExp(
      "^([\\w\\.\\-]+)@([\\w\\-]+)((\\.(\\w){2,3})+)$"
    );
    const phoneRegex = new RegExp("\\(?\\d{3}\\)?-? *\\d{3}-? *-?\\d{4}");

    if (playerName.current.value.length === 0) {
      addToast(t("PlayerNameMustBeFilled"), {
        appearance: "error",
        autoDismiss: true,
      });
    } else if (playerSurname.current.value.length === 0) {
      addToast(t("PlayerSurnameMustBeFilled"), {
        appearance: "error",
        autoDismiss: true,
      });
    } else if (playerNickName.current.value.length === 0) {
      addToast(t("PlayerNicknameMustBeFilled"), {
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
    } else if (
      playerMailAddress.current.value.length === 0 ||
      !emailRegex.test(playerMailAddress.current.value)
    ) {
      addToast(t("MailMustBeFilled"), {
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
    } else if (playerCity.current.value.length === 0) {
      addToast(t("CityMustBeSelected"), {
        appearance: "error",
        autoDismiss: true,
      });
    } else if (playerBirthDate.current.value.length === 0) {
      addToast(t("BirthDateMustBeSelected"), {
        appearance: "error",
        autoDismiss: true,
      });
    } else if (playerGender.current.value.length === 0) {
      addToast(t("GenderMustBeSelected"), {
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
                appearance: "error",
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

  return <div></div>;
};

export default RegisterPlayer;

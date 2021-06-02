import ErrorCode from "../Enums/ErrorCode";
import { useTranslation } from "react-i18next";

const ErrorHandler = (errorCode) => {
  if (ErrorCode.UserExists == errorCode) {
    return "UserExists";
  } else if (ErrorCode.MustBeFilled == errorCode) {
    return "MustBeFilled";
  } else if (ErrorCode.InvalidCredentials == errorCode) {
    return "InvalidCredentials"
  } else if (ErrorCode.Unauthorized == errorCode) {
    return "Unauthorized";
  } else if (ErrorCode.InvalidCode == errorCode) {
    return "InvalidCode";
  } else if (ErrorCode.LinkExpired == errorCode) {
    return "LinkExpired";
  } else if (ErrorCode.UserNotFind == errorCode) {
    return "UserNotFind";
  } else if (ErrorCode.NotFound == errorCode) {
    return "NotFound";
  } else if (ErrorCode.InternalServer == errorCode) {
    return "SomethingWentWrong";
  } else {
    return "SomethingWentWrong";
  }
};

export default ErrorHandler;

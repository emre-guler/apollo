import ErrorCode from "../Enums/ErrorCode";
import { useTranslation } from "react-i18next";

export default class ErroHandler {
  handler = (errorCode) => {
    const { t } = useTranslation();

    if (ErrorCode.UserExists == errorCode) {
      return(t("UserExists"));
    }
    else if(ErrorCode.MustBeFilled == errorCode)
    {
        return(t("MustBeFilled"));
    }
    else if(ErrorCode.InvalidCredentials == errorCode)
    {
        return(t("InvalidCredentials"))
    }
    else if(ErrorCode.Unauthorized == errorCode)
    {
        return(t("Unauthorized"));   
    }
    else if(ErrorCode.InvalidCode == errorCode) 
    {
        return(t("InvalidCode"));
    }
    else if(ErrorCode.LinkExpired == errorCode) 
    {
        return(t("LinkExpired"));
    }
    else if(ErrorCode.UserNotFind == errorCode)
    {
        return(t("UserNotFind"));
    }
    else if(ErrorCode.NotFound == errorCode)
    {
        return(t("NotFound"));
    }
    else if(ErrorCode.InternalServer == errorCode)
    {
        return(t("SomethingWentWrong"));
    }
    else 
    {
        return(t("SomethingWentWrong"));
    }
  };
}

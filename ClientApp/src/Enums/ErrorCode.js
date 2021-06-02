const ErrorCode = Object.freeze({
  UserExists: 100,
  MustBeFilled: 101,
  InvalidCredentials: 102,
  Unauthorized: 103,
  InvalidCode: 104,
  LinkExpired: 105,
  UserNotFind: 106,
  Forbidden: 403,
  NotFound: 400,
  InternalServer: 500,
});

export default ErrorCode;

import ExtraError from "./Error";
export const MyErrorFallback = ({ error, resetErrorBoundary }) => {
  return <ExtraError />;
};
export const logErrorToMyService = (error, info) => {
  console.error(error, info.componentStack);
};

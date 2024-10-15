import React from "react";
import { Route, Navigate, Routes } from "react-router-dom";
import { isAuthenticated } from "../../Utilities/remote/auth";

// eslint-disable-next-line react/prop-types
export const PrivateRoute = ({ children, ...rest }) => {
  return isAuthenticated() ? children : <Navigate to="/login" />;
};

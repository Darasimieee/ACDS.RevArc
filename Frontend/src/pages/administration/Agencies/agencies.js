import React from "react";
import { Link } from "react-router-dom";

const Agencies = () => {
  return (
    <>
      <ol className="breadcrumb float-xl-end">
        <li className="breadcrumb-item">
          <Link to="/dashboard">Home</Link>
        </li>
        <li className="breadcrumb-item">Administration</li>

        <li className="breadcrumb-item active">Agencies </li>
      </ol>
      <h1 className="page-header mb-3">Agencies</h1>
      <hr />
    </>
  );
};

export default Agencies;

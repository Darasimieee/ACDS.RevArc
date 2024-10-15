import React from "react";
import { Link } from "react-router-dom";

const BusinessSize = () => {
  return (
    <>
      <ol className="breadcrumb float-xl-end">
        <li className="breadcrumb-item">
          <Link to="/dashboard">Home</Link>
        </li>
        <li className="breadcrumb-item">Administration</li>

        <li className="breadcrumb-item active">Business Size </li>
      </ol>
      <h1 className="page-header mb-3">Business Size</h1>
      <hr />
    </>
  );
};

export default BusinessSize;

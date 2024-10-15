import React from "react";
import { Link } from "react-router-dom";

const Revenues = () => {
  return (
    <>
      <ol className="breadcrumb float-xl-end">
        <li className="breadcrumb-item">
          <Link to="/dashboard">Home</Link>
        </li>
        <li className="breadcrumb-item">Administration</li>

        <li className="breadcrumb-item active">Revenues </li>
      </ol>
      <h1 className="page-header mb-3">Revenues</h1>
      <hr />
    </>
  );
};

export default Revenues;

import React, { useState, useEffect } from "react";
import api from "../../axios/custom";
import { Link } from "react-router-dom";
import { useSelector } from "react-redux";
import CryptoJS from "crypto-js";

const BillingList = () => {
  const token = localStorage.getItem("myToken");
  const [data, setData] = useState([]);
  // Function to decrypt the ID
  const decryptID = (encryptedID) => {
    const bytes = CryptoJS.AES.decrypt(encryptedID, "secret key");
    const decryptedID = bytes.toString(CryptoJS.enc.Utf8);
    return decryptedID;
  };
  // Get and decrypt Organisation ID from local storage
  const storedEncryptedID = localStorage.getItem("organisationId");
  const organisationId = decryptID(storedEncryptedID);

  const handleEdit = (id) => {
    // Handle edit button click
    console.log(`Edit button clicked for item with ID ${id}`);
  };

  const handleView = (id) => {
    // Handle view button click
    console.log(`View button clicked for item with ID ${id}`);
  };
  //api to get table data
  useEffect(() => {
    // Fetch data from API and update state
  }, []);
  return (
    <>
      <ol className="breadcrumb float-xl-end">
        <li className="breadcrumb-item">
          <Link to="/dashboard">Home</Link>
        </li>
        <li className="breadcrumb-item">Billing</li>
        <li className="breadcrumb-item active">Billing List </li>
      </ol>
      <h1 className="page-header mb-3">Billing List</h1>
      <hr />
      <div className="d-flex flex-row-reverse">
        <Link
          to="generatenewbill"
          className="btn bg-blue-900 shadow-md text-white px-4"
        >
          Generate New Bill
        </Link>
      </div>

      <table>
        <thead>
          <tr>
            <th>S/N</th>
            <th>Property</th>
            <th>Customer Name</th>
            <th>Agency</th>
            <th>Revenue</th>
            <th>Category</th>
            <th>Amount</th>
            <th>Action</th>
          </tr>
        </thead>
        <tbody>
          {data.map((item) => (
            <tr key={item.id}>
              <td>{item.id}</td>
              <td>{item.propertyName}</td>
              <td>{item.customerName}</td>
              <td>{item.payerID}</td>
              <td>{item.payerID}</td>
              <td>{item.payerID}</td>
              <td>{item.payerID}</td>
              <td>
                <Link
                  className="text-decoration-underline"
                  onClick={() => handleEdit(item.id)}
                >
                  View Details
                </Link>
                <br />
                <Link
                  to="print-manifest-slip"
                  className="text-decoration-underline"
                  onClick={() => handleView(item.id)}
                >
                  Print Slip
                </Link>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </>
  );
};

export default BillingList;

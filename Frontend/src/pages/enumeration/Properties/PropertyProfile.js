import React, { useState, useEffect, useMemo } from "react";
import DateRangePicker from "react-bootstrap-daterangepicker";
import { Link, useNavigate } from "react-router-dom";
import DataTable from "react-data-table-component";
import { useSelector } from "react-redux";
import api from "../../../axios/custom";
import { Spinner } from "react-activity";
import "react-activity/dist/library.css";
import CryptoJS from "crypto-js";
import FilterComponent from "../../../components/filter component/filtercomponent";

const columns = [
  // {
  //   name: "S/N",
  //   selector: (row) => row.,
  //   sortable: true,
  // },
  {
    name: "Building Name",
    selector: (row) => row.buildingName,
    sortable: true,
  },
  {
    name: "Location Address",
    selector: (row) => row.locationAddress,
    sortable: true,
  },
  {
    name: "Space Identifier",
    selector: (row) => row.spaceIdentifier.spaceIdentifierName,
    sortable: true,
  },
  {
    name: "Space Floor",
    selector: (row) => row.spaceFloor,
    sortable: true,
  },
  // {
  //   name: "State ",
  //   selector: (row) => row.stateId,
  //   sortable: true,
  // },
  {
    name: "Ward",
    selector: (row) => row.ward.wardName,
    sortable: true,
  },
  // {
  //   name: "LGA",
  //   selector: (row) => row.lgaId,
  //   sortable: true,
  // },
  {
    name: "",
    width: "63px",
    cell: (row) => (
      <Link
        to={`../../enumeration/editproperty/${row.propertyId}`}
        className="btn btn-sm btn-primary"
      >
        Edit
      </Link>
    ),
  },
  {
    width: "67px",
    sortable: false,
    cell: (row) => (
      <button
        data-bs-toggle="modal"
        data-bs-target="#modalAlert"
        className="btn btn-sm btn-info"
        // onClick={() => {
        //   ViewRow(row);
        // }}
      >
        View
      </button>
    ),
  },
];

const PropertyProfile = () => {
  const token = localStorage.getItem("myToken");
  const [filterText, setFilterText] = React.useState("");
  const [resetPaginationToggle, setResetPaginationToggle] = useState(false);
  const [data, setData] = useState([]);
  console.log("dataaaa", data);
  const [editingRowId, setEditingRowId] = useState(null);
  const [pending, setPending] = React.useState(true);
  const [startDate, setStartDate] = useState(null);
  const [endDate, setEndDate] = useState(null);

  const handleEdit = (id) => {
    // Handle edit button click

    console.log(id.propertyId);
  };

  const handleView = (id) => {
    // Handle view button click
    console.log(`View button clicked for item with ID ${id.propertyId}`);
  };
  // Function to decrypt IDs
  const decryptID = (encryptedID) => {
    const bytes = CryptoJS.AES.decrypt(encryptedID, "secret key");
    const decryptedID = bytes.toString(CryptoJS.enc.Utf8);
    return decryptedID;
  };
  // Get and decrypt Organisation ID from local storage
  const storedOrganisationID = localStorage.getItem("organisationId");
  const organisationId = decryptID(storedOrganisationID);

  const storedUserProfileID = localStorage.getItem("userProfileId");
  const userProfileId = decryptID(storedUserProfileID);

  const filteredItems = data.filter(
    (item) =>
      item.buildingName &&
      item.buildingName.toLowerCase().includes(filterText.toLowerCase())
  );

  const subHeaderComponentMemo = React.useMemo(() => {
    const handleClear = () => {
      if (filterText) {
        setResetPaginationToggle(!resetPaginationToggle);
        setFilterText("");
      }
    };

    return (
      <FilterComponent
        onFilter={(e) => setFilterText(e.target.value)}
        onClear={handleClear}
        filterText={filterText}
      />
    );
  }, [filterText, resetPaginationToggle]);
  React.useEffect(() => {
    const timeout = setTimeout(() => {
      setPending(false);
    }, 1500);
    return () => clearTimeout(timeout);
  }, []);

  useEffect(() => {
    api
      .get(`enumeration/${organisationId}/property`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      })
      .then((response) => {
        const paginationHeader = response.headers.get("X-Pagination");
        // console.log("headersssssSSS", paginationHeader);
        setData(response.data);
      })
      .catch((error) => {
        console.log(error);
      });
  }, []);
  return (
    <>
      <div>
        <ol className="breadcrumb float-xl-end">
          <li className="breadcrumb-item">
            <Link to="/home/Dashboard">Home</Link>
          </li>
          <li className="breadcrumb-item">Enumeration</li>
          <li className="breadcrumb-item active">Property </li>
        </ol>
        <h1 className="page-header mb-3">Property Enumeration</h1>
        <hr />

        <div className="d-flex justify-content-between my-3">
          <div className=" d-flex align-items-end">
            <form>
              <div className="d-flex  flex-column">
                <h4 className="mb-2 page-header"> Filter By:</h4>
                <div className="d-flex justify-content-between">
                  <div className="form-check-inline ">
                    <select className="form-select">
                      <option>All Ward</option>
                    </select>
                  </div>
                  <div className=" form-check-inline">
                    <select className="form-select">
                      <option>All Address</option>
                    </select>
                  </div>
                  <div className=" form-check-inline ">
                    <select className="form-select ">
                      <option>All Area Office</option>
                    </select>
                  </div>
                  <div className=" form-check-inline">
                    <DateRangePicker>
                      <button
                        type="button"
                        className="btn btn-white border me-2 text-truncate"
                      >
                        <i className="fa fa-calendar fa-fw text-white text-opacity-50 ms-n1 me-1"></i>
                        <span>Date</span>
                        <b className="caret ms-1 opacity-5"></b>
                      </button>
                    </DateRangePicker>
                  </div>
                </div>
              </div>
            </form>
          </div>
          <div>
            <Link
              className="btn btn-sm shadow-md bg-blue-900 p-3 text-white"
              to="../newPropertyProfile"
            >
              START NEW ENUMERATION
            </Link>
          </div>
        </div>

        {/* <DataTable
          columns={columns}
          data={filteredItems}
          onClick={(item) => console.log(item)}
          pagination
          loading
          progressPending={pending}
          paginationResetDefaultPage={resetPaginationToggle} // optionally, a hook to reset pagination to page 1
          subHeader
          subHeaderComponent={subHeaderComponentMemo}
          //actions={<button className=" btn btn-sm btn-info">Download</button>}
        /> */}
        <div className="table-responsive">
          <table className="table table-bordered">
            <thead>
              <tr>
                <th>S/N</th>
                <th>Building Name</th>
                <th>Location Address</th>
                <th>Space Identifier</th>
                <th>Space floor</th>
                <th>Ward Name</th>
                <th>Action</th>
                <th>Continue Enumeration</th>
              </tr>
            </thead>
            <tbody>
              {filteredItems.map((item) => (
                <tr key={item.propertyId}>
                  <td>{item.propertyId}</td>
                  <td>{item.buildingName}</td>
                  <td>{item.locationAddress}</td>
                  <td>{item.spaceIdentifier.spaceIdentifierName}</td>
                  <td>{item.spaceFloor}</td>
                  <td>{item.ward.wardName}</td>
                  <td>
                    <button
                      data-bs-toggle="modal"
                      data-bs-target="#editDialog"
                      className="text-decoration-underline text-blue-900"
                      onClick={() => handleEdit(item)}
                    >
                      Edit
                    </button>
                    <br />
                    <button
                      data-bs-toggle="modal"
                      data-bs-target="#viewDialog"
                      className="text-decoration-underline text-blue-900"
                      onClick={() => handleView(item)}
                    >
                      View
                    </button>
                    <br />
                  </td>
                  <td>
                    <button
                      data-bs-toggle="modal"
                      data-bs-target="#viewDialog"
                      className="btn btn-sm shadow-md bg-blue-900 text-white"
                      onClick={() => handleView(item)}
                    >
                      Continue Enumeration
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
          <div className="modal fade" id="viewDialog">
            <div className="modal-dialog">
              <div className="modal-header">View Details</div>
              <div className="modal-body">...</div>
              <div className="modal-footer">
                <button className="btn btn-white" data-bs-dismiss="modal">
                  Close
                </button>
                <button className="btn btn-success">Action</button>
              </div>
            </div>
          </div>
          <div className="modal fade" id="editDialog">
            <div className="modal-dialog">
              <div className="modal-header">Edit Details</div>
              <div className="modal-body">...</div>
              <div className="modal-footer">
                <button className="btn btn-white" data-bs-dismiss="modal">
                  Close
                </button>
                <button className="btn btn-success">Action</button>
              </div>
            </div>
          </div>
        </div>
      </div>
      {/* <ViewRow /> */}
    </>
  );
};

export default PropertyProfile;

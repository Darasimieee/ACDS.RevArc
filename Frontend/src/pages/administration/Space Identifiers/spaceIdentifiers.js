import React, { useState, useEffect } from "react";
import { Link } from "react-router-dom";
import { useSelector } from "react-redux";
import FilterComponent from "../../../components/filter component/filtercomponent";
import DataTable from "react-data-table-component";
import api from "../../../axios/custom";
import { Spinner } from "react-activity";
import "react-activity/dist/library.css";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

const columns = [
  {
    name: "space Identifier Name",
    selector: (row) => row.spaceIdentifierName,
    sortable: true,
  },
  {
    name: "",
    width: "63px",
    cell: (row) => (
      <Link
        to={`/Administration/spaceIdentifiers/edit/${row.id}`}
        className="btn btn-sm btn-primary"
      >
        Edit
      </Link>
    ),
  },
  // {
  //   key: "action",
  //   text: "Action",
  //   className: "action",
  //   width: "67px",
  //   align: "center",
  //   sortable: false,
  //   cell: (row) => {
  //     return (
  //       <button
  //         data-bs-toggle="modal"
  //         data-bs-target="#modalAlert"
  //         className="btn btn-sm btn-info"
  //         onClick={() => {
  //           ViewRow(row);
  //         }}
  //       >
  //         View
  //       </button>
  //     );
  //   },
  // },
];

const SpaceIdentifiers = () => {
  const token = localStorage.getItem("myToken");
  const [filterText, setFilterText] = React.useState("");
  const [resetPaginationToggle, setResetPaginationToggle] = useState(false);
  const [data, setData] = useState([]);
  const [editingRowId, setEditingRowId] = useState(null);
  const [pending, setPending] = React.useState(true);
  const [loading, setLoading] = useState(false);
  const [isModalHidden, setIsModalHidden] = useState(false);

  const [newWard, setNewWard] = useState("");
  const state = useSelector((state) => state);
  let user = state.authReducer.user;

  let organisationId = user.organisationId;
  const filteredItems = data.filter(
    (item) =>
      item.spaceIdentifierName &&
      item.spaceIdentifierName.toLowerCase().includes(filterText.toLowerCase())
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
      .get("enumeration/space-identifiers", {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      })
      .then((response) => {
        console.log(response);
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
            <Link to="/dashboard">Home</Link>
          </li>
          <li className="breadcrumb-item">Enumeration</li>
          <li className="breadcrumb-item active">Space Identifier </li>
        </ol>
        <h1 className="page-header mb-3">Space Identifier</h1>
        <hr />

        <div>
          <Link className="btn btn-primary" to="createNew">
            Add New Space Identifier
          </Link>
        </div>
        <DataTable
          columns={columns}
          data={filteredItems}
          onClick={(item) => console.log(item)}
          pagination
          loading
          progressPending={pending}
          paginationResetDefaultPage={resetPaginationToggle} // optionally, a hook to reset pagination to page 1
          subHeader
          subHeaderComponent={subHeaderComponentMemo}
        />
      </div>
    </>
  );
};

export default SpaceIdentifiers;

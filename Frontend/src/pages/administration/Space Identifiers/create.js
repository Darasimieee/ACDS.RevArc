import React, { useState, useEffect } from "react";
import { Link, useNavigate } from "react-router-dom";
import { useSelector } from "react-redux";
import FilterComponent from "../../../components/filter component/filtercomponent";
import DataTable from "react-data-table-component";
import api from "../../../axios/custom";
import { Spinner } from "react-activity";
import "react-activity/dist/library.css";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

const CreateSpaceIdentifier = () => {
  const token = localStorage.getItem("myToken");
  let navigate = useNavigate();
  const [loading, setLoading] = useState(false);

  const [spaceIdentifierName, setSpaceIdentifierName] = useState("");
  const handleChange = (event) => {
    setSpaceIdentifierName(event.target.value);
  };
  const addNewWard = async (e) => {
    setLoading(true);
    e.preventDefault();
    await api
      .post(
        "enumeration/spaceidentifiers",
        {
          SpaceIdentifierName: spaceIdentifierName,
          dateCreated: new Date(),
          createdBy: "Ayomide Sonuga",
        },
        {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        }
      )
      .then((response) => {
        console.log(response);
        if (response.status === 201) {
          console.log(response.data);
          toast.success(response.statusText, {
            position: "top-right",
            autoClose: 5000,
            hideProgressBar: true,
            closeOnClick: true,
            pauseOnHover: true,
            draggable: true,
            progress: undefined,
            theme: "colored",
          });
          setSpaceIdentifierName("");
          setTimeout(() => {
            navigate("/administration/spaceidentifiers");
          }, 2000);
        }
        setLoading(false);
      })
      .catch((error) => {
        console.log(error);
        setLoading(false);
      });
  };

  return (
    <>
      <div className="  mt-5 d-flex justify-content-center">
        <div className=" shadow p-3 w-50 ">
          <div className="">
            <div className="text-center">
              <ToastContainer />
              <h4 className="">Add New Space Identifier</h4>
            </div>
            <div className="modal-body">
              <form onSubmit={addNewWard}>
                <div className="row gx-5">
                  <div className="col">
                    <div className="mb-3 ">
                      <label
                        className="form-label"
                        htmlFor="exampleInputEmail1"
                      >
                        Space Identifier
                      </label>

                      <input
                        type="text"
                        className="form-control"
                        value={spaceIdentifierName}
                        placeholder="Enter Space Identifier Name"
                        onChange={handleChange}
                        required
                      />
                    </div>
                  </div>
                </div>
                <div className="d-flex justify-content-end">
                  <button type="submit" className="btn btn-primary">
                    {loading ? <Spinner /> : "Add"}
                  </button>
                </div>
              </form>
            </div>
          </div>
        </div>
      </div>
    </>
  );
};

export default CreateSpaceIdentifier;

import React, { useState, useEffect } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { useSelector } from "react-redux";
import api from "../../../axios/custom";
import { Spinner } from "react-activity";
import "react-activity/dist/library.css";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

const EditWard = () => {
  const { id } = useParams();
  const token = localStorage.getItem("myToken");
  let navigate = useNavigate();
  const [loading, setLoading] = useState(false);

  const [newWard, setNewWard] = useState("");
  const handleChange = (event) => {
    setNewWard(event.target.value);
  };
  const editWard = async (e) => {
    setLoading(true);
    e.preventDefault();
    await api
      .put(
        `enumeration/${id}/wards`,
        {
          wardName: newWard,
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
          setNewWard("");
          setTimeout(() => {
            navigate("/administration/wards");
          }, 2000);
        }
        setLoading(false);
      })
      .catch((error) => {
        console.log(error);
        setLoading(false);
      });
  };
  useEffect(() => {
    api
      .get(`enumeration/${id}/wards`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      })
      .then((response) => {
        console.log(response);
        setNewWard(response.data.wardName);
      })
      .catch((error) => {
        console.log(error);
      });
  }, []);

  return (
    <>
      <div className="  mt-5 d-flex justify-content-center">
        <div className=" shadow p-3 w-50 ">
          <div className="">
            <div className="text-center">
              <ToastContainer />
              <h4 className="">Edit Ward</h4>
            </div>
            <div className="modal-body">
              <form onSubmit={editWard}>
                <div className="row gx-5">
                  <div className="col">
                    <div className="mb-3 ">
                      <label
                        className="form-label"
                        htmlFor="exampleInputEmail1"
                      >
                        Ward Name
                      </label>

                      <input
                        type="text"
                        className="form-control"
                        value={newWard}
                        placeholder="Enter Ward Name"
                        onChange={handleChange}
                        required
                      />
                    </div>
                  </div>
                </div>
                <div className="d-flex justify-content-end">
                  <button type="submit" className="btn btn-primary">
                    {loading ? <Spinner /> : "Update"}
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

export default EditWard;

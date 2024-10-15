import React, { useState, useEffect } from "react";
import { Link, useNavigate } from "react-router-dom";
import api from "../../axios/custom";
import { Spinner } from "react-activity";
import "react-activity/dist/library.css";
import { PhotoIcon } from "@heroicons/react/24/solid";

const OrganisationData = {
  payerId: "",
  organisationName: "",
  email: "",
  address: "",
  phoneNo: "",
};
const Billings = () => {
  const token = localStorage.getItem("myToken");
  const [isOn, setIsOn] = useState(false);
  const [state, setState] = useState([]);
  const [selectedState, setSelectedState] = useState("");
  const [lgas, setLgas] = useState([]);
  const [selectedLga, setSelectedLga] = useState("");
  const [lcdas, setLcdas] = useState([]);
  const [selectedLcda, setSelectedLcda] = useState("");
  const [file, setFile] = useState("");
  const [fileUrl, setFileUrl] = useState(null);
  const [backgroundImage, setBackgroundImage] = useState("");
  const [backgroundUrl, setBackgroundUrl] = useState(null);
  const [userInput, setUserInput] = useState(OrganisationData);
  const [showNin, setShowNin] = useState(false);
  const [showBvn, setShowBvn] = useState(false);

  const showNinForm = (event) => {
    setShowNin(true);
    setShowBvn(false);
  };
  const showBvnForm = (event) => {
    setShowBvn(true);
    setShowNin(false);
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    setUserInput({ ...userInput, [name]: value });
  };

  const handleStateChange = (event) => {
    const stateId = event.target.value;
    setSelectedState(stateId);
  };
  const handleLgaChange = (event) => {
    setSelectedLga(event.target.value);
  };
  const handleLcdaChange = (event) => {
    setSelectedLcda(event.target.value);
  };
  const handleFileUpload = (event) => {
    const file = event.target.files[0];
    setFile(file);
    const reader = new FileReader();
    reader.onloadend = (e) => {
      setFileUrl(reader.result);
    };
    reader.readAsDataURL(file);
  };
  function submitHandler(e) {
    e.preventDefault();
    const formData = new FormData();
    formData.append("countryId", 149);
    formData.append("payerId", userInput.payerId);
    formData.append("OrganisationName", userInput.organisationName);
    formData.append("address", userInput.address);
    formData.append("city", null);
    formData.append("phoneNo", userInput.phoneNo);
    formData.append("email", userInput.email);
    formData.append("logoData", fileUrl);
    formData.append("logoName", file.name);
    formData.append("backgroundImagesData", backgroundUrl);
    formData.append("backgroundImagesName", backgroundImage.name);
    formData.append("dateCreated", new Date());
    formData.append("createdBy", "Toyeeb");
    // data.append("OrganisationName", userInput.OrganisationName);

    // for (var key of formData.entries()) {
    //   console.log("entries", key[0] + ", " + key[1]);
    // }
  }

  const handleBackgroundUpload = (event) => {
    const backgroundImage = event.target.files[0];
    setBackgroundImage(backgroundImage);
    const reader = new FileReader();
    reader.onloadend = (e) => {
      setBackgroundUrl(reader.result);
    };
    reader.readAsDataURL(backgroundImage);
  };

  // Fetch the list of states from the API and set the options for the state select element
  useEffect(() => {
    const fetchState = async () => {
      await api
        .get("enumeration/states")
        .then((response) => {
          setState(response.data);
        })
        .catch((error) => {
          console.log(error);
        });
    };
    fetchState();
  }, []);

  //Fetch all corresponding Lgas based on the state selected
  useEffect(() => {
    const fetchLga = async () => {
      await api
        .get(`enumeration/${selectedState}/lgas`)
        .then((response) => {
          // console.log(response.data);
          setLgas(response.data);
          // console.log(lgas);
        })
        .catch((error) => {
          console.log(error);
        });
    };
    if (selectedState) {
      fetchLga();
    } else {
      setLgas([]);
      setSelectedLga("");
    }
  }, [selectedState]);

  //Fetch all corresponding Lcdas based on Lgs selected
  useEffect(() => {
    const fetchLcda = async () => {
      await api
        .get(`enumeration/${selectedLga}/lcdas`)
        .then((response) => {
          console.log("fetched lcdas------", response.data);
          setLcdas(response.data);
        })
        .catch((error) => {
          console.log(error);
        });
    };
    if (selectedLga) {
      fetchLcda();
    } else {
      setLcdas([]);
      setSelectedLcda("");
    }
  }, [selectedLga]);

  const handleToggle = () => {
    setIsOn(!isOn);
  };

  return (
    <>
      <ol className="breadcrumb float-xl-end">
        <li className="breadcrumb-item">
          <Link to="/dashboard">Home</Link>
        </li>
        <li className="breadcrumb-item">Bill Management</li>
        <li className="breadcrumb-item active">Billing </li>
      </ol>
      <h1 className="page-header mb-3">Billing</h1>
      <hr />

      <div className="d-flex my-5">
        <h4 className="mr-4"> Type:</h4>
        <div className="form-check form-check-inline">
          <input
            className="form-check-input"
            type="radio"
            name="inlineRadioOptions"
            id="inlineRadio1"
            value="Nin"
            onChange={showNinForm}
          />
          <label className="form-check-label" htmlFor="inlineRadio1">
            Property
          </label>
        </div>
        <div className="form-check form-check-inline">
          <input
            className="form-check-input"
            type="radio"
            name="inlineRadioOptions"
            id="inlineRadio2"
            value="Bvn"
            onChange={showBvnForm}
          />
          <label className="form-check-label" htmlFor="inlineRadio2">
            Non-Property
          </label>
        </div>
      </div>
      <form onSubmit={submitHandler} className="shadow-lg p-3">
        <div className="space-y-12">
          <div className="border-b border-gray-900/10 pb-12">
            <div className="mt-10 grid grid-cols-1 gap-x-6 gap-y-8 sm:grid-cols-6">
              <div className="col-span-6">
                <label
                  htmlFor="first-name"
                  className="block text-sm font-medium leading-6 text-gray-900"
                >
                  Property Address
                </label>
                <div className="mt-2">
                  <input
                    type="text"
                    placeholder="Enter Address"
                    value={userInput.payerId}
                    name="payerId"
                    onChange={handleChange}
                    className=" px-1.5 block w-full rounded-md border py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6"
                  />
                </div>
              </div>
              <div className="col-span-6">
                <label
                  htmlFor="last-name"
                  className="block text-sm font-medium leading-6 text-gray-900"
                >
                  Customer Details
                </label>
                <div className="mt-2">
                  <input
                    type="text"
                    placeholder="Enter Organization Name"
                    value={userInput.organisationName}
                    name="organisationName"
                    onChange={handleChange}
                    className="px-1.5 block w-full rounded-md border py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6"
                  />
                </div>
              </div>

              <div className="sm:col-span-2 ">
                <label
                  htmlFor="city"
                  className="block text-sm font-medium leading-6 text-gray-900"
                >
                  Business Type
                </label>
                <div className="mt-2">
                  <select
                    name="stateId"
                    onChange={handleStateChange}
                    value={selectedState}
                    required
                    className="block w-full rounded-md border-0 py-2 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6"
                  >
                    <option value="">Select State</option>
                    {state.map((state) => (
                      <option key={state.id} value={state.id}>
                        {state.stateName}
                      </option>
                    ))}
                  </select>
                </div>
              </div>
              <div className="sm:col-span-2 ">
                <label
                  htmlFor="city"
                  className="block text-sm font-medium leading-6 text-gray-900"
                >
                  Business Size
                </label>
                <div className="mt-2">
                  <select
                    name="lgaId"
                    onChange={handleLgaChange}
                    value={selectedLga}
                    required
                    className="block w-full rounded-md border-0 py-2 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6"
                  >
                    <option value="">Select Local Government</option>
                    {lgas.map((lga) => (
                      <option key={lga.id} value={lga.id}>
                        {lga.lgaName}
                      </option>
                    ))}
                  </select>
                </div>
              </div>
            </div>
          </div>
          <fieldset>
            <legend class="text-sm font-semibold leading-6 text-gray-900">
              Revenue Type/Code
            </legend>
            <div class="mt-6 flex">
              <div class="flex items-center gap-x-3">
                <input
                  id="push-everything"
                  name="push-notifications"
                  type="radio"
                  class="h-4 w-4 border-gray-300 text-indigo-600 focus:ring-indigo-600"
                />
                <label
                  for="push-everything"
                  class="block text-sm font-medium leading-6 text-gray-900"
                >
                  Everything
                </label>
              </div>
              <div class="flex items-center gap-x-3">
                <input
                  id="push-email"
                  name="push-notifications"
                  type="radio"
                  class="h-4 w-4 border-gray-300 text-indigo-600 focus:ring-indigo-600"
                />
                <label
                  for="push-email"
                  class="block text-sm font-medium leading-6 text-gray-900"
                >
                  Same as email
                </label>
              </div>
              <div class="flex items-center gap-x-3">
                <input
                  id="push-nothing"
                  name="push-notifications"
                  type="radio"
                  class="h-4 w-4 border-gray-300 text-indigo-600 focus:ring-indigo-600"
                />
                <label
                  for="push-nothing"
                  class="block text-sm font-medium leading-6 text-gray-900"
                >
                  No push notifications
                </label>
              </div>
            </div>
          </fieldset>
        </div>

        <div className="mt-6 flex items-center justify-end gap-x-6">
          <button
            type="button"
            className="text-sm font-semibold leading-6 text-gray-900"
          >
            Cancel
          </button>
          <button
            type="submit"
            className="rounded-md btn btn-primary px-3 py-2 text-sm font-semibold text-white shadow-sm hover:bg-indigo-500 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-indigo-600"
          >
            Save
          </button>
        </div>
      </form>
    </>
  );
};

export default Billings;

import React from "react";
import {
  BrowserRouter,
  Routes,
  Route,
  useRoutes,
  HashRouter,
} from "react-router-dom";
import { createStore } from "redux";
import { Provider } from "react-redux";

// bootstrap
import "bootstrap";
// css
import "@fortawesome/fontawesome-free/css/all.css";
import "react-perfect-scrollbar/dist/css/styles.css";
import "./index.css";
import "./scss/react.scss";
// import store from "./redux/store.js";
import LoginV3 from "./pages/User/login-v3.js";
import rootReducer from "./redux/reducers/rootReducer";
import { PrivateRoute } from "./components/auth/PrivateRoute.js";
import LandingPage from "./LandingPage.jsx";
import DefaultLayout from "./components/layout/DefaultLayout";
import RegisterV3 from "./pages/User/register-v3";
import SelfService from "./pages/User/SelfService/SelfService";
import SelfRegistration from "./pages/User/SelfService/SelfRegistration";
import ForgotPassword from "./pages/User/SelfService/ForgotPassword";
import OneTimePassword from "./pages/User/SelfService/otpPage";
import PasswordReset from "./pages/User/SelfService/PasswordReset";
import Dashboard from "./pages/Dashboard/Dashboard";
import Enumerate from "./pages/Enumeration/Enumerate/enumerate";
import PropertyProfile from "./pages/Enumeration/Properties/PropertyProfile";
import Enumeration from "./pages/Enumeration";
import Manifest from "./pages/Enumeration/Manifest/Manifest";
import NonPropertyProfile from "./pages/Enumeration/Non-Property";
import EditProperty from "./pages/Enumeration/Properties/editProperty";
import NewPropertyProfile from "./pages/Enumeration/Properties/NewPropertyProfile";
import AddBusinessProfile from "./pages/Enumeration/Business Profile/createBusinessProfile";
import CreatePayId from "./pages/Enumeration/CreatePayerId";
import BusinessProfile from "./pages/Enumeration/Business Profile/businessProfile";
import CustomerProfile from "./pages/Enumeration/Customer Profile/customerProfile";
import ValidatePayId from "./pages/Enumeration/ValidatePayerId";
import SearchPayId from "./pages/Enumeration/SearchPayerId";
import CreateNewPayerId from "./pages/Enumeration/createNewPayerId";
import Administration from "./pages/Administration";
import Wards from "./pages/Administration/Wards/ward";
import Ward from "./pages/Administration/Wards";
import CreateWard from "./pages/Administration/Wards/create";
import EditWard from "./pages/Administration/Wards/edit";
import Agency from "./pages/Administration/Agencies";
import Agencies from "./pages/Administration/Agencies/agencies";
import SpaceIdentifier from "./pages/Administration/Space Identifiers";
import SpaceIdentifiers from "./pages/Administration/Space Identifiers/spaceIdentifiers";
import CreateSpaceIdentifier from "./pages/Administration/Space Identifiers/create";
import EditSpaceIdentifier from "./pages/Administration/Space Identifiers/edit";
import ManifestSlip from "./pages/Enumeration/Manifest/ManifestSlip";
import Manifests from "./pages/Enumeration/Manifest";
import Organisation from "./pages/Administration/Organisations/organisation";
import Organisations from "./pages/Administration/Organisations/index";
import Billing from "./pages/Billing/index";
import Billings from "./pages/Billing/Billing";
import BillingList from "./pages/Billing/BillingList";
import CustomerDto from "./pages/Enumeration/Customer Profile/CustomerDto";
import { ErrorBoundary } from "react-error-boundary";
import {
  MyErrorFallback,
  logErrorToMyService,
} from "./components/Error Boundary/ErrorBoundary";

const store = createStore(rootReducer);

const App = () => {
  return (
    <Provider store={store}>
      <ErrorBoundary
        FallbackComponent={MyErrorFallback}
        onError={logErrorToMyService}
      >
        <BrowserRouter>
          <Routes>
            <Route path="/" name="Landing Page" element={<LandingPage />} />
            <Route
              path="register"
              name="Register Page"
              element={<RegisterV3 />}
            />
            <Route
              path="selfservice"
              name="Self Service"
              element={<SelfService />}
            />
            <Route
              path="selfregistration"
              name="Self Registration"
              element={<SelfRegistration />}
            />
            <Route
              path="forgotpassword"
              name="Forgot Password"
              element={<ForgotPassword />}
            />
            <Route path="otp" name="otp Page" element={<OneTimePassword />} />
            <Route
              path="passwordreset"
              name="Password Reset Page"
              element={<PasswordReset />}
            />
            <Route exact path="login" name="Login Page" element={<LoginV3 />} />
            <Route
              path="home"
              name="Home"
              element={
                <PrivateRoute>
                  <DefaultLayout />
                </PrivateRoute>
              }
            >
              <Route path="Dashboard" element={<Dashboard />} />

              {/* Enumeration Routes and all children*/}
              <Route path="enumeration" element={<Enumeration />}>
                <Route path="" element={<Enumerate />} />
                <Route path="PropertyProfile" element={<PropertyProfile />} />
                <Route path="manifest" element={<Manifests />}>
                  <Route path="" element={<Manifest />} />
                  <Route
                    path="print-manifest-slip"
                    element={<ManifestSlip />}
                  />
                </Route>

                <Route
                  path="nonpropertyprofile"
                  element={<NonPropertyProfile />}
                />
                <Route path="editproperty/:id" element={<EditProperty />} />
                <Route
                  path="newPropertyProfile"
                  element={<NewPropertyProfile />}
                />
                <Route
                  path="createbusinessprofile"
                  element={<AddBusinessProfile />}
                />
                <Route path="customerprofiles" element={<CustomerDto />} />
                <Route path="createPayerId" element={<CreatePayId />} />
                <Route path="businessprofile" element={<BusinessProfile />} />
                <Route path="customerprofile" element={<CustomerProfile />} />
                <Route path="validatepayerId" element={<ValidatePayId />} />
                <Route path="searchpayerId" element={<SearchPayId />} />
                <Route path="createnewpayerId" element={<CreateNewPayerId />} />
              </Route>

              {/* Administration Routes and all children*/}
              <Route path="administration" element={<Administration />}>
                <Route path="wards" element={<Ward />}>
                  <Route path="" element={<Wards />} />
                  <Route path="createnew" element={<CreateWard />} />
                  <Route path="edit/:id" element={<EditWard />} />
                </Route>
                <Route path="SpaceIdentifiers" element={<SpaceIdentifier />}>
                  <Route path="" element={<SpaceIdentifiers />} />
                  <Route path="createnew" element={<CreateSpaceIdentifier />} />
                  <Route path="edit/:id" element={<EditSpaceIdentifier />} />
                </Route>
                <Route path="Agencies" element={<Agency />}>
                  <Route path="" element={<Agencies />} />
                </Route>
                <Route path="organisation" element={<Organisations />}>
                  <Route path="" element={<Organisation />} />
                </Route>
              </Route>
              {/* Bill Management Routes and all children*/}
              <Route path="billing" element={<Billing />}>
                <Route path="" element={<BillingList />} />
                <Route path="generatenewbill" element={<Billings />} />
              </Route>
            </Route>
          </Routes>
        </BrowserRouter>
      </ErrorBoundary>
    </Provider>
  );
};

export default App;

import CryptoJS from "crypto-js";
// Function to encrypt the ID
const encryptID = (id) => {
  const encrypted = CryptoJS.AES.encrypt(id.toString(), "secret key");
  return encrypted.toString();
};
// Function to decrypt the ID
const decryptID = (encryptedID) => {
  const bytes = CryptoJS.AES.decrypt(encryptedID, "secret key");
  const decryptedID = bytes.toString(CryptoJS.enc.Utf8);
  return decryptedID;
};

export const authenticateUser = (token, organisationId, userProfileId) => {
  const encryptedOrganisationId = encryptID(organisationId);
  const encryptedUserProfileId = encryptID(userProfileId);

  if (typeof window !== "undefined") {
    localStorage.setItem("myToken", token);
    localStorage.setItem("organisationId", encryptedOrganisationId);
    localStorage.setItem("userProfileId", encryptedUserProfileId);
  }
};

export const isAuthenticated = () => {
  if (typeof window === "undefined") {
    return false;
  }
  if (localStorage.getItem("myToken")) {
    return localStorage.getItem("myToken");
  } else {
    return false;
  }
};

export const isAuthenticatedOrganisationId = () => {
  if (typeof window == "undefined") {
    return false;
  }
  if (localStorage.getItem("organisationId")) {
    return localStorage.getItem("organisationId");
  } else {
    return false;
  }
};
export const isAuthenticatedUserProfileId = () => {
  if (typeof window == "undefined") {
    return false;
  }
  if (localStorage.getItem("userProfileId")) {
    return localStorage.getItem("userProfileId");
  } else {
    return false;
  }
};

export const logout = () => {
  if (typeof window != "undefined") {
    localStorage.removeItem("userProfileId");
    localStorage.removeItem("organisationId");
    localStorage.removeItem("myToken");
    window.location.replace("/");
  }
};

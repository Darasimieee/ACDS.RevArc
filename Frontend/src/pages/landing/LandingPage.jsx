import React from "react";
import Hero from "./Hero";
import Navbar from "./Navbar";
import "../..";

const LandingPage = () => {
  return (
    <div className="landing">
      <Navbar />
      <Hero />
      <div className=""></div>
    </div>
  );
};

export default LandingPage;

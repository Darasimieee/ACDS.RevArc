import React from "react";
import phone from "../../assets/images/phone2.png";
import building from "../../assets/images/building.png";
import "./Hero.css";

const Hero = () => {
  return (
    <>
      <div className=" container relative mt-4 pt-5 text-center">
        <div className="row">
          <div className="heading px-5">
            <h1 className="landing_heading font-weight-bold text-dark">
              Enumeration, Billing, Payment &{" "}
              <span className="text-primary font-weight-bold">
                Report Management Application
              </span>{" "}
            </h1>
          </div>
        </div>

        {/* <div className="hero-image mt-4 pt-4">
          <img src={phone} className="w-100" />
        </div> */}
      </div>
    </>

    // <div>
    //   <div className="relative isolate">
    //     <div
    //       className="absolute inset-x-0 -top-40 -z-10 transform-gpu overflow-hidden blur-3xl sm:-top-80"
    //       aria-hidden="true"
    //     ></div>
    //     <div className="mx-auto max-w-4xl pt-32 sm:pt-48 lg:pt-12">
    //       {/* <div className="hidden sm:mb-8 sm:flex sm:justify-center">
    //         <div className="relative rounded-full px-3 py-1 text-sm leading-6 text-gray-600 ring-1 ring-gray-900/10 hover:ring-gray-900/20">
    //           Announcing our next round of funding.{" "}
    //           <a href="#" className="font-semibold text-indigo-600">
    //             <span className="absolute inset-0" aria-hidden="true" />
    //             Read more <span aria-hidden="true">&rarr;</span>
    //           </a>
    //         </div>
    //       </div> */}
    //       <div className="">
    //         <h1 className="text-4xl font-bold tracking-tight text-gray-900 sm:text-6xl">
    //           Enumeration, Billing, Payment &{" "}
    //           <span className="text-danger">Report Management Application</span>{" "}
    //         </h1>

    //         {/* <div className="mt-10 flex items-center justify-center gap-x-6">
    //         <a
    //           href="#"
    //           className="rounded-md bg-indigo-600 px-3.5 py-2.5 text-sm font-semibold text-white shadow-sm hover:bg-indigo-500 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-indigo-600"
    //         >
    //           Get started
    //         </a>
    //         <a
    //           href="#"
    //           className="text-sm font-semibold leading-6 text-gray-900"
    //         >
    //           Learn more <span aria-hidden="true">→</span>
    //         </a>
    //       </div> */}
    //       </div>
    //     </div>

    //     <div
    //       className="absolute inset-x-0 top-[calc(100%-13rem)] -z-10 transform-gpu overflow-hidden blur-3xl sm:top-[calc(100%-30rem)]"
    //       aria-hidden="true"
    //     ></div>
    //   </div>
    //   <div className="hero-image">
    //     <img src={building} className="w-100" />{" "}
    //   </div>
    // </div>
  );
};

export default Hero;

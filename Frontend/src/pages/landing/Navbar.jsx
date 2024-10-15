import React, { useState } from "react";
import logo from "../../assets/images/Logo.png";
import { Link } from "react-router-dom";
import { Fragment } from "react";
import { Disclosure, Menu, Transition } from "@headlessui/react";
import { Dialog } from "@headlessui/react";
import { Bars3Icon, BellIcon, XMarkIcon } from "@heroicons/react/24/outline";

const navigation = [
  { name: "Self-Service", link: "/selfservice" },
  { name: "Developer", link: "/selfservice" },
  { name: "Business", link: "/selfservice" },
];

const Navbar = () => {
  const [mobileMenuOpen, setMobileMenuOpen] = useState(false);
  return (
    <>
      {/* <div className="row">
        <div className="col-3 ">
          <img src={logo} className="" />
        </div>
        <div className=" col-5 d-flex align-items-center justify-content-evenly fs-20px">
          <Link
            className="nav-items text-dark font-weight-bolder text-decoration-none"
            to="/selfservice"
          >
            Self-Service
          </Link>
          <a
            href="#"
            className="nav-items text-dark font-weight-bolder text-decoration-none"
          >
            Developer
          </a>
          <a
            href="#"
            className="nav-items text-dark font-weight-bolder text-decoration-none"
          >
            Business
          </a>
        </div>
        <div className="col-2"></div>
        <div className="col-2 fs-20px d-flex align-items-center justify-content-end ">
          <Link
            className="menu-link login mx-3 px-4 border-0 text-decoration-none text-dark rounded bg-light py-1"
            to="/login"
          >
            Login
          </Link>
          <Link
            className="menu-link login px-4 py-1 bg-white text-dark text-decoration-none border-2 rounded border-dark"
            to="/register"
          >
            Onboarding
          </Link>
        </div>
      </div> */}
      <header className="">
        <nav
          className="flex items-center justify-between p-6 lg:px-8"
          aria-label="Global"
        >
          <div className="flex">
            <Link to="/" className="">
              <span className="sr-only">Revbills</span>
              <img className=" h-8 md:h-12" src={logo} alt="revbills" />
            </Link>
          </div>
          <div className="flex lg:hidden">
            <button
              type="button"
              className="-m-2.5 inline-flex items-center justify-center rounded-md p-2.5 text-gray-700"
              onClick={() => setMobileMenuOpen(true)}
            >
              <span className="sr-only">Open main menu</span>
              <Bars3Icon className="h-6 w-6" aria-hidden="true" />
            </button>
          </div>
          <div className="hidden lg:flex lg:gap-x-12">
            {navigation.map((item) => (
              <Link
                key={item.name}
                to={item.link}
                className="text-lg font-normal text-decoration-none leading-6 text-gray-900"
              >
                {item.name}
              </Link>
            ))}
          </div>
          <div className="hidden lg:flex ">
            <Link
              to="/register"
              className="text-lg px-3 py-1 border-2 text-decoration-none rounded  font-normal leading-6 text-gray-900 border-dark transform hover:scale-105 duration-500 ease-in-out"
            >
              Onboarding
            </Link>
            <Link
              to="/login"
              className="text-lg pl-5 py-2 font-normal leading-6 text-gray-900 transform hover:scale-105 duration-500 ease-in-out"
            >
              Log in <span aria-hidden="true">&rarr;</span>
            </Link>
          </div>
        </nav>
        <Dialog
          as="div"
          className="lg:hidden"
          open={mobileMenuOpen}
          onClose={setMobileMenuOpen}
        >
          <div className="fixed inset-0 z-50" />
          <Dialog.Panel className="fixed inset-y-0 right-0 z-50 w-full overflow-y-auto bg-white px-6 py-6 sm:max-w-sm sm:ring-1 sm:ring-gray-900/10">
            <div className="flex items-center justify-between">
              <a href="#" className="-m-1.5 p-1.5">
                <span className="sr-only">Revbill</span>
                <img className="h-8 w-auto" src={logo} alt="revbills" />
              </a>
              <button
                type="button"
                className="-m-2.5 rounded-md p-2.5 text-gray-700"
                onClick={() => setMobileMenuOpen(false)}
              >
                <span className="sr-only">Close menu</span>
                <XMarkIcon className="h-6 w-6" aria-hidden="true" />
              </button>
            </div>
            <div className="mt-6 flow-root">
              <div className=" -my-6 divide-y divide-gray-500/10">
                <div className="space-y-2 py-6">
                  {navigation.map((item) => (
                    <Link
                      key={item.name}
                      to={item.link}
                      className="-mx-3 block rounded-lg px-3 py-2 text-base font-normal leading-7 text-gray-900 hover:bg-gray-50"
                    >
                      {item.name}
                    </Link>
                  ))}
                </div>
                <div className="py-6">
                  <Link
                    to="/register"
                    className="-mx-3 block rounded-lg px-3 py-2.5 text-base font-normal leading-7 text-gray-900 hover:bg-gray-50"
                  >
                    Onboarding
                  </Link>
                  <Link
                    to="/login"
                    className="-mx-3 block rounded-lg px-3 py-2.5 text-base font-normal leading-7 text-gray-900 hover:bg-gray-50"
                  >
                    Log in
                  </Link>
                </div>
              </div>
            </div>
          </Dialog.Panel>
        </Dialog>
      </header>
    </>
  );
};

export default Navbar;

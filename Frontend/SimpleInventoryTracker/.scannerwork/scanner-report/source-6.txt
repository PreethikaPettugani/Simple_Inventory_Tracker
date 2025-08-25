import React from "react";
import { Link } from "react-router-dom";
import "./Navbar.css";

const Navbar = () => (
  <nav className="navbar">
    <h2>StockHub</h2>
    <div>
      <Link to="/">Home</Link>
      <Link to="/dashboard"> Dashboard</Link>
      <Link to="/about">About Us</Link>
    </div>
  </nav>
);

export default Navbar;


import React from 'react';
import { Link } from 'react-router-dom';

const Navbar = ({ onLogout }) => (
  <nav className="navbar navbar-expand-lg navbar-dark" style={{ background: 'linear-gradient(to right,rgb(75, 158, 160),rgb(17, 147, 158))' }}>
    <div className="container-fluid">
      <Link className="navbar-brand" to="/book">Patient Portal</Link>
      <div>
        <Link className="btn btn-outline-light me-2" to="/book">Book Appointment</Link>
        <Link className="btn btn-outline-light me-2" to="/appointments">View Appointments</Link>
        <button className="btn btn-danger" onClick={onLogout}>Logout</button>
      </div>
    </div>
  </nav>
);

export default Navbar;





import React, { useState } from 'react';
import axios from 'axios';
import { useNavigate, Link } from 'react-router-dom';

const Register = () => {
  const [fullName, setFullName] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [message, setMessage] = useState('');
  const navigate = useNavigate();

  const handleRegister = async (e) => {
    e.preventDefault();
    try {
      await axios.post('http://localhost:5079/api/Auth/register', {
        fullName,
        email,
        password,
        role: "Patient",
      });
      setMessage("Registration successful! Redirecting to login...");
      setTimeout(() => navigate('/login'), 1500);
    } catch (err) {
      setMessage("Registration failed. Ensure all fields are valid.");
    }
  };

  return (
    <div className="container d-flex justify-content-center align-items-center vh-100">
      <div className="card p-4 shadow" style={{ minWidth: '400px' }}>
        <h2 className="text-center mb-3">Register</h2>
        <form onSubmit={handleRegister}>
          <div className="mb-3">
            <label>Full Name</label>
            <input type="text" className="form-control" required value={fullName}
              onChange={(e) => setFullName(e.target.value)} />
          </div>
          <div className="mb-3">
            <label>Email</label>
            <input type="email" className="form-control" required value={email}
              onChange={(e) => setEmail(e.target.value)} />
          </div>
          <div className="mb-3">
            <label>Password</label>
            <input type="password" className="form-control" required value={password}
              onChange={(e) => setPassword(e.target.value)} />
          </div>
          <button className="btn btn-success w-100" type="submit">Register</button>
          <p className="text-danger mt-2">{message}</p>
        </form>
        <p className="text-center mt-3">
          Already registered? <Link to="/login">Login here</Link>
        </p>
      </div>
    </div>
  );
};

export default Register;











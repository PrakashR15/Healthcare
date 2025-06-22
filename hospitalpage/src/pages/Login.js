import React, { useState } from 'react';
import axios from 'axios';
import { useNavigate, Link } from 'react-router-dom';

const Login = ({ setIsLoggedIn }) => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [message, setMessage] = useState('');
  const navigate = useNavigate();

  const isValidInput = () => {
    if (!email || !password) {
      setMessage('Email and password are required.');
      return false;
    } else if (!email.includes('@') || !email.includes('.')) {
      setMessage('Please enter a valid email.');
      return false;
    } else if (password.length < 6) {
      setMessage('Password must be at least 6 characters.');
      return false;
    } else {
      setMessage('success');
      return true;
    }
  };

  const handleLogin = async (e) => {
    e.preventDefault();

    if (!isValidInput()) return;

    try {
      const response = await axios.post('http://localhost:5079/api/Auth/login', {
        email,
        password,
      });

      const { token, userId } = response.data;
      localStorage.setItem('token', token);
      localStorage.setItem('userId', userId);
      setIsLoggedIn(true);
      navigate('/book');
    } catch (error) {
      setMessage('Invalid credentials or server error.');
    }
  };

  return (
    <div className="container d-flex justify-content-center align-items-center vh-100">
      <div className="card p-4 shadow" style={{ minWidth: '400px' }}>
        <h2 className="text-center mb-3">Login</h2>
        <form onSubmit={handleLogin}>
          <div className="mb-3">
            <label>Email</label>
            <input
              type="email"
              className="form-control"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              required
            />
          </div>
          <div className="mb-3">
            <label>Password</label>
            <input
              type="password"
              className="form-control"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
            />
          </div>
          <button className="btn btn-primary w-100" type="submit">
            Login
          </button>
          <p className="text-danger mt-2">{message}</p>
        </form>
        <p className="text-center mt-3">
          New user? <Link to="/register">Register here</Link>
        </p>
      </div>
    </div>
  );
};

export default Login;



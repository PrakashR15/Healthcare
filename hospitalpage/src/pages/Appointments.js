import React, { useEffect, useState } from 'react';
import axios from 'axios';

const Appointments = () => {
  const [appointments, setAppointments] = useState([]);
  const [error, setError] = useState('');

  useEffect(() => {
    const fetchAppointments = async () => {
      const token = localStorage.getItem('token');
      const userId = localStorage.getItem('userId');

      try {
        const response = await axios.get(`http://localhost:5079/api/appointment/patient/${userId}`, {
          headers: { Authorization: `Bearer ${token}` },
        });
        setAppointments(response.data);
      } catch (err) {
        setError('Failed to fetch appointments.');
      }
    };

    fetchAppointments();
  }, []);

  const wrapperStyle = {
    maxWidth: '1200px',
    margin: '0 auto',
    padding: '2rem',
    color: '#fff',
  };

  const titleStyle = {
    textAlign: 'center',
    color: 'rgb(21, 121, 132)',
    marginBottom: '2rem',
    fontSize: '2rem',
  };

  const gridStyle = {
    display: 'grid',
    gridTemplateColumns: 'repeat(auto-fit, minmax(500px, 1fr))',
    gap: '2rem',
    justifyItems: 'center',
  };

  const cardStyle = {
    backgroundColor: 'rgb(21, 134, 124)',
    padding: '20px',
    borderRadius: '16px',
    boxShadow: '0 4px 12px rgb(6, 131, 121)',
    width: '100%',
    maxWidth: '600px',
    boxSizing: 'border-box',
  };

  return (
    <div style={wrapperStyle}>
      <h2 style={titleStyle}>Your Appointments</h2>
      {error && <p>{error}</p>}
      {appointments.length === 0 && <p>No appointments found.</p>}

      <div style={gridStyle}>
        {appointments.map((appt, index) => (
          <div key={index} style={cardStyle}>
            <p><strong>Appointment ID:</strong> {appt.appointmentID}</p>
            <p><strong>Doctor Name:</strong> {appt.doctorName}</p>
            <p><strong>Status:</strong> {appt.status}</p>
            <p><strong>Date:</strong> {new Date(appt.date).toLocaleString()}</p>
            <p><strong>Reason:</strong> {appt.reason}</p>
          </div>
        ))}
      </div>
    </div>
  );
};

export default Appointments;

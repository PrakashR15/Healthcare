import React, { useEffect, useState } from 'react';
import axios from 'axios';

const BookAppointment = () => {
  const [doctors, setDoctors] = useState([]);
  const [selectedDoctor, setSelectedDoctor] = useState('');
  const [reason, setReason] = useState('');
  const [dateTime, setDateTime] = useState('');
  const [message, setMessage] = useState('');

  useEffect(() => {
    const fetchDoctors = async () => {
      try {
        const token = localStorage.getItem('token');
        const response = await axios.get('http://localhost:5079/api/doctor', {
          headers: { Authorization: `Bearer ${token}` },
        });
        setDoctors(response.data);
      } catch (err) {
        console.error(err);
      }
    };
    fetchDoctors();
  }, []);

  const bookAppointment = async () => {
    try {
      const token = localStorage.getItem('token');
      const userId = localStorage.getItem('userId');
      await axios.post(
        'http://localhost:5079/api/appointment',
        {
          patientID: userId,
          doctorID: selectedDoctor,
          date: dateTime,
          status: 'Pending',
          reason,
        },
        { headers: { Authorization: `Bearer ${token}` } }
      );
      setMessage('Appointment booked successfully.');
    } catch (err) {
      setMessage('Failed to book appointment.');
    }
  };

  return (
    <div className="container mt-5">
      <div className="card p-4 shadow">
        <h2 className="mb-4">Book Appointment</h2>
        <div className="mb-3">
          <label>Select Doctor</label>
          <select className="form-select" value={selectedDoctor} onChange={(e) => setSelectedDoctor(e.target.value)}>
            <option value="">Choose...</option>
            {doctors.map((doc) => (
              <option key={doc.id} value={doc.id}>
                {doc.fullName || doc.email}
              </option>
            ))}
          </select>
        </div>
        <div className="mb-3">
          <label>Date & Time</label>
          <input type="datetime-local" className="form-control" value={dateTime}
            onChange={(e) => setDateTime(e.target.value)} required />
        </div>
        <div className="mb-3">
          <label>Reason</label>
          <input type="text" className="form-control" value={reason}
            onChange={(e) => setReason(e.target.value)} required />
        </div>
        <button className="btn btn-primary" onClick={bookAppointment}>Book</button>
        {message && <p className="text-success mt-3">{message}</p>}
      </div>
    </div>
  );
};

export default BookAppointment;


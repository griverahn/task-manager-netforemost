import React, { useState } from 'react';
import api from '../api';

const UserForm = ({ onUserCreated }) => {
    const [user, setUser] = useState({
        firstName: '',
        lastName: '',
        email: '',
        phone: ''
    });

    const handleChange = (e) => {
        const { name, value } = e.target;
        setUser({
            ...user,
            [name]: value
        });
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        api.post('/users', user)
            .then(response => {
                console.log('User created:', response.data);
                onUserCreated();
                setUser({ firstName: '', lastName: '', email: '', phone: '' }); 
            })
            .catch(error => {
                console.error('There was an error creating the user!', error);
            });
    };

    return (
        <form onSubmit={handleSubmit} className="container mt-4">
            <div className="form-group">
                <label>First Name</label>
                <input type="text" className="form-control" name="firstName" value={user.firstName} onChange={handleChange} />
            </div>
            <div className="form-group">
                <label>Last Name</label>
                <input type="text" className="form-control" name="lastName" value={user.lastName} onChange={handleChange} />
            </div>
            <div className="form-group">
                <label>Email</label>
                <input type="email" className="form-control" name="email" value={user.email} onChange={handleChange} />
            </div>
            <div className="form-group">
                <label>Phone</label>
                <input type="text" className="form-control" name="phone" value={user.phone} onChange={handleChange} />
            </div>
            <button type="submit" className="btn btn-primary mt-3">Create User</button>
        </form>
    );
};

export default UserForm;

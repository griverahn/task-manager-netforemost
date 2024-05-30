import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import api from '../api';

const UserList = () => {
    const [users, setUsers] = useState([]);

    useEffect(() => {
        fetchUsers();
    }, []);

    const fetchUsers = () => {
        api.get('/users')
            .then(response => {
                setUsers(response.data);
            })
            .catch(error => {
                console.error('There was an error fetching the users!', error);
            });
    };

    return (
        <div className="container mt-4">
            <h2>User List</h2>
            <ul className="list-group">
                {users.map(user => (
                    <li key={user.id} className="list-group-item">
                        <Link to={`/tasks/${user.id}`}>{user.firstName} {user.lastName}</Link>
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default UserList;

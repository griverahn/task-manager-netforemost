import React, { useState } from 'react';
import { Route, Routes, Link, useParams } from 'react-router-dom';
import UserList from './components/UserList';
import UserForm from './components/UserForm';
import AssignmentList from './components/AssignmentList';
import AssignmentForm from './components/AssignmentForm';
import 'bootstrap/dist/css/bootstrap.min.css';

function App() {
    const [usersUpdated, setUsersUpdated] = useState(false);

    const handleUserCreated = () => {
        setUsersUpdated(!usersUpdated);
    };

    return (
        <div className="App">
            <nav className="navbar navbar-expand-lg navbar-light bg-light">
                <div className="container-fluid">
                    <Link className="navbar-brand" to="/">Task Manager</Link>
                    <div className="collapse navbar-collapse">
                        <ul className="navbar-nav me-auto mb-2 mb-lg-0">
                            <li className="nav-item">
                                <Link className="nav-link" to="/">Users</Link>
                            </li>
                        </ul>
                    </div>
                </div>
            </nav>

            <div className="container mt-4">
                <Routes>
                    <Route exact path="/" element={
                        <>
                            <UserForm onUserCreated={handleUserCreated} />
                            <UserList key={usersUpdated} />
                        </>
                    } />
                    <Route path="/tasks/:userId" element={<UserTasks />} />
                </Routes>
            </div>
        </div>
    );
}

const UserTasks = () => {
    const { userId } = useParams();
    const [assignmentsUpdated, setAssignmentsUpdated] = useState(false);
    const [assignmentToEdit, setAssignmentToEdit] = useState(null);

    const handleAssignmentCreated = () => {
        setAssignmentsUpdated(!assignmentsUpdated);
        setAssignmentToEdit(null);
    };

    const handleEditAssignment = (assignment) => {
        setAssignmentToEdit(assignment);
    };

    return (
        <>
            <AssignmentForm userId={userId} assignmentToEdit={assignmentToEdit} onAssignmentSaved={handleAssignmentCreated} />
            <AssignmentList userId={userId} onEditAssignment={handleEditAssignment} key={assignmentsUpdated} />
        </>
    );
};

export default App;

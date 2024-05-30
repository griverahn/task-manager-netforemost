import React, { useState, useEffect } from 'react';
import api from '../api';

const AssignmentList = ({ userId, onEditAssignment }) => {
    const [assignments, setAssignments] = useState([]);

    useEffect(() => {
        fetchAssignments();
    }, [userId]);

    const fetchAssignments = () => {
        api.get(`/assignments/user/${userId}`)
            .then(response => {
                const sortedAssignments = response.data.sort((a, b) => new Date(a.dueDate) - new Date(b.dueDate));
                setAssignments(sortedAssignments);
            })
            .catch(error => {
                console.error('There was an error fetching the assignments!', error);
            });
    };

    const handleDelete = (assignmentId) => {
        api.delete(`/assignments/${assignmentId}`)
            .then(() => {
                fetchAssignments();
            })
            .catch(error => {
                console.error('There was an error deleting the assignment!', error);
            });
    };

    const handleComplete = (assignment) => {
        const updatedAssignment = { ...assignment, isCompleted: !assignment.isCompleted };
        api.put(`/assignments/${assignment.id}`, updatedAssignment)
            .then(() => {
                fetchAssignments();
            })
            .catch(error => {
                console.error('There was an error updating the assignment!', error);
            });
    };

    return (
        <div className="container mt-4">
            <h2>Task List</h2>
            <ul className="list-group">
                {assignments.map(assignment => (
                    <li key={assignment.id} className="list-group-item">
                        <div>
                            <h5>{assignment.title}</h5>
                            <p>{assignment.description}</p>
                            <p>Due: {new Date(assignment.dueDate).toLocaleDateString()}</p>
                            <button className="btn btn-warning btn-sm me-2" onClick={() => onEditAssignment(assignment)}>Edit</button>
                            <button className="btn btn-success btn-sm me-2" onClick={() => handleComplete(assignment)}>
                                {assignment.isCompleted ? 'Mark as Incomplete' : 'Mark as Complete'}
                            </button>
                            <button className="btn btn-danger btn-sm" onClick={() => handleDelete(assignment.id)}>Delete</button>
                        </div>
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default AssignmentList;

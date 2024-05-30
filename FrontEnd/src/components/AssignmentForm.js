import React, { useState, useEffect } from 'react';
import api from '../api';

const AssignmentForm = ({ userId, assignmentToEdit, onAssignmentSaved }) => {
    const [assignment, setAssignment] = useState({
        title: '',
        description: '',
        dueDate: '',
        isCompleted: false,
        tags: '',
        priorityId: 1,
        userId: userId
    });

    useEffect(() => {
        if (assignmentToEdit) {
            setAssignment(assignmentToEdit);
        }
    }, [assignmentToEdit]);

    const handleChange = (e) => {
        const { name, value } = e.target;
        setAssignment({
            ...assignment,
            [name]: value
        });
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        if (assignment.id) {
            api.put(`/assignments/${assignment.id}`, assignment)
                .then(response => {
                    console.log('Assignment updated:', response.data);
                    onAssignmentSaved();
                    setAssignment({
                        title: '',
                        description: '',
                        dueDate: '',
                        isCompleted: false,
                        tags: '',
                        priorityId: 1,
                        userId: userId
                    });
                })
                .catch(error => {
                    console.error('There was an error updating the assignment!', error);
                });
        } else {
            api.post('/assignments', assignment)
                .then(response => {
                    console.log('Assignment created:', response.data);
                    onAssignmentSaved();
                    setAssignment({
                        title: '',
                        description: '',
                        dueDate: '',
                        isCompleted: false,
                        tags: '',
                        priorityId: 1,
                        userId: userId
                    });
                })
                .catch(error => {
                    console.error('There was an error creating the assignment!', error);
                });
        }
    };

    return (
        <form onSubmit={handleSubmit} className="container mt-4">
            <div className="form-group">
                <label>Title</label>
                <input type="text" className="form-control" name="title" value={assignment.title} onChange={handleChange} />
            </div>
            <div className="form-group">
                <label>Description</label>
                <input type="text" className="form-control" name="description" value={assignment.description} onChange={handleChange} />
            </div>
            <div className="form-group">
                <label>Due Date</label>
                <input type="date" className="form-control" name="dueDate" value={assignment.dueDate} onChange={handleChange} />
            </div>
            <div className="form-group">
                <label>Tags</label>
                <input type="text" className="form-control" name="tags" value={assignment.tags} onChange={handleChange} />
            </div>
            <div className="form-group">
                <label>Priority</label>
                <select className="form-control" name="priorityId" value={assignment.priorityId} onChange={handleChange}>
                    <option value={1}>High</option>
                    <option value={2}>Medium</option>
                    <option value={3}>Low</option>
                </select>
            </div>
            <button type="submit" className="btn btn-primary mt-3">
                {assignment.id ? 'Update Assignment' : 'Create Assignment'}
            </button>
        </form>
    );
};

export default AssignmentForm;

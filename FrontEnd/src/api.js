import axios from 'axios';

const api = axios.create({
    baseURL: 'http://localhost:5001/api',
    headers: {
        'x-api-key': 'net-foremost-api-key'
    }
});

export default api;

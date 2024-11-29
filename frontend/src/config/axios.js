import axios from 'axios';

// Создаем экземпляр axios
const api = axios.create({
    baseURL: 'http://localhost:5062/pharmacy',  // Базовый URL для API
    headers: {
        'Content-Type': 'application/json'
    }
});

// Перехватчик для добавления токена в каждый запрос
api.interceptors.request.use(
    (config) => {
        const token = localStorage.getItem('authToken');
        if (token) {
            // Если токен есть, добавляем его в заголовки
            config.headers['Authorization'] = `Bearer ${token}`;
        }
        return config;
    },
    (error) => {
        return Promise.reject(error);
    }
);

api.interceptors.response.use(
    (response) => {
        return response;
    },
    (error) => {
        if (error.response && error.response.status === 401) {
            this.$router.push("/login");
        }
        return Promise.reject(error);
    }
);

export default api;

import axios from 'axios'

const api = axios.create({
    baseURL: import.meta.env.VITE_API_BASE_URL,
    headers: {
        'Content-Type': 'application/json',
    },
})

api.interceptors.request.use((config) => {
    const authData = localStorage.getItem('auth')
    if (authData) {
        const parsedAuth = JSON.parse(authData)
        const token = parsedAuth?.token
        if (token) {
            config.headers.Authorization = `Bearer ${token}`
        }
    }
    return config
})

api.interceptors.response.use(
    response => response,
    error => {
        if (error.response && (error.response.status === 401 || error.response.status === 403)) {
            localStorage.removeItem('auth')
            window.location.href = '/login'
        }
        return Promise.reject(error)
    }
)

export default api
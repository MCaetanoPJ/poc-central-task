import { defineStore } from 'pinia'
import { useToast } from 'vue-toastification'
import api from './../services/axios';

export const useUserStore = defineStore('user', {
    state: () => ({
        user: null,
        token: null,
        loading: false,
        error: null,
    }),

    actions: {
        async login(email, senha, router) {
            this.error = null
            if (!email || !senha) {
                this.error = 'Please fill in email and password.'
                return false
            }

            this.loading = true
            const toast = useToast()

            try {
                const { data } = await api.post('User/login', { email, senha })

                this.user = {
                    name: data.nome,
                    email: data.email,
                    userId: data.userId,
                    nivel: data.nivel,
                }
                this.token = data.accessToken

                localStorage.setItem('auth', JSON.stringify({
                    token: this.token,
                    user: this.user,
                    expiresInSeconds: data.expiresInSeconds,
                }))

                toast.success('Login successful!')
                router.push('/dashboard')
                return true
            } catch (err) {
                this.error = err?.response?.data?.errors?.[0] ||
                    err?.message ||
                    'An unexpected error occurred.'
                toast.error(this.error)
                return false
            } finally {
                this.loading = false
            }
        },

        logout(router) {
            this.user = null
            this.token = null
            localStorage.removeItem('auth')
            router.push('/login')
        },

        loadFromStorage() {
            const auth = JSON.parse(localStorage.getItem('auth'))
            if (auth && auth.token && auth.user) {
                this.token = auth.token
                this.user = auth.user
            }
        }
    }
})

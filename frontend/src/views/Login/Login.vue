<script setup>
import { ref } from 'vue'
import { useUserStore } from '../../stores/user'
import { useRouter } from 'vue-router'

const userStore = useUserStore()
const email = ref('')
const password = ref('')

const router = useRouter()

const loading = ref(false)

async function login() {
    await userStore.login(email.value, password.value, router)
}
</script>

<template>
    <main class="login">
        <h2>Login</h2>
        <form @submit.prevent="login">
            <label for="email">Email</label>
            <input id="email" type="email" v-model="email" placeholder="email@example.com" required />

            <label for="password">password</label>
            <input id="password" type="password" v-model="password" placeholder="Your password" required />

            <button type="submit" :disabled="loading">{{ loading ? 'Logging in...' : 'Log in' }}</button>
        </form>
    </main>
</template>

<style src="./Login.css" scoped></style>
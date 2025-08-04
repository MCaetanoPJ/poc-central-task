<template>
    <section class="greeting">
        <h2 class="title">
            What's up, <span class="name">{{ pascalCaseName }}</span>!
        </h2>

        <button @click="logout" class="logout-button">Logout</button>
    </section>
</template>

<script setup>
import { computed } from 'vue'
import { useRouter } from 'vue-router';
import { useUserStore } from '../stores/user';

const userStore = useUserStore()
const router = useRouter()

const pascalCaseName = computed(() => {
    if (!userStore.user?.name) return ''
    return userStore.user.name
        .split(' ')
        .map(word => word.charAt(0).toUpperCase() + word.slice(1).toLowerCase())
        .join(' ')
})

const logout = () => {
    userStore.logout(router)
}
</script>

<style scoped>
.greeting {
    display: flex;
    align-items: center;
    justify-content: space-between;
    margin-bottom: 1.5rem;
}

.greeting .title {
    display: flex;
}

.greeting .title,
.greeting .title .name {
    color: var(--dark);
    font-size: 1.5rem;
    font-weight: 700;
}

.greeting .title .name {
    margin: 0 0.5rem;
    flex: 1 1 0%;
    min-width: 0;
    color: var(--personal);
}

.logout-button {
    background-color: #333;
    color: white;
    border: none;
    padding: 0.5rem 1rem;
    margin-top: 0.5rem;
    cursor: pointer;
    border-radius: 4px;
    transition: background-color 0.3s;
}

.logout-button:hover {
    background-color: #555;
}
</style>

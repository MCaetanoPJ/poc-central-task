<template>
    <div style="display: none"></div>
</template>

<script setup>
import { onMounted } from 'vue'
import { HubConnectionBuilder } from '@microsoft/signalr'
import { useToast } from 'vue-toastification'

const toast = useToast()

let connection = null;
let isConnected = false;

onMounted(() => {
    if (isConnected) return

    const auth = JSON.parse(localStorage.getItem('auth') || '{}')
    const token = auth.token || '';

    if (!token) {
        console.warn('Não há token. Não vai conectar ao SignalR.')
        return
    }

    connection = new HubConnectionBuilder()
        .withUrl(import.meta.env.VITE_SIGNALR_URL, {
            accessTokenFactory: () => token
        })
        .withAutomaticReconnect()
        .build()

    function showNotification({ type, data }) {
        const icon = type === 'TaskUpdated' ? '♻️' : '📥'
        toast.info(`${icon} ${data.message}`, {
            timeout: 5000,
            closeOnClick: true,
            pauseOnHover: true,
        })
    }

    connection.on('NotificationTaskCreated', showNotification)
    connection.on('NotificationTaskUpdated', showNotification)

    connection
        .start()
        .then(() => {
            console.log('✅ Conectado ao SignalR')
            isConnected = true
        })
        .catch(err => console.error('❌ Erro ao conectar ao SignalR:', err))
})
</script>

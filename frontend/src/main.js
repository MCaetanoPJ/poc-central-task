import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import './main.css'

import { createPinia } from 'pinia'

import Toast from 'vue-toastification'
import 'vue-toastification/dist/index.css'

const app = createApp(App)
const pinia = createPinia()

app.use(router)
app.use(Toast)
app.use(pinia)
app.mount('#app')
import { createRouter, createWebHistory } from 'vue-router'
import Login from '../views/Login/Login.vue'
import Dashboard from '../views/Dashboard/Dashboard.vue'
import { useUserStore } from '../stores/user'

const routes = [
  { path: '/', redirect: '/login' },
  { path: '/login', component: Login },
  { path: '/dashboard', component: Dashboard },
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

router.beforeEach((to, from, next) => {
  const userStore = useUserStore()

  if (!userStore.user) {
    userStore.loadFromStorage()
  }

  const isAuthenticated = !!userStore.token

  if (to.path === '/dashboard' && !isAuthenticated) {
    next('/login')
  } else if (to.path === '/login' && isAuthenticated) {
    next('/dashboard')
  } else {
    next()
  }
})

export default router

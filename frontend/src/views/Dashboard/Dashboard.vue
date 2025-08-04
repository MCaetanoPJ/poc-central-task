<script setup>
import { ref, onMounted } from 'vue'
import Greeting from '../../components/Greeting.vue'
import TodoList from '../../components/TodoList.vue'
import CreateEditTodo from '../../components/CreateEditTodo.vue'
import api from '../../services/axios'
import { useToast } from 'vue-toastification'

const name = ref('')
const todos = ref([])
const users = ref([])
const editTodo = ref(null)

const toast = useToast()

const pageNumber = ref(0)
const pageSize = ref(5)
const totalPages = ref(1)

async function fetchTodos() {
	try {
		const { data } = await api.get('Tasks/paginado', {
			params: {
				PageSize: pageSize.value,
				PageNumber: pageNumber.value,
			}
		})

		todos.value = data.result || []

		totalPages.value = data.pagination.totalPages
	} catch (err) {
		todos.value = []
		toast.error('Failed to load todos')
	}
}

async function fetchUsers() {
	try {
		const { data } = await api.get('User')

		users.value = data.result || []
	} catch (err) {
		users.value = []
		toast.error('Failed to load todos')
	}
}

onMounted(() => {
	fetchTodos()
	fetchUsers()
})

const goPrevious = () => {
	if (pageNumber.value > 0) {
		pageNumber.value--
		fetchTodos()
	}
}

const goNext = () => {
	if (pageNumber.value < totalPages.value - 1) {
		pageNumber.value++
		fetchTodos()
	}
}

const addTodo = async (todo) => {
	try {
		await api.post('Tasks', todo)

		fetchTodos()

		toast.success('Todo added successfully!')
	} catch (err) {
		const errorMessage = err?.response?.data?.errors?.[0] ||
			err?.message ||
			'An unexpected error occurred.'

		toast.error(errorMessage)
	}
}

const updateTodo = async (updatedTodo) => {
	try {
		await api.put('Tasks', { ...updatedTodo })

		fetchTodos()

		toast.success('Todo updated successfully!')
	} catch (err) {
		const errorMessage = err?.response?.data?.errors?.[0] ||
			err?.message ||
			'An unexpected error occurred.'

		toast.error(errorMessage)
	}
}

const editExistingTodo = (todo) => {
	editTodo.value = { ...todo }
}

const removeTodo = async (todo) => {
	try {
		await api.delete('Tasks', {
			data: {
				id: todo.id,
			},
		})

		fetchTodos()

		toast.success('Todo removed successfully!')
	} catch (err) {
		const errorMessage =
			err?.response?.data?.errors?.[0] ||
			err?.message ||
			'An unexpected error occurred.'

		toast.error(errorMessage)
	}
}
</script>

<template>
	<main class="app">
		<Greeting v-model="name" />

		<CreateEditTodo :editTodo="editTodo" :users="users" @add="addTodo" @update="updateTodo" />

		<TodoList :todos="todos" @remove="removeTodo" @edit="editExistingTodo" />

		<div v-if="todos.length" class="pagination">
			<button @click="goPrevious" :disabled="pageNumber === 0">Previous</button>
			<span>Page {{ pageNumber + 1 }} of {{ totalPages }}</span>
			<button @click="goNext" :disabled="pageNumber >= totalPages - 1">Next</button>
		</div>
	</main>
</template>

<style scoped>
.pagination {
	display: flex;
	justify-content: center;
	align-items: center;
	margin-top: 1rem;
	gap: 1rem;
}

.pagination button:disabled {
	opacity: 0.5;
	cursor: not-allowed;
}
</style>

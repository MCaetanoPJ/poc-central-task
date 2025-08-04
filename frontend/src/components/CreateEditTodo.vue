<script setup>
import { ref, defineProps, defineEmits, watch } from 'vue'
import { useUserStore } from '../stores/user'

const props = defineProps({
    editTodo: {
        type: Object,
        default: null
    },
    users: {
        type: Array,
        required: true
    }
})

const emit = defineEmits(['add', 'update'])

const userStore = useUserStore()

const showForm = ref(false)
const input_title = ref('')
const input_description = ref('')
const input_dueDate = ref('')
const input_status = ref(0)
const input_userId = ref(userStore.user.userId || '')

watch(() => props.editTodo, (newVal) => {
    if (newVal) {
        input_title.value = newVal.title
        input_description.value = newVal.description
        input_dueDate.value = new Date(newVal.dueDate).toISOString().split('T')[0]
        input_status.value = newVal.status
        input_userId.value = newVal.userId || userStore.user.userId || ''
        showForm.value = true
    }
})

const toggleForm = () => {
    showForm.value = !showForm.value
}

const resetForm = () => {
    input_title.value = ''
    input_description.value = ''
    input_dueDate.value = ''
    input_status.value = 0
}

const saveTodo = () => {
    if (
        input_title.value.trim() === '' ||
        input_description.value.trim() === '' ||
        input_dueDate.value === ''
    ) {
        return
    }

    const todoData = {
        title: input_title.value,
        description: input_description.value,
        dueDate: input_dueDate.value,
        userId: input_userId.value,
        status: input_status.value
    }

    if (props.editTodo) {
        emit('update', { ...todoData, id: props.editTodo.id })
    } else {
        emit('add', todoData)
    }

    resetForm()
    toggleForm()
}
</script>

<template>
    <section class="create-todo">
        <button @click="toggleForm" class="toggle-button">
            Add New Todo
        </button>

        <div v-if="showForm" class="modal-overlay">
            <div class="modal-content">
                <div class="header-row">
                    <h3></h3>

                    <button @click="{ toggleForm(); resetForm(); }" class="toggle-button">
                        Close
                    </button>
                </div>

                <form id="new-todo-form" @submit.prevent="saveTodo">
                    <h4>Title</h4>
                    <input type="text" name="title" placeholder="Title" v-model="input_title" required />

                    <h4>Description</h4>
                    <textarea name="description" placeholder="Description" v-model="input_description"
                        required></textarea>

                    <div class="input-row">
                        <div class="input-group">
                            <h4>Due Date</h4>
                            <input type="date" name="dueDate" v-model="input_dueDate" required />
                        </div>

                        <div class="input-group">
                            <h4>Status</h4>
                            <select v-model.number="input_status">
                                <option :value="0">Not started</option>
                                <option :value="1">In progress</option>
                                <option :value="2">Completed</option>
                            </select>
                        </div>

                        <div class="input-group">
                            <h4>User</h4>
                            <select :disabled="!props.editTodo" v-model="input_userId" required>
                                <option disabled value="">Select a user</option>
                                <option v-for="user in users" :key="user.id" :value="user.id">
                                    {{ user.nome }}
                                </option>
                            </select>
                        </div>
                    </div>

                    <input type="submit" value="Submit" />
                </form>
            </div>
        </div>
    </section>
</template>

<style scoped>
.create-todo .header-row {
    display: flex;
    align-items: center;
    justify-content: space-between;
    margin-bottom: 1rem;
    gap: 1rem;
}

.create-todo .header-row h3 {
    margin: 0;
    font-weight: 700;
    font-size: 1.5rem;
}

.toggle-button {
    padding: 0.5rem 1rem;
    font-size: 1rem;
    background-color: var(--primary);
    color: white;
    border: none;
    border-radius: 0.4rem;
    cursor: pointer;
    transition: all 0.3s ease;
}

.toggle-button:hover {
    opacity: 0.8;
}

.modal-overlay {
    position: fixed;
    top: 0;
    left: 0;
    width: 100vw;
    height: 100vh;
    background-color: rgba(0, 0, 0, 0.5);
    display: flex;
    justify-content: center;
    align-items: center;
    z-index: 999;
}

.modal-content {
    background: white;
    padding: 2rem;
    border-radius: 0.5rem;
    width: 90%;
    max-width: 500px;
    position: relative;
    box-shadow: 0 0 15px rgba(0, 0, 0, 0.3);
}

.create-todo .input-row {
    display: flex;
    gap: 1rem;
    flex-wrap: wrap;
    margin-bottom: 1.5rem;
}

.create-todo .input-group {
    flex: 1;
    min-width: 150px;
    display: flex;
    flex-direction: column;
}

.create-todo .input-group h4 {
    margin-bottom: 0.5rem;
}

.create-todo input[type="text"],
.create-todo textarea,
.create-todo input[type="date"],
.create-todo select {
    display: block;
    width: 100%;
    font-size: 1.125rem;
    padding: 1rem 1.5rem;
    color: var(--dark, #333);
    background-color: #fff;
    border-radius: 0.5rem;
    box-shadow: var(--shadow, 0 2px 5px rgba(0, 0, 0, 0.1));
    margin-bottom: 1.5rem;
    border: 1px solid #ccc;
    font-family: inherit;
    resize: vertical;
    box-sizing: border-box;
}

.create-todo textarea {
    min-height: 80px;
}

.create-todo input[type="submit"] {
    display: block;
    width: 100%;
    font-size: 1.125rem;
    padding: 1rem 1.5rem;
    color: #fff;
    background-color: var(--primary, #007bff);
    border-radius: 0.5rem;
    box-shadow: var(--personal-glow, 0 0 8px rgba(0, 123, 255, 0.7));
    cursor: pointer;
    transition: 0.2s ease-in-out;
    border: none;
}

.create-todo input[type="submit"]:hover {
    opacity: 0.75;
}
</style>

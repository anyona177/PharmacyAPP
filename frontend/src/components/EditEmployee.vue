<template>
    <div class="edit-employee" v-if="employee">
        <h1>Редактировать сотрудника</h1>
        <form @submit.prevent="updateEmployee">
            <div class="form-group">
                <label>Имя:</label>
                <input v-model="name" required />
                <p v-if="errors.name" class="error">{{ errors.name }}</p>
            </div>
            <div class="form-group">
                <label>Фамилия:</label>
                <input v-model="surname" required />
                <p v-if="errors.surname" class="error">{{ errors.surname }}</p>
            </div>
            <div class="form-group">
                <label>Отчество:</label>
                <input v-model="patronymic" />
            </div>
            <div class="form-group">
                <label>Телефон:</label>
                <input type="tel" v-model="phone" required />
                <p v-if="errors.phone" class="error">{{ errors.phone }}</p>
            </div>
            <div class="form-group">
                <label>Должность:</label>
                <input v-model="post" required />
                <p v-if="errors.post" class="error">{{ errors.post }}</p>
            </div>
            <button class="save-button" type="submit">Сохранить</button>
            <button class="cancel-button" @click="$emit('cancel')">Отмена</button>
        </form>
    </div>
    <div v-else>
        <p>Загрузка данных...</p>
    </div>
</template>

<script>
    import axios from '@/config/axios';

export default {
    props: {
        employee: Object, 
    },
    data() {
        return {
            name: this.employee?.name|| '', 
            surname: this.employee?.surname || '',
            patronymic: this.employee?.patronymic || '',
            phone: this.employee?.phone || '',
            post: this.employee?.post || '', 
            errors: {} 
        };
    },
    methods: {
        async updateEmployee() {
            this.errors = {}; 

            if (!this.name) {
                this.errors.name = "Имя сотрудника обязательно";
            }
            if (!this.surname) {
                this.errors.surname = "Фамилия сотрудника обязательна";
            }
            if (!this.phone) {
                this.errors.phone = "Телефон обязателен";
            } else if (!/^(\+7|8)[0-9]{10}$/.test(this.phone)) {
                this.errors.phone = "Неверный формат телефона";
            }
            if (!this.post) {
                this.errors.post = "Должность обязательна";
            }

            if (Object.keys(this.errors).length > 0) {
                return;
            }

            try {
                await axios.put(`http://localhost:5062/pharmacy/employees/${this.employee.employeeId}`, {
                    name: this.name,
                    surname: this.surname,
                    patronymic: this.patronymic,
                    phone: this.phone,
                    post: this.post,
                });
                this.$emit('employeeUpdated'); 
            } catch (error) {
                console.error("Ошибка при обновлении сотрудника:", error);
            }
        }
    },
};
</script>

<style scoped>
    * {
        box-sizing: border-box;
        margin: 0;
        padding: 0;
        overflow-y: auto;
    }

    .edit-employee {
        display: flex;
        align-items: center;
        justify-content: center;
        flex-direction: column;
        position: absolute;
        top: 0;
        left: 0;
        width: 100vw;
        height: 100vh;
        background-color: #333;
        color: #fff;
        text-align: center;
    }

    h1 {
        font-size: 2.5em;
        margin-bottom: 20px;
    }

    .form-group {
        margin-bottom: 15px;
        width: 100%;
        max-width: 320px;
    }

    label {
        display: block;
        font-size: 1em;
        margin-bottom: 5px;
    }

    input {
        width: 100%;
        max-width: 300px;
        padding: 8px;
        font-size: 1em;
        margin-bottom: 10px;
        border-radius: 5px;
        border: 1px solid #ddd;
    }

    .save-button, .cancel-button {
        padding: 10px 20px;
        font-size: 1.2em;
        color: white;
        border: 3px solid white;
        border-radius: 9px;
        transition: background 0.3s;
        background-color: transparent;
        position: relative;
        cursor: pointer;
    }

        .save-button:hover, .cancel-button:hover {
            background-color: white;
            color: #000;
        }

    .save-button {
        margin-right: 25px;
    }

    .cancel-button {
        margin-left: 25px;
    }

    .error {
        color: red;
        font-size: 0.9em;
        margin-top: 5px;
    }
</style>

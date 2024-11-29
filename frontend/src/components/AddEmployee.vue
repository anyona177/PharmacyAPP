<template>
    <div class="add-employee">
        <img src="@/assets/back-arrow.png" alt="Back" class="back-icon" @click="goBack" />
        <h1>Добавить сотрудника</h1>
        <form @submit.prevent="addEmployee">
            <div class="form-group">
                <label for="name">Имя:</label>
                <input type="text" v-model="employee.name" />
                <div v-if="errors.name" class="error-message">{{ errors.name }}</div>
            </div>
            <div class="form-group">
                <label for="patronymic">Отчество (не обязательно):</label>
                <input type="text" v-model="employee.patronymic" />
            </div>
            <div class="form-group">
                <label for="surname">Фамилия:</label>
                <input type="text" v-model="employee.surname" />
                <div v-if="errors.surname" class="error-message">{{ errors.surname }}</div>
            </div>
            <div class="form-group">
                <label for="phone">Телефон:</label>
                <input type="tel" v-model="employee.phone" placeholder="+79991234567" />
                <div v-if="errors.phone" class="error-message">{{ errors.phone }}</div>
            </div>
            <div class="form-group">
                <label for="post">Должность:</label>
                <input type="text" v-model="employee.post" />
                <div v-if="errors.post" class="error-message">{{ errors.post }}</div>
            </div>
            <button class="save-button" type="submit">Добавить</button>
        </form>
    </div>
</template>

<script>
    import axios from '@/config/axios';

    export default {
        data() {
            return {
                employee: {
                    name: '',          
                    patronymic: '',
                    surname: '',      
                    phone: '',
                    post: '',
                },
                errors: {
                    name: '',
                    surname: '',
                    phone: '',
                    post: '',
                },
            };
        },
        methods: {
            async addEmployee() {
                if (this.isValid()) {
                    try {
                        const response = await axios.post('http://localhost:5062/pharmacy/employees', this.employee);
                        if (response.status === 201) {
                            this.$router.push('/employees'); 
                        }
                    } catch (error) {
                        console.error("Ошибка при добавлении сотрудника:", error);
                        
                        alert("Произошла ошибка при добавлении сотрудника.");
                    }
                }
            },
            isValid() {
                let isValid = true;

                this.errors = {
                    name: '',
                    surname: '',
                    phone: '',
                    post: '',
                };

                if (!this.employee.name.trim()) {
                    this.errors.name = 'Обязательное поле';
                    isValid = false;
                }

                if (!this.employee.surname.trim()) {
                    this.errors.surname = 'Обязательное поле';
                    isValid = false;
                }

                if (!this.employee.phone.trim()) {
                    this.errors.phone = 'Обязательное поле';
                    isValid = false;
                } else if (!/^(\+7|8)[0-9]{10}$/.test(this.employee.phone)) {
                    this.errors.phone = 'Неверный формат';
                    isValid = false;
                }

                if (!this.employee.post.trim()) {
                    this.errors.post = 'Обязательное поле';
                    isValid = false;
                }

                return isValid;
            },
            goBack() {
                this.$router.push('/employees');
            }
        }
    };
</script>

<style scoped>
    * {
        box-sizing: border-box;
        margin: 0;
        padding: 0;
        overflow-y: auto;
    }

    .add-employee {
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

    input, select {
        width: 100%;
        max-width: 300px;
        padding: 8px;
        font-size: 1em;
        margin-bottom: 10px;
        border-radius: 5px;
        border: 1px solid #ddd;
    }

    .back-icon {
        width: 30px;
        height: 30px;
        filter: invert(100%);
        cursor: pointer;
        position: absolute;
        transition: filter 0.3s;
        top: 20px;
        left: 20px;
        border-radius: 50%;
        padding: 5px;
    }

        .back-icon:hover {
            filter: invert(70%);
        }

    .save-button {
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

        .save-button:hover {
            background-color: white;
            color: #000;
        }

    .error-message {
        color: red;
        font-size: 0.9em;
        height: 100%;
        width: 100%;
        text-align: left;
    }
</style>
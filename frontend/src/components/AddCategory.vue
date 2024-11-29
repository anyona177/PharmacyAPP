<template>
    <div class="add-category">
        <img src="@/assets/back-arrow.png" alt="Back" class="back-icon" @click="goBack" />
        <h1>Добавить категорию</h1>
        <div class="form-group">
            <label>Название категории</label>
            <input type="text" v-model="categoryName" />
            <span v-if="nameError && !errorMessage" class="error-message">{{ nameError }}</span>
            <span v-if="errorMessage" class="error-message">{{ errorMessage }}</span>
        </div>
        <button class="save-button" @click="addCategory">Сохранить</button>
    </div>
</template>

<script>
    import axios from '@/config/axios';

    export default {
        name: 'AddCategory',
        data() {
            return {
                categoryName: '',
                nameError: '',
                errorMessage: ''
            };
        },
        methods: {
            async addCategory() {
                // Проверка на пустое название категории
                if (!this.categoryName.trim()) {
                    this.nameError = 'Название категории обязательно для заполнения';
                    return;
                }
                this.nameError = '';

                // Отправка данных на сервер
                try {
                    await axios.post('http://localhost:5062/pharmacy/categories', {
                        categoryName: this.categoryName
                    });
                    this.categoryName = ''; 
                    this.errorMessage = '';

                    this.$router.push({ name: 'Categories' });
                } catch (error) {
                    this.errorMessage = 'Права доступны только администратору';
                }
            },
             goBack() {
                 this.$router.push('/categories');
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

    .add-category {
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
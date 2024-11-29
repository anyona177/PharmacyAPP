<template>
    <div class="edit-category">
        <h1>Редактировать категорию</h1>
        <form @submit.prevent="updateProduct">
            <div class="form-group">
                <label>Название категории</label>
                <input type="text" v-model="categoryName" placeholder="Введите название категории" />
                <span v-if="nameError && !errorMessage" class="error-message">{{ nameError }}</span>
                <span v-if="errorMessage" class="error-message">{{ errorMessage }}</span>
            </div>
            <button class="save-button" @click="updateCategory">Сохранить</button>
            <button class="cancel-button" @click="$emit('cancel')">Отмена</button>
        </form>
    </div>
</template>

<script>
    import axios from '@/config/axios';

    export default {
        name: 'EditCategory',
        props: {
            category: {
                type: Object,
                required: true,
            },
        },
        data() {
            return {
                categoryName: this.category.categoryName,
                nameError: '',
                errorMessage: '',
            };
        },
        methods: {
            async updateCategory() {
                if (!this.categoryName) {
                    this.nameError = 'Название категории обязательно для заполнения';
                    return;
                }
                this.nameError = '';

                try {
                    const response = await axios.put(`http://localhost:5062/pharmacy/categories/${this.category.categoryId}`, {
                        categoryName: this.categoryName,
                    });
                    this.$emit('categoryUpdated', response.data);
                } catch (error) {
                    this.errorMessage = 'Права доступны только администратору';
                    console.error('Ошибка при обновлении категории:', error);
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

    .edit-category {
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

    .error-message {
        color: red;
        font-size: 0.9em;
        height: 100%;
        width: 100%;
        text-align: left;
    }
</style>

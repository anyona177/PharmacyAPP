<template>
    <div class="confirmation-dialog">
        <div class="dialog-content">
            <h2>Подтверждение удаления</h2>
            <p v-if="category && category.categoryName">Вы уверены, что хотите удалить категорию "{{ category.categoryName }}"?</p>
            <p v-else>Категория не найдена.</p>
            <div class="error-container">
                <span v-if="errorMessage" class="error-message">{{ errorMessage }}</span>
            </div>
                <button class="yes-button" @click="$emit('confirm')">Да</button>
                <button class="no-button" @click="$emit('cancel')">Нет</button>
            </div>
        </div>
</template>

<script>
    export default {
        props: {
            category: {
                type: Object,
                required: true,
            },
            errorMessage: {
                type: String,
                default: '',
            },
        },
    };
</script>

<style scoped>
    .confirmation-dialog {
        position: fixed;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        background-color: rgba(0, 0, 0, 0.5);
        display: flex;
        align-items: center;
        justify-content: center;
    }

    .dialog-content {
        background-color: #fff;
        padding: 20px;
        border-radius: 5px;
        background-color: #333;
        color: #fff;
        text-align: center;
        border: 3px solid white;
    }

    .yes-button, .no-button {
        display: inline-block;
        padding: 10px 20px;
        font-size: 1.2em;
        color: white;
        border: 3px solid white;
        border-radius: 9px;
        text-decoration: none;
        transition: background 0.3s;
        background-color: transparent;
        cursor: pointer;
    }

        .yes-button:hover, .no-button:hover {
            background-color: white;
            color: #000;
        }

    .yes-button {
        margin-right: 30px;
    }

    .no-button {
        margin-left: 30px;
    }

    .error-message {
        color: red;
        font-size: 0.9em;
        height: 100%;
        width: 100%;
        text-align: left;
    }

    .error-container {
        margin-top: 10px;
        margin-bottom: 20px;
    }
</style>

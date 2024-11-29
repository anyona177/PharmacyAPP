<template>
    <div class="confirmation-dialog">
        <div class="dialog-content">
            <h2>Подтверждение удаления</h2>
            <p v-if="employee && employee.name">Вы уверены, что хотите удалить сотрудника "{{ employee.surname + " " + employee.name + " " + employee.patronymic }}"?</p>
            <p v-else>Сотрудник не найден.</p>
            <button class="yes-button" @click="$emit('confirm')">Да</button>
            <button class="no-button" @click="$emit('cancel')">Нет</button>
        </div>
    </div>
</template>

<script>
    export default {
        props: {
            employee: {
                type: Object,
                required: true,
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
</style>

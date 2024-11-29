<template>
    <div class="employees-page">
        <img src="@/assets/back-arrow.png" alt="Back" class="back-icon" @click="goBack" />
        <h1>Сотрудники</h1>
        <div v-if="editingEmployee">
            <EditEmployee :employee="editingEmployee"
                          @employeeUpdated="handleEmployeeUpdated"
                          @cancel="cancelEditing" />
        </div>
        <div v-else class="table-container">
            <table>
                <thead>
                    <tr>
                        <th>id</th>
                        <th>фамилия</th>
                        <th>имя</th>
                        <th>отчество</th>
                        <th>должность</th>
                        <th>телефон</th>
                        <th>действия</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="employee in employees" :key="employee.employeeId">
                        <td>{{ employee.employeeId }}</td>
                        <td>{{ employee.surname }}</td>
                        <td>{{ employee.name }}</td>
                        <td>{{ employee.patronymic }}</td>
                        <td>{{ employee.post }}</td>
                        <td>{{ employee.phone }}</td>
                        <td class="action-buttons">
                            <img src="@/assets/edit-icon.png" alt="Edit" class="action-icon" @click="editEmployee(employee)" />
                            <img src="@/assets/delete-icon.png" alt="Delete" class="action-icon" @click="confirmDelete(employee)" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <button v-show="false" v-if="!editingEmployee" class="add-button" @click="openAddEmployeePage">Добавить</button>
        <ConfirmationDialog v-if="deletingEmployee"
                            :employee="deletingEmployee"
                            @confirm="deleteEmployee(deletingEmployee)"
                            @cancel="deletingEmployee = null" />
    </div>
</template>

<script>
    import axios from '@/config/axios';
    import EditEmployee from '@/components/EditEmployee.vue';
    import ConfirmationDialog from '@/components/ConfirmationDialogEmployee.vue';

    export default {
        components: {
            EditEmployee,
            ConfirmationDialog,
        },
        data() {
            return {
                employees: [],
                editingEmployee: null, 
                deletingEmployee: null, 
            };
        },
        created() {
            this.fetchEmployees();
        },
        methods: {
            async fetchEmployees() {
                try {
                    const response = await axios.get('http://localhost:5062/pharmacy/employees');
                    this.employees = response.data.data;
                } catch (error) {
                    console.error('Ошибка при получении данных:', error);
                }
            },
            openAddEmployeePage() {
                this.$router.push('/add-employee');
            },
            goBack() {
                this.$router.push('/home');
            },
            editEmployee(employee) {
                this.editingEmployee = employee;
            },
            cancelEditing() {
                this.editingEmployee = null; 
            },
            handleEmployeeUpdated() {
                this.cancelEditing(); 
                this.fetchEmployees(); 
            },
            confirmDelete(employee) {
                this.deletingEmployee = employee; 
            },
            async deleteEmployee(employee) {
                try {
                    await axios.delete(`http://localhost:5062/pharmacy/employees/${employee.employeeId}`);
                    this.fetchEmployees(); 
                    this.deletingEmployee = null; 
                } catch (error) {
                    console.error("Ошибка при удалении сотрудника:", error);
                }
            },
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

    .employees-page {
        position: absolute;
        top: 0;
        left: 0;
        padding: 20px;
        width: 100vw;
        height: 100vh;
        background-color: #333;
        text-align: center;
        color: #fff;
    }

    h1 {
        font-size: 3em;
        margin-bottom: 20px;
    }

    .table-container {
        height: calc(100vh - 200px);
        background-color: #555;
        border-radius: 10px;
        padding: 10px;
        border: 2.2px solid #fff;
    }

    table {
        width: 100%;
        border-spacing: 0;
        border: 2px solid #fff;
        margin-top: 20px;
        background-color: #777;
        border-radius: 20px;
        overflow: hidden;
    }

    th, td {
        padding: 10px;
        text-align: center;
        border-top: 1px solid #ffffff;
        border-left: 1px solid #ffffff;
    }

    .action-icon {
        width: 24px;
        height: 24px;
        filter: invert(100%);
        cursor: pointer;
        transition: filter 0.3s;
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

        .action-icon:hover, .back-icon:hover {
            filter: invert(70%);
        }

    .add-button {
        padding: 10px 20px;
        font-size: 1.2em;
        color: white;
        border: 3px solid white;
        border-radius: 9px;
        transition: background 0.3s;
        background-color: transparent;
        position: relative;
        top: 20px;
        left: 650px;
        cursor: pointer;
    }

        .add-button:hover {
            background-color: white;
            color: #000;
        }

</style>

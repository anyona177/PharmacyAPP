<template>
    <div class="categories-page">
        <img src="@/assets/back-arrow.png" alt="Back" class="back-icon" @click="goBack" />
        <h1>Категории</h1>
        <div v-if="editingCategory">
            <EditCategory :category="editingCategory"
                          @categoryUpdated="handleCategoryUpdated"
                          @cancel="cancelEditing" />
        </div>
        <div v-else class="table-container">
            <table v-if="categories.length">
                <thead>
                    <tr>
                        <th>id</th>
                        <th>название категории</th>
                        <th>действия</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="category in categories" :key="category.categoryId">
                        <td>{{ category.categoryId }}</td>
                        <td>{{ category.categoryName }}</td>
                        <td class="action-buttons">
                            <img src="@/assets/edit-icon.png" alt="Edit" class="action-icon" @click="editCategory(category)" />
                            <img src="@/assets/delete-icon.png" alt="Delete" class="action-icon" @click="confirmDelete(category)" />
                        </td>
                    </tr>
                </tbody>
            </table>
            <p v-else>Нет доступных категорий.</p>
        </div>
        <button v-if="!editingCategory" class="add-button" @click="openAddCategoryPage">Добавить</button>
        <ConfirmationDialog v-if="deletingCategory"
                            :category="deletingCategory"
                            :errorMessage="errorMessage"
                            @confirm="deleteCategory(deletingCategory)"
                            @cancel="deletingCategory = null" />
    </div>
</template>

<script>
    import axios from '@/config/axios';
    import EditCategory from '@/components/EditCategory.vue'; 
    import ConfirmationDialog from '@/components/ConfirmationDialogCategory.vue'; 

    export default {
        components: {
            EditCategory,
            ConfirmationDialog,
        },
        data() {
            return {
                categories: [],
                editingCategory: null,
                deletingCategory: null,
                errorMessage: '',
            };
        },
        created() {
            this.fetchCategories();
        },
        methods: {
            async fetchCategories() {
                try {
                    const response = await axios.get('http://localhost:5062/pharmacy/categories');
                    this.categories = response.data.data;
                } catch (error) {
                    console.error('Ошибка при загрузке категорий:', error);
                }
            },
            openAddCategoryPage() {
                this.$router.push('/add-category');
            },
            cancelEditing() {
                this.editingCategory = null; 
            },
            handleCategoryUpdated() {
                this.cancelEditing(); 
                this.fetchCategories(); 
            },
            editCategory(category) {
                this.editingCategory = category; 
            },
            confirmDelete(category) {
                this.deletingCategory = category; 
            },
            async deleteCategory(category) {
                try {
                    await axios.delete(`http://localhost:5062/pharmacy/categories/${category.categoryId}`);
                    this.fetchCategories(); 
                    this.deletingCategory = null; 
                } catch (error) {
                    this.errorMessage = 'Права доступны только администратору';
                    console.error("Ошибка при удалении категории:", error);
                }
            },
            goBack() {
                this.$router.push('/home');
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

    .categories-page {
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

<template>
    <div class="products-page">
        <img src="@/assets/back-arrow.png" alt="Back" class="back-icon" @click="goBack" />
        <h1>Товары</h1>
        <div v-if="editingProduct">
            <EditProduct :product="editingProduct"
                             :categories="categories"
                             @productUpdated="handleProductUpdated"
                             @cancel="cancelEditing" />
        </div>
        <div v-else class="table-container">
            <table v-if="products.length">
                <thead>
                    <tr>
                        <th>id</th>
                        <th>наименование</th>
                        <th>категория</th>
                        <th>кол-во</th>
                        <th>цена</th>
                        <th>поставщик</th>
                        <th>действия</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="product in products" :key="product.productId">
                        <td>{{ product.productId }}</td>
                        <td>{{ product.name }}</td>
                        <td>{{ product.category }}</td>
                        <td>{{ product.quantity }}</td>
                        <td>{{ product.price }}</td>
                        <td>{{ product.supplier }}</td>
                        <td class="action-buttons">
                            <img src="@/assets/edit-icon.png" alt="Edit" class="action-icon" @click="editProduct(product)" />
                            <img src="@/assets/delete-icon.png" alt="Delete" class="action-icon" @click="confirmDelete(product)" />
                        </td>
                    </tr>
                </tbody>
            </table>
            <p v-else>Нет доступных продуктов.</p>
        </div>
        <button v-if="!editingProduct" class="add-button" @click="openAddProductPage">Добавить</button>
        <ConfirmationDialog v-if="deletingProduct"
                            :product="deletingProduct"
                            @confirm="deleteProduct(deletingProduct)"
                            @cancel="deletingProduct = null" />
    </div>
</template>

<script>
    import axios from '@/config/axios';
    import EditProduct from '@/components/EditProduct.vue';
    import ConfirmationDialog from '@/components/ConfirmationDialogProduct.vue';

    export default {
        components: {
            EditProduct,
            ConfirmationDialog,
        },
        data() {
            return {
                products: [],
                editingProduct: null,
                deletingProduct: null,
                categories: [],
            };
        },
        created() {
            this.fetchProducts();
            this.fetchCategories();
        },
        methods: {
            async fetchProducts() {
                try {
                    const response = await axios.get('http://localhost:5062/pharmacy/products');
                    this.products = response.data.data;
                } catch (error) {
                    console.error("Ошибка при загрузке товаров:", error);
                }
            },
            async fetchCategories() {
                try {
                    const response = await axios.get('http://localhost:5062/pharmacy/categories');
                    this.categories = response.data.data;
                } catch (error) {
                    console.error("Ошибка при загрузке категорий:", error);
                }
            },
            goBack() {
                this.$router.push('/home');
            },
            openAddProductPage() {
                this.$router.push('/add-product');
            },
            editProduct(product) {
                this.editingProduct = product;
            },
            cancelEditing() {
                this.editingProduct = null; 
            },
            handleProductUpdated() {
                this.cancelEditing(); 
                this.fetchProducts(); 
            },
            confirmDelete(product) {
                this.deletingProduct = product;
            },
            async deleteProduct(product) {
                try {
                    await axios.delete(`http://localhost:5062/pharmacy/products/${product.productId}`);
                    this.fetchProducts(); 
                    this.deletingProduct = null;
                } catch (error) {
                    console.error("Ошибка при удалении товара:", error);
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

    .products-page {
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

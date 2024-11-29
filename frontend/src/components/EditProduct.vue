<template>
    <div class="edit-product">
        <h1>Редактировать товар</h1>
        <form @submit.prevent="updateProduct">
            <div class="form-group">
                <label>Название:</label>
                <input v-model="productName" required />
                <p v-if="errors.productName" class="error">{{ errors.productName }}</p>
            </div>
            <div class="form-group">
                <label>Категория:</label>
                <select v-model="selectedCategoryId" required>
                    <option value="" disabled>Выберите категорию</option>
                    <option v-for="category in localCategories" :key="category.categoryId" :value="category.categoryId">
                        {{ category.categoryName }}
                    </option>
                </select>
            </div>
            <div class="form-group">
                <label>Количество:</label>
                <input type="number" v-model.number="quantity" required @input="validateQuantity" />
                <p v-if="errors.quantity" class="error">{{ errors.quantity }}</p>
            </div>
            <div class="form-group">
                <label>Цена:</label>
                <input type="number" v-model.number="price" required @input="validatePrice" />
                <p v-if="errors.price" class="error">{{ errors.price }}</p>
            </div>
            <div class="form-group">
                <label>Поставщик:</label>
                <input v-model="supplier" required />
            </div>
            <button class="save-button" type="submit">Сохранить</button>
            <button class="cancel-button" @click="$emit('cancel')">Отмена</button>
        </form>
    </div>
</template>

<script>
    import axios from '@/config/axios';

    export default {
        props: {
            product: Object,
            categories: Array,
        },
        data() {
            return {
                productName: this.product.name,
                quantity: this.product.quantity,
                price: this.product.price,
                supplier: this.product.supplier,
                selectedCategoryId: null,
                localCategories: [],
                errors: {}
            };
        },
        methods: {
            validateQuantity() {
                if (this.quantity <= 0) {
                    this.errors.quantity = "Количество должно быть больше нуля";
                    this.quantity = 0;
                } else {
                    this.quantityError = '';
                }
            },
            validatePrice() {
                if (this.price <= 0) {
                    this.errors.price = "Цена должна быть больше нуля";
                    this.price = 0.0;
                } else {
                    this.priceError = '';
                }
            },
            async updateProduct() {
                this.validateQuantity();
                this.validatePrice();

                if (this.quantityError || this.priceError || this.productName === '' || this.supplier === '' || !this.selectedCategoryId) {
                    return;
                }

                try {
                    await axios.put(`http://localhost:5062/pharmacy/products/${this.product.productId}`, {
                        Name: this.productName,
                        Quantity: this.quantity,
                        Price: this.price,
                        Supplier: this.supplier,
                        CategoryId: this.selectedCategoryId,
                    });
                    this.$emit('productUpdated');
                } catch (error) {
                    console.error("Ошибка при обновлении товара:", error);
                }
            },
            async loadCategories() {
                try {
                    const response = await axios.get('http://localhost:5062/pharmacy/categories');
                    this.localCategories = response.data.data;
                    console.log(this.localCategories);

                    const category = this.localCategories.find(cat => cat.categoryName === this.product.category);
                    this.selectedCategoryId = category ? category.categoryId : null;
                } catch (error) {
                    console.error("Ошибка загрузки категорий:", error);
                }
            },
        },
        mounted() {
            this.loadCategories();
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

    .edit-product {
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

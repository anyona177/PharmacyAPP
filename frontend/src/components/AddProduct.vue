<template>
    <div class="add-product">
        <img src="@/assets/back-arrow.png" alt="Back" class="back-icon" @click="goBack" />
        <h1>Добавить товар</h1>
        <form @submit.prevent="addProduct">
            <div class="form-group">
                <label>Название:</label>
                <input v-model="productName" required />
            </div>

            <div class="form-group">
                <label>Категория:</label>
                <select v-model="selectedCategoryId" required >
                    <option value="" disabled>Выберите категорию</option>
                    <option v-for="category in categories" :key="category.id" :value="category.id">
                        {{ category.name }}
                    </option>
                </select>
            </div>

            <div class="form-group">
                <label>Количество:</label>
                <input type="number" v-model.number="quantity" required @input="validateQuantity" />
                <div v-if="quantityError" class="error-message">{{ quantityError }}</div>
            </div>

            <div class="form-group">
                <label>Цена:</label>
                <input type="number" v-model.number="price" required @input="validatePrice" />
                <div v-if="priceError" class="error-message">{{ priceError }}</div>
            </div>

            <div class="form-group">
                <label>Поставщик:</label>
                <input v-model="supplier" required />
            </div>

            <button class="save-button" type="submit">Сохранить</button>
        </form>
    </div>
</template>

<script>
    import axios from '@/config/axios';

    export default {
        data() {
            return {
                productName: '',
                quantity: 0,
                price: 0.0,
                supplier: '',
                selectedCategoryId: null,
                categories: [], 
                quantityError: '', 
                priceError: ''
            };
        },
        created() {
            this.fetchCategories();
        },
        methods: {
            async fetchCategories() {
                try {
                    const response = await axios.get('http://localhost:5062/pharmacy/categories');
                    this.categories = response.data.data.map(category => ({
                        id: category.categoryId,
                        name: category.categoryName
                    }));
                    console.log("Категории:", this.categories);
                } catch (error) {
                    console.error("Ошибка при загрузке категорий:", error);
                }
            },
            validateQuantity() {
                if (this.quantity <= 0) {
                    this.quantityError = "Количество должно быть больше нуля";
                    this.quantity = 0;
                } else {
                    this.quantityError = ''; 
                }
            },
            validatePrice() {
                if (this.price <= 0) {
                    this.priceError = "Цена должна быть больше нуля";
                    this.price = 0.0; 
                } else {
                    this.priceError = '';
                }
            },
            async addProduct() {
                this.validateQuantity(); 
                this.validatePrice(); 
            
                if (this.quantityError || this.priceError || this.productName === '' || this.supplier === '' || !this.selectedCategoryId) {
                    return; 
                }

                try {
                    await axios.post('http://localhost:5062/pharmacy/products', {
                        Name: this.productName,
                        Quantity: this.quantity,
                        Price: this.price,
                        Supplier: this.supplier,
                        CategoryId: this.selectedCategoryId
                    });
                 
                    this.$router.push({ name: 'Products' });
                } catch (error) {
                    console.error("Ошибка при создании товара:", error);

                    alert("Произошла ошибка при создании товара.");
                }
            },
            resetForm() {
                this.productName = '';
                this.quantity = 0;
                this.price = 0.0;
                this.supplier = '';
                this.selectedCategoryId = null;
                this.quantityError = ''; 
                this.priceError = '';
            },
            goBack() {
                this.$router.push('/products');
            }
        }
    }
</script>

<style scoped>
    * {
        box-sizing: border-box;
        margin: 0;
        padding: 0;
        overflow-y: auto;
    }

    .add-product {
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
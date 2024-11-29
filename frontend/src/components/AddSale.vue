<template>
    <div class="add-sale">
        <img src="@/assets/back-arrow.png" alt="Back" class="back-icon" @click="goBack" />
        <h1>Добавить Продажу</h1>
        <p>Сотрудник: {{ employeeName }}</p>
        <div class="form-group" v-if="soldItems.length">
            <label>Товар</label>
            <select v-model="soldItems[currentItemIndex].productId">
                <option v-for="product in filteredProducts" :key="product.productId" :value="product.productId">
                    {{ product.name }}
                </option>
            </select>
        </div>
        <div class="form-group">
            <label>Количество</label>
            <input type="number" v-model="soldItems[currentItemIndex].quantity" min="1" placeholder="Количество" @input="validateQuantity" />
            <div v-if="quantityError" class="error-message">{{ quantityError }}</div>
            <div>
                <button class="navigation-buttons" v-bind:disabled="currentItemIndex === 0" @click="prevItem">Назад</button>
                <span>Товар {{ currentItemIndex + 1 }} из {{ soldItems.length }}</span>
                <button class="navigation-buttons" v-bind:disabled="currentItemIndex === soldItems.length - 1" @click="nextItem">Далее</button>
            </div>
        </div>
        <span v-if="errorMessage" class="error-message">{{ errorMessage }}</span>
        <button class="navigation-buttons" @click="addNewItem">Добавить Товар</button>
        <button class="navigation-buttons" @click="createSale">Создать Продажу</button>
    </div>
</template>


<script>
    import axios from '@/config/axios';

    export default {
        name: 'AddSale',
        data() {
            return {
                sale: { employeeId: null },
                soldItems: [{ productId: null, quantity: 1 }],
                currentItemIndex: 0,
                products: [],
                employees: [],
                searchQuery: '',
                quantityError: '',
                errorMessage: ''
            };
        },
        computed: {
            employeeName() {
                const employee = this.employees.find(emp => emp.employeeId === this.sale.employeeId);
                return employee
                    ? `${employee.surname} ${employee.name} ${employee.patronymic}`
                    : 'Сотрудник не найден';
            },
            filteredProducts() {
                return this.products;
            }
        },
        created() {
            const token = localStorage.getItem('authToken'); 
            if (token) {
                try {
                    const payload = JSON.parse(atob(token.split('.')[1]));

                    if (payload.employeeId) {
                        this.sale.employeeId = parseInt(payload.employeeId, 10); 
                        this.fetchEmployee(payload.employeeId); 
                    } else {
                        console.error('ID сотрудника отсутствует в токене');
                    }
                } catch (error) {
                    console.error('Ошибка декодирования токена:', error);
                }
            }

            this.fetchProducts();
            this.fetchEmployees();
        },
        methods: {
            async fetchProducts() {
                try {
                    const response = await axios.get('http://localhost:5062/pharmacy/products');
                    this.products = response.data.data;
                } catch (error) {
                    console.error('Error fetching products:', error);
                }
            },
            async fetchEmployees() {
                try {
                    const response = await axios.get('http://localhost:5062/pharmacy/employees');
                    this.employees = response.data.data;
                } catch (error) {
                    console.error('Error fetching employees:', error);
                }
            },
            validateQuantity() {
                const currentItem = this.soldItems[this.currentItemIndex];
                if (currentItem.quantity <= 0) {
                    this.quantityError = 'Количество должно быть больше 0';
                } else {
                    this.quantityError = ''; 
                }
            },
            addNewItem() {
                const currentItem = this.soldItems[this.currentItemIndex];
                if (currentItem.quantity <= 0) {
                    this.quantityError = 'Количество должно быть больше нуля';
                    return;
                }
                this.quantityError = '';
                this.soldItems.push({ productId: null, quantity: 1 });
                this.currentItemIndex = this.soldItems.length - 1;
            },
            prevItem() {
                if (this.currentItemIndex > 0) {
                    this.currentItemIndex--;
                }
            },
            nextItem() {
                if (this.currentItemIndex < this.soldItems.length - 1) {
                    this.currentItemIndex++;
                }
            },
            async createSale() {
                for (const item of this.soldItems) {
                    if (item.quantity <= 0) {
                        this.quantityError = 'Количество должно быть больше нуля';
                        return;
                    }
                    if (!item.productId) {
                        this.quantityError = 'Не выбран товар';
                        return;
                    }
                }

                const productIds = this.soldItems.map(item => item.productId);
                const duplicateProducts = productIds.filter((item, index) => productIds.indexOf(item) !== index);

                if (duplicateProducts.length > 0) {
                    this.quantityError = 'Одинаковые товары нельзя добавить';
                    return;
                }

                try {
                    const response = await axios.post('http://localhost:5062/pharmacy/sales/create-sale', {
                        sale: this.sale,
                        soldItems: this.soldItems
                    });
                    console.log('Продажа создана:', response.data);
                    this.$router.push({ name: 'Sales' });
                } catch (error) {
                       this.errorMessage = 'Права доступны только фармацевту';
                }
            },
            goBack() {
                this.$router.push('/sales');
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

    .add-sale {
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

    .navigation-buttons {
        padding: 10px 20px;
        font-size: 1.2em;
        color: white;
        border: 3px solid white;
        border-radius: 9px;
        transition: background 0.3s;
        background-color: transparent;
        position: relative;
        cursor: pointer;
        margin-top: 10px;
        margin-left: 5px;
        margin-right: 5px;
    }

        .navigation-buttons:hover {
            background-color: white;
            color: #000;
        }

    .error-message {
        color: red;
        font-size: 0.9em;
        height: 20px;
        margin-left: 10px;
        width: 85%;
        text-align: center;
    }
</style>


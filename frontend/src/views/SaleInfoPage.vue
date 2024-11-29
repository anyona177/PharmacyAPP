<template>
    <div class="sale-info-page">
        <img src="@/assets/back-arrow.png" alt="Back" class="back-icon" @click="goBack" />
        <h1>Детализация продажи №{{ saleId }}</h1>
        <div v-if="soldItems.length" class="table-container">
            <table>
                <thead>
                    <tr>
                        <th>id</th>
                        <th>название товара</th>
                        <th>количество</th>
                        <th>цена</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="item in soldItems" :key="item.soldItemId">
                        <td>{{ item.soldItemId }}</td>
                        <td>{{ item.productName }}</td>
                        <td>{{ item.quantity }}</td>
                        <td>{{ item.price }}</td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div v-else>
            <p>Данные о проданных товарах не найдены.</p>
        </div>
    </div>
</template>

<script>
    import axios from '@/config/axios';

    export default {
        name: 'SaleInfoPage',
        props: {
            saleId: {
                type: Number,
                required: true,
            },
        },
        data() {
            return {
                soldItems: [],
            };
        },
        created() {
            this.fetchSoldItems();
        },
        methods: {
            async fetchSoldItems() {
                try {
                    const response = await axios.get(`http://localhost:5062/pharmacy/sales/solditems/${this.saleId}`);
                    this.soldItems = response.data.data;
                } catch (error) {
                    console.error('Ошибка при загрузке детализации:', error);
                    this.soldItems = [];
                }
            },
            goBack() {
                this.$router.push('/sales');
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

    .sale-info-page {
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
</style>

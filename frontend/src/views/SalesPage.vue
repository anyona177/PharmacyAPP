<template>
    <div class="sales-page">
        <img src="@/assets/back-arrow.png" alt="Back" class="back-icon" @click="goBack" />
        <h1>Продажи</h1>
        <div class="table-container">
            <table>
                <thead>
                    <tr>
                        <th>id</th>
                        <th>сотрудник</th>
                        <th>дата продажи</th>
                        <th>сумма</th>
                        <th>действия</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="sale in sales" :key="sale.saleId">
                        <td>{{ sale.saleId }}</td>
                        <td>{{ sale.employee }}</td>
                        <td>{{ sale.saleDate }}</td>
                        <td>{{ sale.totalAmount }}</td>
                        <td class="action-buttons">
                            <img src="@/assets/view-icon.png" alt="Review" class="action-icon" @click="reviewSale(sale)" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <button class="add-button" @click="openAddSalePage">Добавить</button>
    </div>
</template>

<script>
    import axios from '@/config/axios';

    export default {
        name: 'SalesPage',
        data() {
            return {
                sales: [],
            };
        },
        created() {
            this.fetchSales();
        },
        methods: {
            async fetchSales() {
                try {
                    const response = await axios.get('http://localhost:5062/pharmacy/sales');
                    this.sales = response.data.data;
                } catch (error) {
                    console.error('Ошибка при загрузке продаж:', error);
                }
            },
            openAddSalePage() {
                this.$router.push('/add-sale');
            },
            goBack() {
                this.$router.push('/home');
            },
            reviewSale(sale) {
                this.$router.push({ name: 'SaleInfo', params: { saleId: sale.saleId } });
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

    .sales-page {
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

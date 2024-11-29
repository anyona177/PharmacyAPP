import { createRouter, createWebHistory } from 'vue-router';
import AuthPage from '../views/AuthPage.vue';
import UserLogin from '../views/UserLogin.vue';
import UserRegister from '../views/UserRegister.vue';
import HomePage from '../views/HomePage.vue';
import ProductsPage from '../views/ProductsPage.vue';
import SalesPage from '../views/SalesPage.vue';
import EmployeesPage from '../views/EmployeesPage.vue';
import CategoriesPage from '../views/CategoriesPage.vue';
import SaleInfoPage from '../views/SaleInfoPage.vue';
import AddProduct from '../components/AddProduct.vue';
import AddSale from '../components/AddSale.vue';
import AddCategory from '../components/AddCategory.vue';
import AddEmployee from '../components/AddEmployee.vue';

const routes = [
    { path: '/', name: 'AuthPage', component: AuthPage },
    { path: '/login', name: 'UserLogin', component: UserLogin },
    { path: '/register', name: 'UserRegister', component: UserRegister },
    { path: '/home', name: 'SalesTrack Med', component: HomePage, meta: { requiresAuth: true } },
    { path: '/products', name: 'Products', component: ProductsPage, meta: { requiresAuth: true } },
    { path: '/sales', name: 'Sales', component: SalesPage, meta: { requiresAuth: true } },
    { path: '/employees', name: 'Employees', component: EmployeesPage, meta: { requiresAuth: true } },
    { path: '/categories', name: 'Categories', component: CategoriesPage, meta: { requiresAuth: true } },
    { path: '/sales/solditems/:saleId', name: 'SaleInfo', component: SaleInfoPage, props: true, meta: { requiresAuth: true } },
    { path: '/add-product', name: 'AddProduct', component: AddProduct, meta: { requiresAuth: true } },
    { path: '/add-sale', name: 'AddSale', component: AddSale, meta: { requiresAuth: true } },
    { path: '/add-category', name: 'AddCategory', component: AddCategory, meta: { requiresAuth: true } },
    { path: '/add-employee', name: 'AddEmployee', component: AddEmployee, meta: { requiresAuth: true } },
];

const router = createRouter({
    history: createWebHistory(),
    routes,
});

// Глобальный перехватчик навигации для авторизации
router.beforeEach((to, from, next) => {
    const token = localStorage.getItem('authToken');

    if (to.matched.some(record => record.meta.requiresAuth)) {
        if (!token) {
            next('/login');
        } else {
            next();
        }
    } else {
        next();
    }
});


export default router;

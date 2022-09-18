import { createRouter, createWebHistory } from 'vue-router'
import HomePage from './pages/Home'
import MyOrderPage from './pages/MyOrder'
import NotFoundPage from './pages/NotFound'
import OrderSummaryPage from './pages/OrderSummary'
import AddProductPage from './pages/Products/AddProductPage'
import EditProductPage from './pages/Products/EditProductPage'
import ProductsPage from './pages/Products/ProductsPage'
import AddAdditionPage from './pages/Additions/AddAdditionPage'
import EditAdditionPage from './pages/Additions/EditAdditionPage'
import AdditionsPage from './pages/Additions/AdditionsPage'

const routes = [
    {
        path: '/',
        name: 'home',
        component: HomePage
    },
    {
        path: '/my-order',
        name: 'my-order',
        component: MyOrderPage
    },
    {
        path: '/order-summary',
        name: 'order-summary',
        component: OrderSummaryPage
    },
    {
        path: '/products/add',
        name: 'add-product',
        component: AddProductPage
    },
    {
        path: '/products/edit/:productId',
        name: 'edit-product',
        component: EditProductPage
    },
    {
        path: '/products/',
        name: 'all-products',
        component: ProductsPage
    },
    {
        path: '/additions/add',
        name: 'add-addition',
        component: AddAdditionPage
    },
    {
        path: '/additions/edit/:additionId',
        name: 'edit-addition',
        component: EditAdditionPage
    },
    {
        path: '/additions/',
        name: 'all-additions',
        component: AdditionsPage
    },
    // and finally the default route, when none of the above matches:
    { 
        path: "/:catchAll(.*)",
        name: 'not-found',
        component: NotFoundPage
    }
];

const router = createRouter({
    history: createWebHistory(),
    routes
});

export default router;
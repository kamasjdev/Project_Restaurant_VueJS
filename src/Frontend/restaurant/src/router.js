import { createRouter, createWebHistory } from 'vue-router'
import HomePage from './pages/Home'
import MyOrderPage from './pages/MyOrder'
import NotFoundPage from './pages/NotFound'
import ForbiddenPage from './pages/Forbidden'
import OrderSummaryPage from './pages/OrderSummary'
import AddProductPage from './pages/Products/AddProductPage'
import EditProductPage from './pages/Products/EditProductPage'
import ProductsPage from './pages/Products/ProductsPage'
import AddAdditionPage from './pages/Additions/AddAdditionPage'
import EditAdditionPage from './pages/Additions/EditAdditionPage'
import AdditionsPage from './pages/Additions/AdditionsPage'
import LoginPage from './pages/Login'
import UsersPage from './pages/Users/UsersPage'
import AddUserPage from './pages/Users/AddUserPage'
import EditUserPage from './pages/Users/EditUserPage'
import ChangeUserRolePage from './pages/Users/ChangeUserRolePage'
import * as authService from '@/services/AuthService'

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
        path: '/order-summary/:orderId',
        name: 'order-summary',
        component: OrderSummaryPage
    },
    {
        path: '/products/add',
        name: 'add-product',
        component: AddProductPage,
        meta: {
            auth: true
        }
    },
    {
        path: '/products/edit/:productId',
        name: 'edit-product',
        component: EditProductPage,
        meta: {
            auth: true
        }
    },
    {
        path: '/products',
        name: 'all-products',
        component: ProductsPage,
        meta: {
            auth: true
        }
    },
    {
        path: '/additions/add',
        name: 'add-addition',
        component: AddAdditionPage,
        meta: {
            auth: true
        }
    },
    {
        path: '/additions/edit/:additionId',
        name: 'edit-addition',
        component: EditAdditionPage,
        meta: {
            auth: true
        }
    },
    {
        path: '/additions',
        name: 'all-additions',
        component: AdditionsPage,
        meta: {
            auth: true
        }
    },
    {
        path: '/login',
        name: 'login',
        component: LoginPage
    },
    {
        path: '/users',
        name: 'all-users',
        component: UsersPage,
        meta: {
            auth: true,
            role: 'admin'
        },
        children: [{
            path: ':userId',
            name: 'user-change-role',
            component: ChangeUserRolePage,
        }]
    },
    {
        path: '/users/add',
        name: 'add-user',
        component: AddUserPage,
        meta: {
            auth: true,
            role: 'admin'
        }
    },
    {
        path: '/users/edit/:userId',
        name: 'edit-user',
        component: EditUserPage,
        meta: {
            auth: true,
            role: 'admin'
        }
    },
    {
        path: '/forbidden',
        name: 'forbidden',
        component: ForbiddenPage,
        meta: {
            auth: true
        }
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
router.beforeEach((to, from, next) => {
    const userLoggedIn = authService.isLogged();
    if (to.meta.auth && !userLoggedIn) {
      next('/login');
    }

    const user = authService.getUser();
    if (to.meta.role && to.meta.role != user.role) {
        next('/forbidden');
    }

    next();
});

export default router;
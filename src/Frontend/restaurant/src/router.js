import { createRouter, createWebHistory } from 'vue-router'
import Home from './pages/Home'
import Settings from './pages/Settings'

const routes = [
    {
        path: '/',
        name: 'home',
        component: Home
    },
    {
        path: '/settings',
        name: 'settings',
        component: Settings
    }
];

const router = createRouter({
    history: createWebHistory(),
    routes
});

export default router;
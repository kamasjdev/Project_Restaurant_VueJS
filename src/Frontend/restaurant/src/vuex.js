import { createStore } from 'vuex'
import * as authService from '@/services/AuthService'

const store = createStore({
    state() {
        return {
            isAuthenticated: authService.isLogged(),
            user: authService.getUser(),
            users: [],
            userToChangeRole: null
        }
    },
    getters: {
        isAuthenticated: (state) => { return state.isAuthenticated; },
        user: (state) => { return state.user; },
        users: (state) => { return state.users },
        userToChangeRole: (state) => { return state.userToChangeRole }
    },
    actions: {
        isAuthenticated(context, isAuthenticated) {
            context.commit('isAuthenticated', isAuthenticated);
            context.dispatch('user', authService.getUser());
        },
        user(context, user) {
            context.commit('user', user);
        },
        users(context, users) {
            context.commit('users', users);
        },
        userToChangeRole(context, userToChangeRole) {
            context.commit('userToChangeRole', userToChangeRole);
        }
    },
    mutations: {
        isAuthenticated(state, isAuthenticated) {
            state.isAuthenticated = isAuthenticated;
        },
        user(state, user) {
            state.user = user;
        },
        users(state, users) {
            state.users = users;
        },
        userToChangeRole(state, userToChangeRole) {
            state.userToChangeRole = userToChangeRole;
        }
    }
});

export default store;
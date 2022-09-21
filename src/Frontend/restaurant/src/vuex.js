import { createStore } from 'vuex'
import * as authService from '@/services/AuthService'

const store = createStore({
    state() {
        return {
            isAuthenticated: authService.isLogged(),
            user: authService.getUser()
        }
    },
    getters: {
        isAuthenticated: (state) => { return state.isAuthenticated; },
        user: (state) => { return state.user; }
    },
    actions: {
        isAuthenticated(context, isAuthenticated) {
            context.commit('isAuthenticated', isAuthenticated);
            context.dispatch('user', authService.getUser());
        },
        user(context, user) {
            context.commit('user', user);
        }
    },
    mutations: {
        isAuthenticated(state, isAuthenticated) {
            state.isAuthenticated = isAuthenticated;
        },
        user(state, user) {
            state.user = user;
        }
    }
});

export default store;
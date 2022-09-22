<template>
    <div class="users-page">
        <div v-if="loading">
            <LoadingIconComponent />
        </div>
        <div v-if="loading === false">
            <div class="user-outlet mt-2 mb-2">                
                <router-view></router-view>
            </div>
            <h3 class="mt-2 mb-2">Lista użytkowników</h3>
            <div class="users-buttons mt-2 mb-2">
                <RouterButtonComponent :namedRoute="{ name: 'add-user' }" :buttonText="'Dodaj użytkownika'"/>
            </div>
            <table class="table table-bordered">
                <thead class="table-dark">
                    <tr>
                        <td>
                            id
                        </td>
                        <td>
                            Email
                        </td>
                        <td>
                            Rola
                        </td>
                        <td>
                            Utworzono
                        </td>
                        <td>
                            Akcja
                        </td>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="user in users" :key="user.id" class="text-start" :data-id="user.id">
                        <td>
                            {{ user.id }}
                        </td>
                        <td>
                            {{ user.email }}
                        </td>
                        <td>
                            {{ user.role }}
                        </td>
                        <td>
                            {{ user.createdAt }}
                        </td>
                        <td>
                            <button type="button" class="btn btn-primary me-2" @click="changeRole(user.id)">Zmień rolę</button>
                            <RouterButtonComponent :namedRoute="{ name: 'edit-user', params: { userId: user.id } }" 
                                    :buttonText="'Edytuj'" :buttonClass="'btn btn-warning me-2'"
                                    :buttonType="'button'" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</template>

<script>
    import LoadingIconComponent from '@/components/LoadingIcon/LoadingIcon';
    import RouterButtonComponent from '@/components/RouterButton/RouterButton';
    import axios from '../../axios-setup';
    import { mapGetters } from 'vuex';

    export default {
        name: 'UsersPage',
        components: {
            LoadingIconComponent,
            RouterButtonComponent,
        },
        data() {
            return {
                loading: true,
            }
        },
        methods: {
            async fetchUsers() {
                try {
                    const response = await axios.get('/api/users');
                    const users = response.data.map(u => ({
                        id: u.id,
                        email: u.email,
                        role: u.role,
                        createdAt: new Date(u.createdAt).toLocaleString()
                    }));
                    return users;
                } catch(exception) {
                    console.log(exception);
                }
            },
            async changeRole(userId) {
                const response = await axios.get(`/api/users/${userId}`);
                this.$store.dispatch('userToChangeRole', response.data);
                this.$router.push({ name: 'user-change-role', params: { userId } });
            },
        },
        computed: {
            ...mapGetters(['users'])
        },
        async created() {
            const users = await this.fetchUsers();
            this.$store.dispatch('users', users);
            this.loading = false;
        }
    }
</script>

<style>
    .users-page {
        padding-left: 2rem;
        padding-right: 2rem;
    }

    .users-buttons {
        float: left;
    }

    .user-outlet {

    }
</style>
<template>
    <div class="users-page">
        <div v-if="loading">
            <LoadingIconComponent />
        </div>
        <div v-if="loading === false">
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
                    <tr v-for="user in users" :key="user.id" class="text-start">
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
                            <button type="button" class="btn btn-primary me-2">Zmień rolę</button>
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

    export default {
        name: 'UsersPage',
        components: {
            LoadingIconComponent,
            RouterButtonComponent,
        },
        data() {
            return {
                loading: true,
                users: []
            }
        },
        methods: {
            async fetchUsers() {
                try {
                    const response = await axios.get('/api/users');
                    this.users = response.data.map(u => ({
                        id: u.id,
                        email: u.email,
                        role: u.role,
                        createdAt: new Date(u.createdAt).toLocaleString()
                    }));
                } catch(exception) {
                    console.log(exception);
                }
            }
        },
        async created() {
            this.fetchUsers();
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
</style>
<template>
    <div v-if="userToChangeRole === null && loading === true">
        <LoadingIconComponent />
    </div>
    <div v-else-if="userToChangeRole === null && loading === false">
        <h3 class="mb-2 mt-2">Edytuj rolę użytkownika</h3>
        <div class="change-user-form-position">
            <div v-if="error.length > 0" className="change-user-form alert alert-danger">{{error}}</div>
        </div>
        <RouterButton :namedRoute="{ name: 'all-users' }" :buttonText="'Powrót'" :buttonClass="'btn btn-secondary'" />
    </div>
    <div v-else>
        <h3>Edytuj rolę użytkownika {{ userToChangeRole.email }}</h3>
        <div v-if="error" className="alert alert-danger">{{error}}</div>
        <div class="change-user-form-position">
            <div class="change-user-form alert">
                <Input :label="'Rola'" :type="'select'" :value="userToChangeRole.role" 
                        v-model="userToChangeRole.role" :showError="userToChangeRole.role === null" 
                        :error="'Pole jest wymagane'" 
                        :options="roles" 
                        @valueChanged="($event) => userToChangeRole.role = $event"/>
            </div>
        </div>
        <div class="mt-2">
            <RouterButton :namedRoute="{ name: 'all-users' }" :buttonText="'Powrót'" :buttonClass="'btn btn-secondary me-2'" @click="backToUsers" />
            <button class="btn btn-success" @click="submit">
                Zatwierdź
            </button>
        </div>
    </div>
</template>

<script>
    import Input from '@/components/Input/Input';
    import LoadingIconComponent from '@/components/LoadingIcon/LoadingIcon';
    import axios from '@/axios-setup';
    import exceptionMapper from '@/helpers/exceptionToMessageMapper';
    import RouterButton from '@/components/RouterButton/RouterButton';
    import { mapGetters } from 'vuex';

    export default {
        name: 'ChangeUserRolePage',
        components: {
        Input,
            LoadingIconComponent,
            RouterButton
        },
        data() {
            return {
                roles: [{ label: 'admin', value: 'admin'}, { label: 'user', value: 'user' }],
                loading: true,
                error: ''
            }
        },
        methods: {
            async submit() {
                try {
                    const role =  this.userToChangeRole.role;
                    await axios.patch(`/api/users/change-role/${this.$route.params.userId}`, {
                        role
                    });                
                    this.loading = true;
                    this.$store.dispatch('userToChangeRole', null);
                    await this.updateUsers();
                    this.$router.push({ name: 'all-users' });
                } catch(exception) {
                    const message = exceptionMapper(exception);
                    this.error = message;
                    console.log(exception);
                }
            },
            async updateUsers() {
                try {
                    const response = await axios.get('/api/users');
                    const users = response.data.map(u => ({
                        id: u.id,
                        email: u.email,
                        role: u.role,
                        createdAt: new Date(u.createdAt).toLocaleString()
                    }));
                    this.$store.dispatch('users', users);
                } catch(exception) {
                    console.log(exception);
                }
            },
            backToUsers() {
                this.$store.dispatch('userToChangeRole', null);
            }
        },
        computed: {
            ...mapGetters(['userToChangeRole'])
        },
        mounted() {
            this.loading = false;
        }
    }
</script>

<style>
    .change-user-form-position {
        display: flex;
        justify-content: center;
        text-align: center;
        align-items: center;
    }

    .change-user-form {
        display: flex;
        flex-direction: column;
        width: 50%;
        padding-left: 5px;
        padding-right: 5px;
    }
</style>
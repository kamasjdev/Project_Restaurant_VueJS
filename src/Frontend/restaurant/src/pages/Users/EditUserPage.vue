<template>
    <div v-if="user === null && loading === true">
        <LoadingIconComponent />
    </div>
    <div v-else-if="user === null && loading === false">
        <h3 class="mb-2 mt-2">Edytuj użytkownika</h3>
        <div v-if="error.length > 0" className="alert alert-danger">{{error}}</div>
    </div>
    <div v-else>
        <h3>Edytuj użytkownika</h3>
        <div v-if="error" className="alert alert-danger">{{error}}</div>
        <UserFormComponent :user="user" :roles="roles" @submitForm="onSubmitForm" />
    </div>
</template>

<script>
    import UserFormComponent from '@/components/User/UserForm';
    import LoadingIconComponent from '@/components/LoadingIcon/LoadingIcon';
    import axios from '@/axios-setup';
    import exceptionMapper from '@/helpers/exceptionToMessageMapper';

    export default {
        name: 'EditUserPage',
        components: {
            UserFormComponent,
            LoadingIconComponent
        },
        data() {
            return {
                user: null,
                roles: [{ label: 'admin', value: 'admin'}, { label: 'user', value: 'user' }],
                loading: true,
                error: ''
            }
        },
        methods: {
            async onSubmitForm(form) {
                this.error = '';
                try {
                    await axios.post('/api/users/sign-up', form);
                    this.$router.push({ name: 'all-users' });
                } catch(exception) {
                    const message = exceptionMapper(exception);
                    this.error = message;
                    console.log(exception);
                }
            }
        },
        async created() {
            try {
                const response = await axios.get(`/api/users/${this.$route.params.userId}`);
                this.user = response.data;
            } catch(exception) {
                const message = exceptionMapper(exception);
                this.error = message;
                console.log(exception);
            }

            this.loading = false;
        }
    }
</script>

<style>
</style>
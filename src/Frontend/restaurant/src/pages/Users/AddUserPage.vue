<template>
    <h3>Dodaj użytkownika</h3>
    <div v-if="error" className="alert alert-danger">{{error}}</div>
    <UserFormComponent :roles="roles" @submitForm="onSubmitForm" />
</template>

<script>
    import UserFormComponent from '@/components/User/UserForm';
    import axios from '@/axios-setup';
    import exceptionMapper from '@/helpers/exceptionToMessageMapper';

    export default {
        name: 'AddUserPage',
        components: {
            UserFormComponent
        },
        data() {
            return {
                roles: [{ label: 'admin', value: 'admin'}, { label: 'user', value: 'user' }],
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
        }
    }
</script>

<style>
</style>
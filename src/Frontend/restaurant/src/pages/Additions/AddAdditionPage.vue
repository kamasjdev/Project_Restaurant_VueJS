<template>
    <h3 class="mb-2 mt-2">Dodaj dodatek</h3>
    <div v-if="error" className="alert alert-danger">{{error}}</div>
    <AdditionFormComponent :additionKinds="additionKinds" @submitForm="onSubmitForm" />
</template>

<script>
    import AdditionFormComponent from '@/components/Addition/AdditionForm';
    import axios from '@/axios-setup';
    import exceptionMapper from '@/helpers/exceptionToMessageMapper';

    export default {
        name: 'AddAdditionPage',
        components: {
            AdditionFormComponent
        },
        data() {
            return {
                additionKinds: [{label: 'Napój', value: 'Drink'}, {label: 'Sałatka', value: 'Salad'}],
                error: ''
            }
        },
        methods: {
            async onSubmitForm(form) {
                this.error = '';
                try {
                    await axios.post('/api/additions', form);
                    this.$router.push({ name: 'all-additions' });
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
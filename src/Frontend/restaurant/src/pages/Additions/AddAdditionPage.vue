<template>
    <h3 class="mb-2 mt-2">Dodaj dodatek</h3>
    <AdditionFormComponent :additionKinds="additionKinds" @submitForm="onSubmitForm" />
</template>

<script>
    import AdditionFormComponent from '@/components/Addition/AdditionForm';

    export default {
        name: 'AddAdditionPage',
        components: {
            AdditionFormComponent
        },
        data() {
            return {
                additionKinds: [{label: 'Napój', value: 'Drink'}, {label: 'Sałatka', value: 'Salad'}]
            }
        },
        methods: {
            onSubmitForm(form) {
                console.log("From AddAdditionPage, ", form);
                // send to API
                this.$router.push({ name: 'all-additions' });
            }
        }
    }
</script>

<style>
</style>
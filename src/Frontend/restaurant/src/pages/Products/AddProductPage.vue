<template>
    <h3 class="mb-2 mt-2">Dodaj produkt</h3>
    <div v-if="error" className="alert alert-danger">{{error}}</div>
    <ProductFormComponent :productKinds="productKinds" @submitForm="onSubmitForm" />
</template>

<script>
    import ProductFormComponent from '@/components/Product/ProductForm';
    import axios from '@/axios-setup';
    import exceptionMapper from '@/helpers/exceptionToMessageMapper';

    export default {
        name: 'AddProductPage',
        components: {
            ProductFormComponent
        },
        data() {
            return {
                productKinds: [{label: 'Pizza', value: 'Pizza'}, {label: 'Danie główne', value: 'MainDish'}, {label: 'Zupa', value: 'Soup'}],
                error: ''
            }
        },
        methods: {
            async onSubmitForm(form) {
                this.error = '';
                try {
                    await axios.post('/api/products', form);
                    this.$router.push({ name: 'all-products' });
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
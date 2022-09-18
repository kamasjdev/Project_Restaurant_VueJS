<template>
    <h3 class="mb-2 mt-2">Dodaj produkt</h3>
    <ProductFormComponent :productKinds="productKinds" @submitForm="onSubmitForm" />
</template>

<script>
    import ProductFormComponent from '@/components/Product/ProductForm';

    export default {
        name: 'AddProductPage',
        components: {
            ProductFormComponent
        },
        data() {
            return {
                productKinds: [{label: 'Pizza', value: 'Pizza'}, {label: 'Danie główne', value: 'MainDish'}, {label: 'Zupa', value: 'Soup'}]
            }
        },
        methods: {
            onSubmitForm(form) {
                console.log("From AddProductPage, ", form);
                // send to API
                this.$router.push({ name: 'all-products' });
            }
        }
    }
</script>

<style>
</style>
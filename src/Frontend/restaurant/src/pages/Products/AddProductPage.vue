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
                productKinds: [{label: 1, value: 1}, {label: 2, value: 2}]
            }
        },
        methods: {
            onSubmitForm(form) {
                console.log("From AddProductPage, ", form);
                // send to API
            }
        }
    }
</script>

<style>
</style>
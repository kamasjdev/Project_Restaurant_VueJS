<template>
    <h3 class="mb-2 mt-2">Dodaj produkt</h3>
    <ProductFormComponent />
</template>

<script>
    import ProductFormComponent from '@/components/Product/ProductForm';

    export default {
        name: 'AddProductPage',
        components: {
            ProductFormComponent
        },
        methods: {
        }
    }
</script>

<style>
</style>
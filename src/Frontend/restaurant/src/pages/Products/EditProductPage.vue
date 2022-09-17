<template>
    <h3 class="mb-2 mt-2">Edytuj produkt {{$route.params.productId}}</h3>
    <ProductFormComponent :product="product" :productKinds="productKinds" @submitForm="onSubmitForm" />
</template>

<script>
    import ProductFormComponent from '@/components/Product/ProductForm';

    export default {
        name: 'EditProductPage',
        components: {
            ProductFormComponent
        },
        data() {
            return {
                product: {
                    id: 'e8caf943-608b-4f39-84f0-9fbcd038874b',
                    productName: 'Pizza Capriciosa 60cm',
                    price: 50.85,
                    productKind: 'Pizza'
                },
                productKinds: [{label: 'Pizza', value: 'Pizza'}, {label: 'Zupa', value: 'Zupa'}]
            }
        },
        methods: {
            onSubmitForm(form) {
                console.log("From EditProductPage, ", form);
                // send to API
            }
        }
    }
</script>

<style>
</style>
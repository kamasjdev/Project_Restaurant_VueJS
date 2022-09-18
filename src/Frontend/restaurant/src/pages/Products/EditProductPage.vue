<template>
    <div v-if="product === null && loading === true">
        <LoadingIconComponent />
    </div>
    <div v-else-if="product === null && loading === false">
        <h3 class="mb-2 mt-2">Edytuj produkt</h3>
        <div className="alert alert-danger">{{error}}</div>
    </div>
    <div v-else>
        <h3 class="mb-2 mt-2">Edytuj produkt {{product.productName}}</h3>
        <div v-if="error" className="alert alert-danger">{{error}}</div>
        <ProductFormComponent :product="product" :productKinds="productKinds" @submitForm="onSubmitForm" />
    </div>
</template>

<script>
    import ProductFormComponent from '@/components/Product/ProductForm';
    import LoadingIconComponent from '@/components/LoadingIcon/LoadingIcon';
    import axios from '@/axios-setup';
    import exceptionMapper from '@/helpers/exceptionToMessageMapper';

    export default {
        name: 'EditProductPage',
        components: {
            ProductFormComponent,
            LoadingIconComponent
        },
        data() {
            return {
                product: null,
                productKinds: [{label: 'Pizza', value: 'Pizza'}, {label: 'Danie główne', value: 'MainDish'}, {label: 'Zupa', value: 'Soup'}],
                loading: true,
                error: ''
            }
        },
        methods: {
            async onSubmitForm(form) {
                this.error = '';
                try {
                    await axios.put(`/api/products/${this.$route.params.productId}`, form);
                    this.$router.push({ name: 'all-products' });
                } catch(exception) {
                    const message = exceptionMapper(exception);
                    this.error = message;
                    console.log(exception);
                }
            }
        },
        async created() {
            try {
                const response = await axios.get(`/api/products/${this.$route.params.productId}`);
                this.product = {
                    id: response.data.id,
                    productName: response.data.productName,
                    price: new Number(response.data.price).toFixed(2),
                    productKind: response.data.productKind
                }
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
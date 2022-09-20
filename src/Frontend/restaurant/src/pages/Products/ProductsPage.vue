<template>
    <div class="products-page">
        <div v-if="loading">
            <LoadingIconComponent />
        </div>
        <div v-if="loading === false">
            <h3 class="mt-2 mb-2">Lista produktów</h3>
            <div class="products-buttons mt-2 mb-2">
                <RouterButtonComponent :namedRoute="{ name: 'add-product' }" :buttonText="'Dodaj produkt'"/>
            </div>
            <table class="table table-bordered">
                <thead class="table-dark">
                    <tr>
                        <td>
                            id
                        </td>
                        <td>
                            Nazwa produktu
                        </td>
                        <td>
                            Cena [PLN]
                        </td>
                        <td>
                            Typ produktu
                        </td>
                        <td>
                            Akcja
                        </td>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="product in products" :key="product.id" class="text-start">
                        <td>
                            {{ product.id }}
                        </td>
                        <td>
                            {{ product.productName }}
                        </td>
                        <td>
                            {{ product.price }}
                        </td>
                        <td>
                            {{ product.productKind }}
                        </td>
                        <td>
                            <RouterButtonComponent :namedRoute="{ name: 'edit-product', params: { productId: product.id } }" 
                                    :buttonText="'Edytuj'" :buttonClass="'btn btn-warning me-2'"
                                    :buttonType="'button'" />
                            <button class="btn btn-danger" @click="onDelete($event, product)">Usuń</button>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <PopupComponent :open="openModal" @popupClosed="popupClosed">
            <div>Czy chcesz usunąć produkt {{productToDelete.productName}}?</div>
            <div v-if="error" className="alert alert-danger mt-2 mb-2">{{error}}</div>
            <div class="mt-2">
                <button class="btn btn-danger me-2" @click="confirmDelete">Tak</button>
                <button class="btn btn-secondary" @click="popupClosed">Nie</button>
            </div>
        </PopupComponent>
    </div>
</template>

<script>
    import LoadingIconComponent from '@/components/LoadingIcon/LoadingIcon';
    import RouterButtonComponent from '@/components/RouterButton/RouterButton';
    import PopupComponent from '@/components/Poupup/Popup';
    import axios from '../../axios-setup';
    import exceptionMapper from '@/helpers/exceptionToMessageMapper';

    export default {
        name: 'ProductsPage',
        components: {
            LoadingIconComponent,
            RouterButtonComponent,
            PopupComponent
        },
        data() {
            return {
                loading: true,
                products: [],
                openModal: false,
                productToDelete: null,
                error: ''
            }
        },
        methods: {
            onDelete(event, product) {
                this.productToDelete = product;
                this.openModal = true;
            },
            popupClosed() {
                this.productToDelete = null;
                this.openModal = false;
            },
            async confirmDelete() {
                try {
                    this.error = '';
                    await axios.delete(`/api/products/${this.productToDelete.id}`);
                    this.fetchProducts();
                    this.additionToDelete = null;
                    this.openModal = false;
                } catch(exception) {
                    const message = exceptionMapper(exception);
                    this.error = message;
                    console.log(exception);
                }
            },
            async fetchProducts() {
                try {
                    const response = await axios.get('/api/products');
                    this.products = response.data.map(p => ({
                        id: p.id,
                        productName: p.productName,
                        price: new Number(p.price).toFixed(2),
                        productKind: p.productKind
                    }));
                } catch(exception) {
                    console.log(exception);
                }
            }
        },
        async created() {
            this.fetchProducts();
            this.loading = false;
        }
    }
</script>

<style>
    .products-page {
        padding-left: 2rem;
        padding-right: 2rem;
    }

    .products-buttons {
        float: left;
    }
</style>
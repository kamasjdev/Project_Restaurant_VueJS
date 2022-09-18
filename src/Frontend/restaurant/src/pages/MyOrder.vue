<template>
    <div id="my-order-layout">    
        <div v-if="loading">
            <LoadingIconComponent />
        </div>
        <div v-else>
            <div id="main-dish">
                Dania główne
            </div>
            <div class="d-flex row mt-2 me-2">
                <ProductComponent v-for="product in products" :key="product.id" :product="product" :markedId="productToAdd" @click="markProduct(product)" />
            </div>
            <div v-if="productToAdd">
                <div id="addition">
                    Dodatki
                </div>
                <div class="d-flex row mt-2 me-2 mb-2">
                    <AdditionComponent v-for="addition in additions" :key="addition.id" :addition="addition" :markedId="additionToAdd" @click="markAddition(addition)"/>
                </div>
            </div>
            <div class="text-start mt-2 mb-2">
                <button :class="productToAdd ? 'btn btn-success' : 'btn btn-success disabled'" @click="addToOrder()">Dodaj do zamówienia</button>
                <RouterButtonComponent :url="'/order-summary'" :buttonText="'Zatwierdź zamówienie'" :buttonClass="'ms-2 btn btn-warning'" v-if="productsOrdered.length > 0"/>
                <button class="ms-2 btn btn-danger" v-if="productToDelete" @click="removeFromOrder()">Usuń z zamówienia</button>
            </div>
            <div clas="mt-2">
                <ProductSalesComponent :productSales="productsOrdered" @markedRow="productSaleMarked"/>
            </div>
        </div>
    </div>
</template>

<script>
import ProductComponent from '@/components/Product/Product';
import AdditionComponent from '@/components/Addition/Addition';
import ProductSalesComponent from '@/components/ProductSales/ProductSales';
import RouterButtonComponent from '@/components/RouterButton/RouterButton';
import LoadingIconComponent from '@/components/LoadingIcon/LoadingIcon'
import axios from '@/axios-setup';

  export default {
    name: 'MyOrderPage',
    components: {
        ProductComponent,
        AdditionComponent,
        ProductSalesComponent,
        RouterButtonComponent,
        LoadingIconComponent
    },
    data() {
        return {
            products: [],
            additions: [],
            productToAdd: null,
            additionToAdd: null,
            productsOrdered: [],
            productToDelete: null,
            loading: true
        };
    },
    methods: {
        async fetchAdditions() {
            try {
                const response = await axios.get('/api/additions');
                console.log('fetching Additions')
                this.additions = response.data.map(a => ({
                    id: a.id,
                    additionName: a.additionName,
                    price: new Number(a.price).toFixed(2),
                    additionKind: a.additionKind
                }));
            } catch(exception) {
                console.log(exception);
            }
        },
        async fetchProducts() {
            try {
                const response = await axios.get('/api/products');
                console.log('fetching Products')
                console.log(response.data)
                this.products = response.data.map(p => ({
                    id: p.id,
                    productName: p.productName,
                    price: new Number(p.price).toFixed(2),
                    productKind: p.productKind
                }));
                console.log(this.products)
            } catch(exception) {
                console.log(exception);
            }
        },
        markProduct(product) {
            if (this.productToAdd === product.id) {
                this.productToAdd = null;
                this.additionToAdd = null;
                return;
            }

            this.productToAdd = product.id;
            this.additionToAdd = null;
        },
        markAddition(addition) {
            if (this.additionToAdd === addition.id) {
                this.additionToAdd = null;
                return;
            }

            this.additionToAdd = addition.id;
        },
        addToOrder() {
            const product = this.products.find(p => p.id === this.productToAdd);
            this.productsOrdered.push({id: Math.random(), itemId: product.id, name: product.productName, price: product.price, type: 'products'});
            const addition = this.additions.find(a => a.id === this.additionToAdd);

            if (addition) {
                this.productsOrdered.push({id: Math.random(), itemId: addition.id, name: addition.additionName, price: addition.price, type: 'additions'});
            }
        },
        productSaleMarked(value) {
            this.productToDelete = value;
        },
        removeFromOrder() {
           this.productsOrdered = this.productsOrdered.filter(p => p.id !== this.productToDelete);
           this.productToDelete = null;
        }
    },
    async created() {
        await this.fetchProducts();
        await this.fetchAdditions();
        this.loading = false;
    }
  }
</script>

<style>
    #my-order-layout {
        margin-left: 1rem;
        margin-right: 1rem;
    }

    #main-dish {
        font-size: 2rem;
    }

    #addition {
        font-size: 2rem;
    }
</style>
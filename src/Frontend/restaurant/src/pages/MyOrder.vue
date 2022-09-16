<template>
    <div id="my-order-layout">
        <div id="main-dish">
            Dania główne
        </div>
        <div class="d-flex row mt-2 me-2">
            <ProductComponent v-for="product in products" :key="product.id" :product="product" />
        </div>
        <div id="addition">
            Dodatki
        </div>
        <div class="d-flex row mt-2 me-2">
            <AdditionComponent v-for="addition in additions" :key="addition.id" :addition="addition" />
        </div>
    </div>
</template>

<script>
import ProductComponent from '@/components/Product/Product';
import AdditionComponent from '@/components/Addition/Addition';
import * as response from '../stub/response.json';

  export default {
    name: 'MyOrderPage',
    components: {
        ProductComponent,
        AdditionComponent
    },
    data() {
        return {
            products: [],
            additions: []
        };
    },
    methods: {
        getProducts() {
            return response.products;
        },
        getAdditions() {
            return response.additions;
        }
    },
    mounted() {
        this.products = this.getProducts().map(p => ({
            id: p.id,
            productName: p.productName,
            price: new Number(p.price).toFixed(2),
            productKind: p.productKind
        }));
        this.additions = this.getAdditions(a => ({
            id: a.id,
            productName: a.additionName,
            price: new Number(a.price).toFixed(2),
            productKind: a.additionKind
        }));
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
<template>
    <div id="my-order-layout">
        <div v-if="state===1">
            <h4>Proszę podaj swój email aby zamówić produkty:</h4>
            <div class="my-order-email-poisition mt-3 mb-3">
                <div class="my-order-email-input">
                    <InputComponent :label="'Email'" :type="'text'" :value="email" 
                            v-model="email" :showError="emailError.length > 0" 
                            :error="emailError"
                            @valueChanged="onChangeEmailInput($event)"/>
                </div>
            </div>
            <div v-if="error" className="alert alert-danger mt-2 mb-2">{{error}}</div>
            <div>
                <button class="btn btn-secondary me-2" @click="() => email = ''">
                    Reset
                </button>
                <button class="btn btn-success" @click="confirmEmail">
                    Ok
                </button>
            </div>
        </div>
        <div v-else>
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
                    <button class="ms-2 btn btn-warning" v-if="productsOrdered.length > 0" @click="addOrder">Zatwierdź zamówienie</button>
                    <button class="ms-2 btn btn-danger" v-if="productToDelete" @click="removeFromOrder()">Usuń z zamówienia</button>
                </div>
                <div clas="mt-2">
                    <ProductSalesComponent :productSales="productsOrdered" @markedRow="productSaleMarked"/>
                </div>
                <div v-if="productsOrdered.length > 0">
                    <InputComponent :label="'Uwagi'" :type="'textarea'" :value="note" 
                            v-model="note" :showError="note.length > 500" 
                            :error="'Uwagi powinny być krótsze niż 500 znaków'"
                            @valueChanged="($event) => note = $event"/>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import ProductComponent from '@/components/Product/Product';
import AdditionComponent from '@/components/Addition/Addition';
import ProductSalesComponent from '@/components/ProductSales/ProductSales';
import LoadingIconComponent from '@/components/LoadingIcon/LoadingIcon'
import InputComponent from '@/components/Input/Input';
import axios from '@/axios-setup';
import exceptionMapper from '@/helpers/exceptionToMessageMapper';

  export default {
    name: 'MyOrderPage',
    components: {
        ProductComponent,
        AdditionComponent,
        ProductSalesComponent,
        LoadingIconComponent,
        InputComponent
    },
    data() {
        return {
            products: [],
            additions: [],
            productToAdd: null,
            additionToAdd: null,
            productsOrdered: [],
            productToDelete: null,
            loading: true,
            email: '',
            emailError: '',
            state: 1,
            error: '',
            note: ''
        };
    },
    methods: {
        async fetchAdditions() {
            try {
                const response = await axios.get('/api/additions');
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
                this.products = response.data.map(p => ({
                    id: p.id,
                    productName: p.productName,
                    price: new Number(p.price).toFixed(2),
                    productKind: p.productKind
                }));
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
        async addToOrder() {
            try {
                this.error = '';
                const product = this.products.find(p => p.id === this.productToAdd);
                const addition = this.additions.find(a => a.id === this.additionToAdd);

                const response = await axios.post('/api/product-sales', {
                    productId: product.id,
                    additionId: addition?.id,
                    email: this.email
                });
                const productSaleId = response.headers.location.split('/api/product-sales/')[1];
                
                this.productsOrdered.push({id: productSaleId, itemId: product.id, name: product.productName, price: product.price, type: 'products'});
                if (addition) {
                    this.productsOrdered.push({id: productSaleId, itemId: addition.id, name: addition.additionName, price: addition.price, type: 'additions'});
                }
            } catch(exception) {
                const message = exceptionMapper(exception);
                this.error = message;
                console.log(exception);
            }
        },
        productSaleMarked(value) {
            this.productToDelete = value;
        },
        async removeFromOrder() {
            try {
                this.error = '';
                await axios.delete(`/api/product-sales/${this.productToDelete}`);
                this.productsOrdered = this.productsOrdered.filter(p => p.id !== this.productToDelete);
                this.productToDelete = null;
            } catch(exception) {
                const message = exceptionMapper(exception);
                this.error = message;
                console.log(exception);
            }
        },
        onChangeEmailInput(value) {
            this.email = value;
        },
        confirmEmail() {
            this.emailError = '';
            const pattern = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/; //eslint-disable-line
            if (!pattern.test(this.email)) {
                this.emailError = 'Niepoprawny email';
                return;
            }

           this.state = 2;
        },
        async addOrder() {
            if (this.note.length > 500) {
                return;
            }

            const productsOrderedDistincted = this.productsOrdered.map(p => p.id).filter((v, i, a) => a.indexOf(v) === i);
            const response = await axios.post('/api/orders', {
                email: this.email,
                note: this.note,
                productSaleIds: productsOrderedDistincted
            });
            const orderId = response.headers.location.split('/api/orders/')[1];
            this.$router.push({ name: 'order-summary', params: { orderId } });
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

    .my-order-email-poisition {
        display: flex;
        justify-content: center;
        text-align: center;
        align-items: center;
    }

    .my-order-email-input {
        display: flex;
        flex-direction: column;
        width: 50%;
        padding-left: 5px;
        padding-right: 5px;
    }

    textarea {
        height: 10rem;
        resize: none;
    }
</style>
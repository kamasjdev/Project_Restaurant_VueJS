<template>
    <div id="order-summary-layout">
        <div v-if="loading">
            <LoadingIconComponent />
        </div>
        <div v-else-if="order === null && loading === false">
            <div className="alert alert-danger">{{error}}</div>
        </div>
        <div v-else>
            <div id="order-info" class="mt-2 mb-2">
                Szczegóły zamówienia nr  <span class="fw-bold">{{ order.orderNumber }}</span>
            </div>
            <div>
                <button class="mt-2 mb-2 btn btn-primary" @click="() => openModal = true">Wyślij potwierdzenie na maila</button>
            </div>
            <div>
                <table class="table table-striped table-hover table-bordered">
                    <thead class="table-dark">
                        <tr>
                            <td>Id</td>
                            <td>Numer zamówienia</td>
                            <td>Utworzono</td>
                            <td>Koszt [PLN]</td>
                            <td>Email</td>
                            <td>Uwagi</td>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td>
                                {{ order.id }}
                            </td>
                            <td>
                                {{ order.orderNumber }}
                            </td>
                            <td>
                                {{ order.created }}
                            </td>
                            <td>
                                {{ order.price }}
                            </td>
                            <td>
                                {{ order.email }}
                            </td>
                            <td>
                                {{ order.note }}
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div id="order-info" class="mt-2 mb-2">
                Pozycje
            </div>
            <div>
                <table class="table table-striped table-hover table-bordered">
                    <thead class="table-dark">
                        <tr>
                            <td>Id pozycji</td>
                            <td>Produkt</td>
                            <td>Dodatek</td>
                            <td>Cena [PLN]</td>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="productSale in order.products" :key="productSale.id">
                            <td>
                                {{ productSale.id }}
                            </td>
                            <td>
                                {{ productSale.product.productName }}
                            </td>
                            <td>
                                {{ productSale.addition?.additionName }}
                            </td>
                            <td>
                                {{ productSale.endPrice }}
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
    <PopupComponent :open="openModal" @popupClosed="popupClosed">
        <h4>Podaj email w celu wysłania potwierdzenia</h4>
        <div class="mt-2 mb-2">
            <InputComponent :label="'Email'" :type="'text'" :value="email" 
                            v-model="email" :showError="emailError.length > 0" 
                            :error="emailError"
                            @valueChanged="($event) => email = $event"/>
        </div>
        <div v-if="errorSend.length > 0" className="alert alert-danger mt-2">{{errorSend}}</div>
        <div class="mt-2">
            <button class="btn btn-danger me-2" @click="confirmSend">Tak</button>
            <button class="btn btn-secondary" @click="popupClosed">Nie</button>
        </div>
    </PopupComponent>
    <notifications
      group="custom-style"
      position="top right"
      :width="400"
    />
</template>

<script>
  import LoadingIconComponent from '../components/LoadingIcon/LoadingIcon';
  import PopupComponent from '@/components/Poupup/Popup';
  import InputComponent from '@/components/Input/Input';
  import axios from '@/axios-setup';
  import exceptionMapper from '@/helpers/exceptionToMessageMapper';
  import { notify } from "@kyvg/vue3-notification";

  export default {
    name: 'OrderSummaryPage',
    components: {
        LoadingIconComponent,
        PopupComponent,
        InputComponent
    },
    data() {
        return {
            loading: true,
            order: null,
            openModal: false,
            email: '',
            emailError: '',
            errorSend: ''
        }
    },
    methods: {
        popupClosed() {
            this.openModal = false;
            this.email = '';
            this.emailError = '';
            this.errorSend = '';
        },
        async confirmSend() {
            this.emailError = '';
            this.errorSend = '';
            const pattern = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/; //eslint-disable-line
            if (!pattern.test(this.email)) {
                this.emailError = 'Niepoprawny email';
                return;
            }

            try {
                await axios.post('/api/mails/send', {
                    orderId: this.order.id,
                    email: this.email
                });
                this.openModal = false;
                notify({
                    type: "success",
                    title: 'Send email',
                    text: `Email was sent to ${this.email}!`
                });
                this.email = '';
                this.emailError = '';
                this.errorSend = '';
            } catch(exception) {
                const message = exceptionMapper(exception);
                this.errorSend = message;
                console.log(exception);
            }
        },
    },
    async mounted() {
        try {
            const response = await axios.get(`/api/orders/${this.$route.params.orderId}`);
            this.order = {
                id: response.data.id,
                orderNumber: response.data.orderNumber,
                created: new Date(response.data.created).toLocaleString(),
                price: new Number(response.data.price).toFixed(2),
                email: response.data.email,
                note: response.data.note,
                products: response.data.products.map(p => ({
                    id: p.id,
                    product: {
                        id: p.product.id,
                        productName: p.product.productName,
                        price: p.product.price,
                        productKind: p.product.productKind
                    },
                    addition: p.addition !== null ? {
                        id: p.addition.id,
                        additionName: p.addition.additionName,
                        price: p.addition.price,
                        additionKind: p.addition.additionKind
                    } : null,
                    endPrice: p.endPrice,
                    productSaleState: p.productSaleState,
                    email: p.email
                }))
            };
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
    #order-summary-layout {
        margin-left: 1rem;
        margin-right: 1rem;
    }

    #order-info {
        font-size: 1.8rem;
    }
</style>
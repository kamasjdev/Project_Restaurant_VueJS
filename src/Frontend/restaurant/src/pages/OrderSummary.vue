<template>
    <div id="order-summary-layout">
        <div v-if="order">
            <div id="order-info" class="mt-2 mb-2">
                Szczegóły zamówienia nr  <span class="fw-bold">{{ order.orderNumber }}</span>
            </div>
            <div>
                <table class="table table-striped table-hover table-bordered">
                    <thead class="table-dark">
                        <tr>
                            <td>id</td>
                            <td>id potrawy</td>
                            <td>nazwa produktu</td>
                            <td>koszt [PLN]</td>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="productSale in order.productSales" :key="productSale.id">
                            <td>
                                {{ productSale.id }}
                            </td>
                            <td>
                                {{ productSale.itemId }}
                            </td>
                            <td>
                                {{ productSale.name }}
                            </td>
                            <td>
                                {{ productSale.price }}
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div id="total-price" class="text-start">
                <table class="table mt-2">
                    <thead>
                        <tr>
                            <th scope="col">Suma [PLN]</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td>{{ getTotalPrice() }}</td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
        <div v-else>
            <LoadingIconComponent />
        </div>
    </div>
</template>

<script>
  import LoadingIconComponent from '../components/LoadingIcon/LoadingIcon';

  export default {
    name: 'OrderSummaryPage',
    components: {
        LoadingIconComponent
    },
    data() {
        return {
            order: null
        }
    },
    methods: {
        async getOrder() {
            return Promise.resolve({
                orderNumber: "ac2af143-615b-4f21-83f1-afbcd038072a",
                productSales: [
                    {
                        id: "0.6008623874893207",
                        itemId: "e8caf943-608b-4f39-84f0-9fbcd038874b",
                        name: "Pizza Capriciosa 60cm",
                        price: "50.85"
                    },
                    {
                        id: "0.27508761669555026",
                        itemId: "8d961716-d7f2-4d12-8882-99a2eaf323d8",
                        name: "Sałatka",
                        price: "5.00"
                    }
                ]
            })
        },
        getTotalPrice() {
            let price = this.order.productSales.reduce((acc, current) => acc + new Number(current.price), 0);
            price = new Number(price).toFixed(2);
            return price;
        }
    },
    async mounted() {
        function timeout(ms) {
            return new Promise(resolve => setTimeout(resolve, ms));
        }
        
        await timeout(1000);
        this.order = await this.getOrder();
    }
  }
</script>

<style>
    #order-summary-layout {
        margin-left: 1rem;
        margin-right: 1rem;
    }

    #order-info {
        font-size: 2rem;
    }
</style>
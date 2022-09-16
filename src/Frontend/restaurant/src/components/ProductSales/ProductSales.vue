<template>
    <div v-if="productSales.length > 0">
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
                    <tr v-for="productSale in productSales" :key="productSale.id" @click="markedRow(productSale)" :class="isMarked(productSale.id) ? 'bg-success' : ''">
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
</template>

<script>

    export default {
        name: 'ProductSalesComponent',
        props: [ 'productSales' ],
        components: {
        },
        data() {
            return {
                markedRowId: null
            }
        },
        methods: {
            isMarked(id) {
                let marked = false;

                if (this.markedRowId === id) {
                    marked = true;
                }

                return marked;
            },
            markedRow(productSale) {
                if (this.markedRowId == productSale.id) {
                    this.markedRowId = null;
                    this.$emit('markedRow', null);
                    return;
                }

                this.markedRowId = productSale.id;
                this.$emit('markedRow', productSale.id);
            },
            getTotalPrice() {
                let price = this.productSales.reduce((acc, current) => acc + new Number(current.price), 0);
                price = new Number(price).toFixed(2);
                return price;
            }
        }
    }
</script>

<style>
    #total-price {
        width: 150px;
        margin-left: auto;
        margin-right: 10px;
    }
</style>
<template>
    <div id="itemBorder" :class="isMarked() ? 'border border-secondary ms-2 mt-2 bg-success' : 'border border-secondary ms-2 mt-2'">
        <div class="mt-2 mb-2">
               <div class="row">
                    <div>
                        <div>
                            <div id="product-name" class="text-start">
                                <h4>{{ product.productName }}</h4>
                            </div>
                        </div>
                        <div class="row text-end">
                            <div>
                                <p>Koszt:</p>
                                <h4>{{ product.price }} PLN</h4>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
    </div>
</template>

<script>
    export default {
        name: 'ProductComponent',
        props: ['product', 'markedId'],
        methods: {
            isMarked() {
                let marked = false;

                if (this.markedId === this.product.id) {
                    marked = true;
                }

                return marked;
            }
        }
    }
</script>

<style>
    #itemBorder {
        max-width: 20rem;
    }

    #product-name {
        padding-left: 0.5rem;
        padding-right: 0.5rem;
        text-align: justify;
    }
</style>
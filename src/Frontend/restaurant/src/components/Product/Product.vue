<template>
    <div id="itemBorder" class="border border-secondary ms-2 mt-2">
        <div class="mt-2 mb-2">
               <div class="row">
                    <div>
                        <div class="float-start">
                            <div id="product-name" class="col">
                                <h4>{{ product.productName }}</h4>
                            </div>
                        </div>
                        <div class="row float-end">
                            <div>
                                <p>Koszt:</p>
                                <h4>{{ product.price }} PLN</h4>
                            </div>
                            <div>
                                <button class="btn btn-primary">Dodaj</button>
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
        props: ['product']
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
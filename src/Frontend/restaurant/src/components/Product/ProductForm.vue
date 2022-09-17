<template>
    <div id="product-form-position">
        <form id="product-form" @submit.prevent="submit">
            <div class="form-group">
                <label for="produt-name">Nazwa produktu</label>
                <input v-model="newProduct.productName" name="product-name" type="text" class="form-control"/>
            </div>
            <div class="form-group">
                <label for="product-price">Cena [PLN]</label>
                <input v-model="newProduct.price" name="product-price" type="number" class="form-control"/>
            </div>
            <div class="form-group text-center">
                <label for="product-type">Typ produktu</label>
                <select v-model="newProduct.productKind" name="product-type" class="form-control">
                    <option>1</option>
                    <option>2</option>
                </select>
            </div>
            <div class="mt-2">
                <button type="button" class="btn btn-secondary me-2">
                    Reset
                </button>
                <button class="btn btn-success">
                    Wyślij
                </button>
            </div>
        </form>
    </div>
</template>

<script>
    export default {
        name: 'ProductFormComponent',
        props: [],
        data() {
            return {
                newProduct: {
                    productName: null,
                    price: null,
                    productKind: null
                }
            }
        },
        methods: {
            submit() {
                alert(JSON.stringify(this.newProduct));
            }
        }
    }
</script>

<style>
    #product-form-position {
        display: flex;
        justify-content: center;
        text-align: center;
        align-items: center;
    }

    #product-form {
        display: flex;
        flex-direction: column;
        width: 50%;
        padding-left: 5px;
        padding-right: 5px;
    }
</style>
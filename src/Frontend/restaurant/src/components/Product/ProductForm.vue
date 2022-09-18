<template>
    <div id="product-form-position">
        <form id="product-form" @submit.prevent="submit">
            <div>
                <Input :label="'Nazwa produktu'" :type="'text'" :value="newProduct.productName.value" 
                        v-model="newProduct.productName.value" :showError="newProduct.productName.showError" 
                        :error="newProduct.productName.error" 
                        @valueChanged="onChangeInput($event, 'productName')"/>
            </div>
            <div>
                <Input :label="'Cena [PLN]'" :type="'number'" :value="newProduct.price.value" 
                        v-model="newProduct.price.value" :showError="newProduct.price.showError" 
                        :error="newProduct.price.error" 
                        @valueChanged="onChangeInput($event, 'price')" :step="0.01"/>
            </div>
            <div>
                <Input :label="'Typ produktu'" :type="'select'" :value="newProduct.productKind.value" 
                        v-model="newProduct.productKind.value" :showError="newProduct.productKind.showError" 
                        :error="newProduct.productKind.error" 
                        :options="productKinds" 
                        @valueChanged="onChangeInput($event, 'productKind')"/>
            </div>
            <div class="mt-2">
                <button type="button" class="btn btn-secondary me-2" @click="reset">
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
    import Input from '../Input/Input'

    export default {
        name: 'ProductFormComponent',
        props: ['product', 'productKinds'],
        components: {
            Input
        },
        data() {
            return {
                newProduct: this.initProduct()
            }
        },
        methods: {
            initProduct() {
                return {
                    id: {
                        value: this.product?.id ?? null,
                        rules: []
                    },
                    productName: {
                        value: this.product?.productName ?? null,
                        showError: false,
                        error: '',
                        rules: [
                            v => v !== null || 'Nazwa produktu jest wymagana',
                            v => v.length > 0 || 'Nazwa produktu jest wymagana',
                            v => v.length < 100 || 'Nazwa produktu nie może być większa niż 100 znaków',
                            v => !/^\s+$/.test(v) || 'Nazwa produktu nie może zawierać puste znaki'
                        ]
                    },
                    price: {
                        value: this.product?.price ?? null,
                        showError: false,
                        error: '',
                        rules: [
                            v => v !== null || 'Cena jest wymagana',
                            v => v.toString().length > 0 || 'Cena jest wymagana',
                            v => v >= 0 || 'Cana nie może być ujemna'
                        ]
                    },
                    productKind: {
                        value: this.product?.productKind ?? null,
                        showError: false,
                        error: '',
                        rules: [
                            v => v !== null || 'Typ produktu jest wymagany',
                            v => v.length > 0 || 'Typ produktu jest wymagany',
                        ]
                    }
                }
            },
            submit() {
                const errors = [];
                for (const field in this.newProduct) {
                    const error = this.validate(this.newProduct[field].value, field);
                    if (error.length > 0) {
                        errors.push(error);
                    }
                }

                if (errors.length > 0) {
                    return;
                }

                const formToSend = {};
                for (const field in this.newProduct) {
                    formToSend[field] = this.newProduct[field].value;
                }
                
                if (formToSend.id === null){
                    delete formToSend.id;
                }
                this.$emit('submitForm', formToSend);
            },
            onChangeInput(value, fieldName) {
                this.validate(value, fieldName);
                this.newProduct[fieldName].value = value;
            },
            reset() {
                this.newProduct = this.initProduct();
            },
            validate(value, fieldName) {
                const rules = this.newProduct[fieldName].rules;
                this.newProduct[fieldName].error = '';
                this.newProduct[fieldName].showError = false;
                
                for (const rule of rules) {
                    const valid = rule(value);

                    if (valid !== true) {
                        this.newProduct[fieldName].error = valid;
                        this.newProduct[fieldName].showError = true;
                        return valid;
                    }
                }

                return '';
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
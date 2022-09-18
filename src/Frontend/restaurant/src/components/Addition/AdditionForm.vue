<template>
    <div id="product-form-position">
        <form id="product-form" @submit.prevent="submit">
            <div>
                <Input :label="'Nazwa dodatku'" :type="'text'" :value="newAddition.additionName.value" 
                        v-model="newAddition.additionName.value" :showError="newAddition.additionName.showError" 
                        :error="newAddition.additionName.error" 
                        @valueChanged="onChangeInput($event, 'additionName')"/>
            </div>
            <div>
                <Input :label="'Cena [PLN]'" :type="'number'" :value="newAddition.price.value" 
                        v-model="newAddition.price.value" :showError="newAddition.price.showError" 
                        :error="newAddition.price.error" 
                        @valueChanged="onChangeInput($event, 'price')" :step="0.01"/>
            </div>
            <div>
                <Input :label="'Typ dodatku'" :type="'select'" :value="newAddition.additionKind.value" 
                        v-model="newAddition.additionKind.value" :showError="newAddition.additionKind.showError" 
                        :error="newAddition.additionKind.error" 
                        :options="additionKinds" 
                        @valueChanged="onChangeInput($event, 'additionKind')"/>
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
        name: 'AdditionFormComponent',
        props: ['addition', 'additionKinds'],
        components: {
            Input
        },
        data() {
            return {
                newAddition: this.initAddition()
            }
        },
        methods: {
            initAddition() {
                return {
                    id: {
                        value: this.addition?.id ?? null,
                        rules: []
                    },
                    additionName: {
                        value: this.addition?.additionName ?? null,
                        showError: false,
                        error: '',
                        rules: [
                            v => v !== null || 'Nazwa dodatku jest wymagana',
                            v => v.length > 0 || 'Nazwa dodatku jest wymagana',
                            v => v.length < 100 || 'Nazwa dodatku nie może być większa niż 100 znaków',
                            v => !/^\s+$/.test(v) || 'Nazwa dodatku nie może zawierać puste znaki'
                        ]
                    },
                    price: {
                        value: this.addition?.price ?? null,
                        showError: false,
                        error: '',
                        rules: [
                            v => v !== null || 'Cena jest wymagana',
                            v => v.toString().length > 0 || 'Cena jest wymagana',
                            v => v >= 0 || 'Cana nie może być ujemna'
                        ]
                    },
                    additionKind: {
                        value: this.addition?.additionKind ?? null,
                        showError: false,
                        error: '',
                        rules: [
                            v => v !== null || 'Typ dodatku jest wymagany',
                            v => v.length > 0 || 'Typ dodatku jest wymagany',
                        ]
                    }
                }
            },
            submit() {
                const errors = [];
                for (const field in this.newAddition) {
                    const error = this.validate(this.newAddition[field].value, field);
                    if (error.length > 0) {
                        errors.push(error);
                    }
                }

                if (errors.length > 0) {
                    return;
                }

                const formToSend = {};
                for (const field in this.newAddition) {
                    formToSend[field] = this.newAddition[field].value;
                }
                
                if (formToSend.id === null) {
                    delete formToSend.id;
                }

                this.$emit('submitForm', formToSend);
            },
            onChangeInput(value, fieldName) {
                this.validate(value, fieldName);
                this.newAddition[fieldName].value = value;
            },
            reset() {
                this.newAddition = this.initAddition();
            },
            validate(value, fieldName) {
                const rules = this.newAddition[fieldName].rules;
                this.newAddition[fieldName].error = '';
                this.newAddition[fieldName].showError = false;
                
                for (const rule of rules) {
                    const valid = rule(value);

                    if (valid !== true) {
                        this.newAddition[fieldName].error = valid;
                        this.newAddition[fieldName].showError = true;
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
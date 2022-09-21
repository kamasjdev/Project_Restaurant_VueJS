<template>
    <div class="user-form-position">
        <form class="user-form" @submit.prevent="submit">
            <div>
                <Input :label="'Email'" :type="'text'" :value="newUser.email.value" 
                        v-model="newUser.email.value" :showError="newUser.email.showError" 
                        :error="newUser.email.error" 
                        @valueChanged="onChangeInput($event, 'email')"/>
            </div>
            <div>
                <Input :label="'Hasło'" :type="'password'" :value="newUser.password.value" 
                        v-model="newUser.password.value" :showError="newUser.password.showError" 
                        :error="newUser.password.error" 
                        @valueChanged="onChangeInput($event, 'password')" :step="0.01"/>
            </div>
            <div>
                <Input :label="'Rola'" :type="'select'" :value="newUser.role.value" 
                        v-model="newUser.role.value" :showError="newUser.role.showError" 
                        :error="newUser.role.error" 
                        :options="roles" 
                        @valueChanged="onChangeInput($event, 'role')"/>
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
        name: 'UserFormComponent',
        props: ['user', 'roles'],
        components: {
            Input
        },
        data() {
            return {
                newUser: this.initUser()
            }
        },
        methods: {
            initUser() {
                return {
                    id: {
                        value: this.user?.id ?? null,
                        rules: []
                    },
                    email: {
                        value: this.user?.email ?? null,
                        showError: false,
                        error: '',
                        rules: [
                            v => v !== null || 'Email jest wymagany',
                            v => v.length > 0 || 'Email jest wymagany',
                            v => /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/.test(v) || 'Niepoprawny email' //eslint-disable-line
                        ]
                    },
                    password: {
                        value: this.user?.password ?? null,
                        showError: false,
                        error: '',
                        rules: [
                            v => v !== null || 'Hasło jest wymagane',
                            v => v.length > 0 || 'Hasło jest wymagane',
                            v => /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d\w\W]{8,}$/.test(v) || 'Hasło powinno zawierać co najmniej 8 znaków w tym jedna duża litera i jedna cyfra'
                        ]
                    },
                    role: {
                        value: this.user?.role ?? null,
                        showError: false,
                        error: '',
                        rules: [
                            v => v !== null || 'Rola jest wymagana',
                            v => v.toString().length > 0 || 'Rola jest wymagana'
                        ]
                    }
                }
            },
            submit() {
                const errors = [];
                for (const field in this.newUser) {
                    const error = this.validate(this.newUser[field].value, field);
                    if (error.length > 0) {
                        errors.push(error);
                    }
                }

                if (errors.length > 0) {
                    return;
                }

                const formToSend = {};
                for (const field in this.newUser) {
                    formToSend[field] = this.newUser[field].value;
                }
                
                if (formToSend.id === null){
                    delete formToSend.id;
                }
                this.$emit('submitForm', formToSend);
            },
            onChangeInput(value, fieldName) {
                this.validate(value, fieldName);
                this.newUser[fieldName].value = value;
            },
            reset() {
                this.newUser = this.initUser();
            },
            validate(value, fieldName) {
                const rules = this.newUser[fieldName].rules;
                this.newUser[fieldName].error = '';
                this.newUser[fieldName].showError = false;
                
                for (const rule of rules) {
                    const valid = rule(value);

                    if (valid !== true) {
                        this.newUser[fieldName].error = valid;
                        this.newUser[fieldName].showError = true;
                        return valid;
                    }
                }

                return '';
            }
        }
    }
</script>

<style>
    .user-form-position {
        display: flex;
        justify-content: center;
        text-align: center;
        align-items: center;
    }

    .user-form {
        display: flex;
        flex-direction: column;
        width: 50%;
        padding-left: 5px;
        padding-right: 5px;
    }
</style>
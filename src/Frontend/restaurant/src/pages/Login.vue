<template>
    <div class="login-form-position">
        <div class="login-form">
            <div>
                <h3>Logowanie</h3>
            </div>
            <form>
                <div>
                    <InputComponent :label="'Email'" :type="'text'" :value="email" 
                                            v-model="email" :showError="emailError.length > 0" 
                                            :error="emailError"
                                            @valueChanged="($event) => email = $event"/>
                </div>
                <div>
                    <InputComponent :label="'Hasło'" :type="'password'" :value="password" 
                                        v-model="password" :showError="passwordError.length > 0" 
                                        :error="passwordError"
                                        @valueChanged="($event) => password = $event"/>
                </div>
                <div v-if="error" className="alert alert-danger mt-2">{{error}}</div>
                <div class="mt-2 text-end">
                    <button class="btn btn-success" @click="login">
                        Zaloguj
                    </button>
                </div>
            </form>
        </div>
    </div>
</template>

<script>
    import InputComponent from '@/components/Input/Input';
    import axios from '@/axios-setup';
    import exceptionMapper from '@/helpers/exceptionToMessageMapper';
    import * as authService from '@/services/AuthService';

    export default {
        name: 'LoginPage',
        components: {
            InputComponent
        },
        data() {
            return {
                email: '',
                emailError: '',
                password: '',
                passwordError: '',
                error: ''
            }
        },
        methods: {
            async login(event) {
                event.preventDefault();
                const valid = this.validateInputs();
                
                if (!valid) {
                    return;
                }

                try {
                    const response = await axios.post('/api/users/sign-in', {
                        email: this.email,
                        password: this.password
                    });
                    authService.login(response.data);
                    this.$router.push('/');
                } catch(exception) {
                    const message = exceptionMapper(exception);
                    this.error = message;
                    console.log(exception);
                }
            },
            validateInputs() {
                var patternWhiteSpaces = /^\s*$/ ;
                this.emailError = '';
                this.passwordError = '';

                if (patternWhiteSpaces.test(this.email)) {
                    this.emailError = 'Email nie może być pusty';
                }

                if (patternWhiteSpaces.test(this.password)) {
                    this.passwordError = 'Hasło nie może być puste';
                }

                const patternEmail = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/; //eslint-disable-line
                if (this.emailError.length === 0 && !patternEmail.test(this.email)) {
                    this.emailError = 'Niepoprawny email';
                    return;
                }

                if (this.emailError.length > 0 || this.passwordError.length > 0) {
                    return false;
                }

                return true;
            }
        }
    }
</script>

<style>
    .login-form-position {
        display: flex;
        justify-content: center;
        text-align: center;
        align-items: center;
    }

    .login-form {
        display: flex;
        flex-direction: column;
        width: 50%;
        padding-left: 5px;
        padding-right: 5px;
    }
</style>
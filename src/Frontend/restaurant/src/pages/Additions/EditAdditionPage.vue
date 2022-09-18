<template>
    <div v-if="addition === null && loading === true">
        <LoadingIconComponent />
    </div>
    <div v-else-if="addition === null && loading === false">
        <h3 class="mb-2 mt-2">Edytuj dodatek</h3>
        <div className="alert alert-danger">{{error}}</div>
    </div>
    <div v-else>
        <h3 class="mb-2 mt-2">Edytuj dodatek {{addition.additionName}}</h3>
        <div v-if="error" className="alert alert-danger">{{error}}</div>
        <AdditionFormComponent :addition="addition" :additionKinds="additionKinds" @submitForm="onSubmitForm" />
    </div>
</template>

<script>
    import AdditionFormComponent from '@/components/Addition/AdditionForm';
    import LoadingIconComponent from '@/components/LoadingIcon/LoadingIcon';
    import axios from '@/axios-setup';
    import exceptionMapper from '@/helpers/exceptionToMessageMapper';

    export default {
        name: 'EditAdditionPage',
        components: {
            AdditionFormComponent,
            LoadingIconComponent
        },
        data() {
            return {
                addition: null,
                additionKinds: [{label: 'Napój', value: 'Drink'}, {label: 'Sałatka', value: 'Salad'}],
                loading: true,
                error: ''
            }
        },
        methods: {
            async onSubmitForm(form) {
                this.error = '';
                try {
                    await axios.put(`/api/additions/${this.$route.params.additionId}`, form);
                    this.$router.push({ name: 'all-additions' });
                } catch(exception) {
                    const message = exceptionMapper(exception);
                    this.error = message;
                    console.log(exception);
                }
            }
        },
        async created() {
            try {
                const response = await axios.get(`/api/additions/${this.$route.params.additionId}`);
                this.addition = {
                    id: response.data.id,
                    additionName: response.data.additionName,
                    price: new Number(response.data.price).toFixed(2),
                    additionKind: response.data.additionKind
                }
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
</style>
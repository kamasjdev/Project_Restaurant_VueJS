<template>
    <div v-if="addition===null">
        <LoadingIconComponent />
    </div>
    <div v-else>
        <h3 class="mb-2 mt-2">Edytuj dodatek {{addition.additionName}}</h3>
        <AdditionFormComponent :addition="addition" :additionKinds="additionKinds" @submitForm="onSubmitForm" />
    </div>
</template>

<script>
    import AdditionFormComponent from '@/components/Addition/AdditionForm';
    import LoadingIconComponent from '@/components/LoadingIcon/LoadingIcon';

    export default {
        name: 'EditAdditionPage',
        components: {
            AdditionFormComponent,
            LoadingIconComponent
        },
        data() {
            return {
                addition: null,
                additionKinds: [{label: 'Napój', value: 'Drink'}, {label: 'Sałatka', value: 'Salad'}]
            }
        },
        methods: {
            initAddition() {
                return {
                    id: "8d961716-d7f2-4d12-8882-99a2eaf323d8",
                    additionName: "Sałatka",
                    price: new Number(5).toFixed(2),
                    additionKind: "Salad"
                }
            },
            onSubmitForm(form) {
                console.log("From EditAdditionPage, ", form);
                // send to API
                this.$router.push({ name: 'all-additions' });
                
            }
        },
        async mounted() {
            function timeout(ms) {
                return new Promise(resolve => setTimeout(resolve, ms));
            }
            
            await timeout(1000);
            this.addition = this.initAddition();
        }
    }
</script>

<style>
</style>
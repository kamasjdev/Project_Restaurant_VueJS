<template>
    <div class="additions-page">
        <div v-if="loading">
            <LoadingIconComponent />
        </div>
        <div v-if="loading === false">
            <h3 class="mt-2 mb-2">Lista dodatków</h3>
            <div class="additions-buttons mt-2 mb-2">
                <RouterButtonComponent :namedRoute="{ name: 'add-addition' }" :buttonText="'Dodaj dodatek'"/>
            </div>
            <table class="table table-bordered">
                <thead class="table-dark">
                    <tr>
                        <td>
                            id
                        </td>
                        <td>
                            Nazwa dodatku
                        </td>
                        <td>
                            Cena [PLN]
                        </td>
                        <td>
                            Typ dodatku
                        </td>
                        <td>
                            Akcja
                        </td>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="addition in additions" :key="addition.id" class="text-start">
                        <td>
                            {{ addition.id }}
                        </td>
                        <td>
                            {{ addition.additionName }}
                        </td>
                        <td>
                            {{ addition.price }}
                        </td>
                        <td>
                            {{ addition.additionKind }}
                        </td>
                        <td>
                            <RouterButtonComponent :namedRoute="{ name: 'edit-addition', params: { additionId: addition.id } }"
                                    :buttonText="'Edytuj'" :buttonClass="'btn btn-warning me-2'"
                                    :buttonType="'button'" />
                            <button class="btn btn-danger" @click="onDelete($event, addition)">Usuń</button>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <PopupComponent :open="openModal" @popupClosed="popupClosed">
            <div>Czy chcesz usunąć dodatek {{additionToDelete.additionName}}?</div>
            <div v-if="error" className="alert alert-danger mt-2 mb-2">{{error}}</div>
            <div class="mt-2">
                <button class="btn btn-danger me-2" @click="confirmDelete">Tak</button>
                <button class="btn btn-secondary" @click="popupClosed">Nie</button>
            </div>
        </PopupComponent>
    </div>
</template>

<script>
import LoadingIconComponent from '@/components/LoadingIcon/LoadingIcon';
import RouterButtonComponent from '@/components/RouterButton/RouterButton';
import PopupComponent from '@/components/Poupup/Popup';
import axios from '../../axios-setup';
import exceptionMapper from '@/helpers/exceptionToMessageMapper';

export default {
    name: 'AdditionsPage',
    components: {
        LoadingIconComponent,
        RouterButtonComponent,
        PopupComponent
    },
    data() {
        return {
            loading: true,
            additions: [],
            openModal: false,
            additionToDelete: null,
            error: ''
        }
    },
    methods: {
        onDelete(event, addition) {
            this.additionToDelete = addition;
            this.openModal = true;
        },
        popupClosed() {
            this.additionToDelete = null;
            this.openModal = false;
        },
        async confirmDelete() {
            try {
                this.error = '';
                await axios.delete(`/api/additions/${this.additionToDelete.id}`);
                this.fetchAdditions();
                this.additionToDelete = null;
                this.openModal = false;
            } catch(exception) {
                const message = exceptionMapper(exception);
                this.error = message;
                console.log(exception);
            }
        },
        async fetchAdditions() {
            try {
                const response = await axios.get('/api/additions');
                this.additions = response.data.map(a => ({
                    id: a.id,
                    additionName: a.additionName,
                    price: new Number(a.price).toFixed(2),
                    additionKind: a.additionKind
                }));
            } catch(exception) {
                console.log(exception);
            }
        }
    },
    async created() {
        this.fetchAdditions();
        this.loading = false;
    }
}
</script>

<style>
.additions-page {
    padding-left: 2rem;
    padding-right: 2rem;
}

.additions-buttons {
    float: left;
}
</style>
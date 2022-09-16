<template>
    <div id="itemAdditionBorder" class="border border-secondary ms-2 mt-2">
        <div class="mt-2 mb-2">
               <div class="row">
                    <div>
                        <div class="float-start">
                            <div id="addition-name" class="col">
                                <h4>{{ addition.additionName }}</h4>
                            </div>
                        </div>
                        <div class="row float-end">
                            <div>
                                <p>Koszt:</p>
                                <h4>{{ addition.price }} PLN</h4>
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
        name: 'AdditionComponent',
        props: ['addition']
    }
</script>

<style>
    #itemAdditionBorder {
        max-width: 20rem;
    }

    #addition-name {
        padding-left: 0.5rem;
        text-align: justify;
    }
</style>
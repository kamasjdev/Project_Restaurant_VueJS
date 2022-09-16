<template>
    <div id="itemAdditionBorder" :class="isMarked() ? 'border border-secondary ms-2 mt-2 bg-success' : 'border border-secondary ms-2 mt-2'">
        <div class="mt-2 mb-2">
               <div class="row">
                    <div>
                        <div>
                            <div id="addition-name" class="text-start">
                                <h4>{{ addition.additionName }}</h4>
                            </div>
                        </div>
                        <div class="text-end">
                            <div>
                                <p>Koszt:</p>
                                <h4>{{ addition.price }} PLN</h4>
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
        props: ['addition', 'markedId'],
        methods: {
            isMarked() {
                let marked = false;

                if (this.markedId === this.addition.id) {
                    marked = true;
                }

                return marked;
            }
        }
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
<template>
    <div id="home-body">
      <div class="mb-3">
        <h3>Witaj w Restaurant App</h3>
      </div>
      <div class="mb-3">
        <h5>Jeśli chcesz zamówić jedzenie, naciśnij przycisk poniżej lub przejdź na pasku Menu na zakładkę Zamów</h5>
      </div>
      <div>
        <RouterButtonComponent :url="'/my-order'" :buttonText="'Zamów jedzienie'" />
      </div>
    </div>
</template>

<script>
  import RouterButtonComponent from '@/components/RouterButton/RouterButton';

  export default {
    name: 'HomePage',
    components: {
      RouterButtonComponent
    }
  }
</script>

<style>
  #home-body {
    min-height: 120px;
    height: fit-content;
  }
</style>
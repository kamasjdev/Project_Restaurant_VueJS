<template>
    <div id="home-body">
      <div class="mb-3">
        <h3>Witaj w Restaurant App</h3>
      </div>
      <div class="mb-3">
        <h5>Jeśli chcesz zamówić jedzenie, naciśnij przycisk poniżej lub przejdź na pasku Menu na zakładkę Zamów</h5>
      </div>
      <div>
        <button class="btn btn-primary">
          Zamów jedzienie
        </button>
      </div>
    </div>
</template>

<script>

  export default {
    name: 'HomePage',
    components: {
    }
  }
</script>

<style>
  #home-body {
    min-height: 120px;
    height: fit-content;
  }
</style>
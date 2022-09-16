<template>
    <router-link :to="{path: url}" v-slot="{href, navigate}">
        <button :href="href" :class="buttonClass ? buttonClass : 'btn btn-primary'" @click="navigate">{{ buttonText }}</button>
    </router-link>
</template>

<script>

    export default {
      name: 'RouterButtonComponent',
      props: [ 'url', 'buttonText', 'buttonClass' ],
      components: {
      }
    }
  </script>
  
  <style>
  </style>
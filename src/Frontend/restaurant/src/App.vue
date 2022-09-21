<template>
  <HeaderComponent></HeaderComponent>
  <MenuComponent></MenuComponent>
  <router-view />
  <FooterComponent></FooterComponent>
  <notifications />
</template>

<script>
import HeaderComponent from './components/Header/Header';
import MenuComponent from './components/Menu/Menu';
import FooterComponent from './components/Footer/Footer'
import * as authService from '@/services/AuthService'

export default {
  name: 'App',
  components: {
    HeaderComponent,
    MenuComponent,
    FooterComponent
  },
  methods: {
    verifiedAuthenticated() {
      setInterval(() => {
        const authenticated = authService.isLogged();
        if (!authenticated) {
          this.$store.dispatch('isAuthenticated', false);
        }
      }, 5000);
    }
  },
  created() {
    this.verifiedAuthenticated();
  }
}
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
}
</style>

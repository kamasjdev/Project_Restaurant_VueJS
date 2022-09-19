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

export default {
  name: 'App',
  components: {
    HeaderComponent,
    MenuComponent,
    FooterComponent
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

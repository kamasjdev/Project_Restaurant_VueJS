<template>
 <nav id="navContainer" class='navbar navbar-expand-lg bg-light mb-2'>
    <ul>
        <li id="navItem" class="nav-item">
          <router-link to="/" id="navUrl" class="nav-link">Strona główna</router-link>
        </li>
        <li id="navItem" class="nav-item">
          <router-link to="/my-order" id="navUrl" class="nav-link">Zamów</router-link>
        </li>
        <li v-if="!isAuthenticated" id="navItem" class="nav-item">
          <router-link to="/login" id="navUrl" class="nav-link">Logowanie</router-link>
        </li>
        <li v-if="isAuthenticated" id="navItem" class="nav-item">
          <router-link to="/products" id="navUrl" class="nav-link">Produkty</router-link>
        </li>
        <li v-if="isAuthenticated" id="navItem" class="nav-item">
          <router-link to="/additions" id="navUrl" class="nav-link">Dodatki</router-link>
        </li>
        <li v-if="isAuthenticated && user.role === 'admin'" id="navItem" class="nav-item">
          <router-link to="/users" id="navUrl" class="nav-link">Użytkownicy</router-link>
        </li>
        <li v-if="isAuthenticated" id="navItem" class="nav-item">
          <router-link to="" id="navUrl" class="nav-link" @click="logout">Wyloguj</router-link>
        </li>
    </ul>
  </nav>
</template>

<script>
  import * as authService from '@/services/AuthService';
  import { mapGetters } from 'vuex';

  export default {
    name: 'MenuComponent',
    components: {
    },
    methods: {
      authenticated() {
        return authService.isLogged();
      },
      logout() {
        authService.logout();
        this.$router.push('/');
        this.$store.dispatch('isAuthenticated', false);
      }
    },
    computed: {
      ...mapGetters(['isAuthenticated', 'user'])
    }
  }
</script>
    
<style>
  dl, ol, ul {
    margin-bottom: 0px !important;
  }

  #navContainer {
    border: 1px solid #cdcdcd;
  }

  #navItem {
    display: inline-block;
    margin-right: 15px;
    font-size: 1.5rem;
  }

  #navUrl:hover {
    color: #534a4a
  }
</style>
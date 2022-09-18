<template>
 <nav id="navContainer" class='navbar navbar-expand-lg bg-light mb-2'>
    <ul>
        <li id="navItem" class="nav-item">
          <router-link to="/" id="navUrl" class="nav-link">Strona główna</router-link>
        </li>
        <li id="navItem" class="nav-item">
          <router-link to="/my-order" id="navUrl" class="nav-link">Zamów</router-link>
        </li>
    </ul>
  </nav>
</template>

<script>

  export default {
    name: 'MenuComponent',
    components: {
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
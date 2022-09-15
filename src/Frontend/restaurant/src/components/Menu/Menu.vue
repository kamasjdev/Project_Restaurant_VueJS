<template>
 <nav id="navContainer" class='navbar navbar-expand-lg bg-light'>
    <ul>
        <li id="navItem" class="nav-item">
          <a id="navUrl" class="nav-link" href="#">Home</a>
        </li>
        <li id="navItem" class="nav-item">
          <a id="navUrl" class="nav-link" href="#">Next</a>
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
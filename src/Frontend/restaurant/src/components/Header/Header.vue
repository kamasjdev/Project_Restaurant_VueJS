<template>
    <div class='top-sticky-bar-container js-top-sticky-bar-container'>
            <header id="header" class="header-fixed-container header-fixed-container-visible text-light">
                <div id='headerImage'>
                </div>
                <div class='container-fluid'>
                    <div class='d-flex justify-content-center align-items-center ms-2'>
                        <div id="title" class="bg-dark">
                            Restaurant App
                        </div>
                    </div>
                </div>
            </header>
        </div>
</template>

<script>

    export default {
        name: 'HeaderComponent',
        components: {
        }
    }
</script>

<style>
    #header {
        height: 100px;
        display: flex;
        justify-content: center;
        align-items: center;
        overflow: hidden;
        position: relative;
    }

    #headerImage {
        background-color: #cdcdcd;
        background-image: url('~@/assets/header.jpg');
        background-size: 100%;
        position: absolute;
        background-repeat: no-repeat;
        background-position: center;
        top: 0;
        bottom: 0%;
        left: 0%;
        right: 0%;
        z-index: -1;
    }

    #title {
        font-size: 2rem;
        padding-right: 0.8rem;
        padding-left: 0.8rem;
    }
</style>
<template>
    <div class="auth-page">
        <div class="overlay">
            <h1>Добро пожаловать в SalesTrack Med!</h1>
            <nav>
                <button class="nav-button" @click="navigateTo('login')">Вход</button>
                <button class="nav-button" @click="navigateTo('register')">Регистрация</button>
            </nav>
        </div>
    </div>
</template>

<script>
    export default {
        methods: {
            navigateTo(page) {
                this.$router.push(`/${page}`);
            },
        },
    };
</script>


<style scoped>
    * {
        box-sizing: border-box;
        margin: 0;
        padding: 0;
    }

    html, body {
        height: 100%;
        overflow: hidden;
    }

    .auth-page {
        position: absolute;
        top: 0;
        left: 0;
        width: 100vw;
        height: 100vh;
        background-image: url('@/assets/background.jpg');
        background-size: cover;
        background-position: center;
        display: flex;
        align-items: center;
        justify-content: center;
    }

    .overlay {
        background-color: rgba(0, 0, 0, 0.6);
        width: 100%;
        height: 100%;
        display: flex;
        flex-direction: column;
        align-items: center;
        justify-content: center;
        text-align: center;
        color: white;
    }

    h1 {
        font-size: 3em;
        font-family: 'Lucida Console', monospace;
        margin-bottom: 20px;
    }

    nav {
        display: flex;
        flex-direction: column;
        gap: 15px;
    }

    .nav-button {
        display: inline-block;
        padding: 10px 20px;
        font-size: 1.2em;
        color: white;
        cursor: pointer;
        border: 3px solid white;
        border-radius: 9px;
        text-decoration: none;
        transition: background 0.3s;
        background-color: transparent;
    }

        .nav-button:hover {
            background-color: white;
            color: #000;
        }
</style>

<template>
    <div class="home">
        <div class="overlay">
            <div class="user-info">
                <span>user: {{ username }}</span>
            </div>
            <h1>SalesTrack Med</h1>
            <nav>
                <router-link to="/products" class="nav-button">Товары</router-link>
                <router-link to="/sales" class="nav-button">Продажи</router-link>
                <router-link to="/employees" class="nav-button">Сотрудники</router-link>
            </nav>
            <img src="@/assets/exit.png" alt="Exit" class="exit-icon" @click="logout">
            <img src="@/assets/gear-icon.png" alt="Settings" class="settings-icon" @click="openSettings">
        </div>
    </div>
</template>

<script>
    export default {
        data() {
            return {
                username: null,
            };
        },
        mounted() {
            this.username = localStorage.getItem("username");
        },
        methods: {
            openSettings() {
                this.$router.push('/categories'); 
            },
            logout() {
                //Удаляем токен из localStorage
                localStorage.removeItem("authToken");
                this.$router.push('/');
            }
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

    .home {
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
        position: relative;
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

    .user-info {
        position: absolute;
        top: 10px;
        left: 10px;
        font-size: 1.0em;
        font-weight: bold;
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

    .exit-icon {
        width: 40px;
        height: 40px;
        filter: invert(100%);
        cursor: pointer;
        position: absolute;
        transition: filter 0.3s;
        bottom: 20px;
        right: 20px;
        border-radius: 50%;
        padding: 5px;
        border: 2.3px solid black;
    }

        .exit-icon:hover {
            filter: invert(70%);
        }

    .settings-icon {
        width: 40px;
        height: 40px;
        filter: invert(100%);
        cursor: pointer;
        position: absolute;
        transition: filter 0.3s;
        top: 20px;
        right: 20px;
        border-radius: 50%;
        padding: 5px;
        border: 2.3px solid black;
    }

        .settings-icon:hover {
            filter: invert(70%);
        }
</style>

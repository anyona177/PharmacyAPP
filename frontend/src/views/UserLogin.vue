<template>
    <div class="login-page">
        <img src="@/assets/back-arrow.png" alt="Back" class="back-icon" @click="goBack" />
        <h2>Вход</h2>
        <form @submit.prevent="login">
            <div class="form-group">
                <label for="username">Имя пользователя:</label>
                <input type="text" v-model="username" id="username" required />
            </div>
            <div class="form-group">
                <label for="password">Пароль:</label>
                <div class="password-wrapper">
                    <input v-if="passwordVisible" type="text" v-model="password" id="password" required />
                    <input v-else type="password" v-model="password" id="password" required />
                    <img src="@/assets/eye.png" alt="eye" class="eye-icon" @click="togglePasswordVisibility">
                </div>
            </div>
            <button class="save-button" type="submit">Войти</button>
            <p v-if="errorMessage" class="error-message">{{ errorMessage }}</p>
        </form>
    </div>
</template>

<script>
    export default {
        data() {
            return {
                username: "", 
                password: "", 
                passwordVisible: false,
                errorMessage: "", 
            };
        },
        methods: {
            togglePasswordVisibility() {
                this.passwordVisible = !this.passwordVisible;
            },
            async login() {
                try {
                    const response = await fetch("http://localhost:5062/login", {
                        method: "POST",
                        headers: { "Content-Type": "application/json" },
                        body: JSON.stringify({
                            username: this.username,
                            password: this.password,
                        }),
                    });

                    if (!response.ok) {
                        throw new Error("Проверьте логин и пароль.");
                    }

                    const responseJson = await response.json();

                    console.log("Ответ сервера:", responseJson);

                    //Проверяем наличие токена
                    const token = responseJson.token;
                    if (!token) {
                        throw new Error("Токен отсутствует в ответе сервера.");
                    }

                    //Сохраняем токен в localStorage
                    localStorage.setItem("authToken", token);
                    localStorage.setItem("username", this.username); 
                    console.log("Токен успешно сохранён. Перенаправление на /home");
                    this.$router.push('/home');
                } catch (error) {
                    console.error("Ошибка входа:", error);
                    this.errorMessage = error.message;
                }
            },
            goBack() {
                this.$router.push('/');
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

    .login-page {
        display: flex;
        align-items: center;
        justify-content: center;
        flex-direction: column;
        position: absolute;
        top: 0;
        left: 0;
        width: 100vw;
        height: 100vh;
        background-color: #333;
        color: #fff;
        text-align: center;
    }

    h1 {
        font-size: 2.5em;
    }

    .form-group {
        margin-bottom: 15px;
        width: 100%;
        max-width: 320px;
    }

    label {
        display: block;
        font-size: 1em;
        margin-bottom: 5px;
    }

    input {
        width: 100%;
        max-width: 300px;
        padding: 8px;
        font-size: 1em;
        margin-bottom: 10px;
        border-radius: 5px;
        border: 1px solid #ddd;
    }

    .password-wrapper {
        display: flex;
        align-items: center;
        position: relative;
        width: 100%;
        max-width: 300px;
    }

        .password-wrapper input {
            width: 100%;
            padding-right: 40px; 
        }

    .eye-icon {
        position: absolute;
        right: 5px;
        bottom: 7px;
        color: rgba(0, 0, 0, 0.6);
        cursor: pointer;
        width: 40px;
        height: 40px;
        transition: filter 0.3s;
    }

        .eye-icon:hover {
            filter: invert(70%);
        }

    .save-button {
        padding: 10px 20px;
        font-size: 1.2em;
        color: white;
        border: 3px solid white;
        border-radius: 9px;
        transition: background 0.3s;
        background-color: transparent;
        position: relative;
        cursor: pointer;
    }

        .save-button:hover {
            background-color: white;
            color: #000;
        }

    .error-message {
        color: red;
        font-size: 0.9em;
        height: 20px;
        margin-left: 10px;
        width: 85%;
        text-align: left;
    }
    .back-icon {
        width: 30px;
        height: 30px;
        filter: invert(100%);
        cursor: pointer;
        position: absolute;
        transition: filter 0.3s;
        top: 20px;
        left: 20px;
        border-radius: 50%;
        padding: 5px;
    }

        .back-icon:hover {
            filter: invert(70%);
        }
</style>

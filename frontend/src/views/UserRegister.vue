<template>
    <div class="register-page">
        <img src="@/assets/back-arrow.png" alt="Back" class="back-icon" @click="goBack" />
        <h2>Регистрация</h2>
        <form @submit.prevent="register">
            <div class="form-group">
                <label>
                    Имя пользователя:
                    <input type="text" v-model="username" required />
                </label>
            </div>
            <div class="form-group">
                <label>
                    Пароль:
                    <input type="password" v-model="password" required />
                </label>
            </div>
            <div class="form-group">
                <label>
                    Фамилия:
                    <input type="text" v-model="surname" required />
                </label>
            </div>
            <div class="form-group">
                <label>
                    Имя:
                    <input type="text" v-model="name" required />
                </label>
            </div>
            <div class="form-group">
                <label>
                    Отчество:
                    <input type="text" v-model="patronymic" />
                </label>
            </div>
            <div class="form-group">
                <label>
                    Должность:
                    <input type="text" v-model="post" required />
                </label>
            </div>
            <div class="form-group">
                <label>
                    Телефон:
                    <input type="text" v-model="phone" required />
                </label>
            </div>
            <button class="save-button" type="submit">Зарегистрироваться</button>
            <p v-if="errorMessage" class="error">{{ errorMessage }}</p>
        </form>
    </div>
</template>

<script>
    export default {
        data() {
            return {
                username: "",
                password: "",
                surname: "",
                name: "",
                patronymic: "",
                post: "",
                phone: "",
                errorMessage: "",
            };
        },
        methods: {
            async register() {
                try {
                    const response = await fetch("http://localhost:5062/register", {
                        method: "POST",
                        headers: { "Content-Type": "application/json" },
                        body: JSON.stringify({
                            username: this.username,
                            password: this.password,
                            surname: this.surname,
                            name: this.name,
                            patronymic: this.patronymic,
                            post: this.post,
                            phone: this.phone,
                        }),
                    });

                    if (!response.ok) {
                        throw new Error("Ошибка регистрации.");
                    }

                    alert("Регистрация успешна! Теперь вы можете войти.");
                    this.$router.push("/login");
                } catch (error) {
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
        overflow-y: auto;
    }

    .register-page {
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
        height: 100%;
        width: 100%;
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
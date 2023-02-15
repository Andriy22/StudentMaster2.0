<template>
  <v-container fill-height>
    <v-layout align-center justify-center>
      <v-flex md8 sm8 style="max-width: 800px" xs12>
        <v-card>
          <v-toolbar dark>
            <v-toolbar-title>Налаштування</v-toolbar-title>
          </v-toolbar>

          <v-row>
            <v-col md="2" sm="12">
              <v-avatar class="ma-5" size="125px">
                <img
                  :src="`https://localhost:7189/static/avatars/${$store.state.account.profile.avatar}`"
                  class="img-circle elevation-2"
                />
              </v-avatar>
            </v-col>
            <v-col class="ma-5" md="6">
              <v-file-input
                :rules="avatarRules"
                accept="image/png, image/jpeg, image/bmp"
                class="ma-5"
                color="pink"
                label="Аватар"
                placeholder="Оберіть новий аватар"
                prepend-icon="mdi-camera"
                @change="changeAvatar"
              ></v-file-input>
            </v-col>
          </v-row>

          <v-spacer></v-spacer>
          <v-divider></v-divider>
          <v-spacer></v-spacer>

          <v-col sm="11">
            <v-tabs color="pink">
              <v-tab>Change Password</v-tab>
              <v-tab>Application Settings</v-tab>
              <v-tab-item>
                <v-form v-model="isValid" @submit.prevent="changePassword">
                  <v-text-field
                    v-model="currentPassword"
                    :rules="passwordRules"
                    color="pink"
                    label="Поточний пароль"
                    type="password"
                  ></v-text-field>
                  <v-text-field
                    v-model="newPassword"
                    :rules="passwordRules"
                    color="pink"
                    label="Новий пароль"
                    type="password"
                  ></v-text-field>
                  <v-text-field
                    v-model="confirmPassword"
                    :rules="confirmPasswordRules"
                    color="pink"
                    label="Підтвердіть новий пароль"
                    type="password"
                  ></v-text-field>
                  <v-btn
                    :disabled="!isValid"
                    :loading="$store.state.account.status.isLoading"
                    color="pink"
                    @click="changePassword"
                    >Змінити пароль
                  </v-btn>
                </v-form>
              </v-tab-item>
              <v-tab-item>
                <div>Application Settings Content Goes Here</div>
              </v-tab-item>
            </v-tabs>
          </v-col>
        </v-card>
      </v-flex>
    </v-layout>
  </v-container>
</template>

<script>
export default {
  data() {
    return {
      isValid: false,
      currentPassword: "",
      newPassword: "",
      confirmPassword: "",
      avatarRules: [
        (value) =>
          !value ||
          value.size < 2000000 ||
          "Avatar size should be less than 2 MB!",
      ],
      passwordRules: [
        (v) => !!v || "Password is required",
        (v) =>
          (v && v.length >= 8) || "Password must contain at least 8 characters",
      ],
      confirmPasswordRules: [
        (v) => !!v || "Please confirm password",
        (v) => v === this.newPassword || "Passwords do not match",
      ],
    };
  },
  methods: {
    changeAvatar(event) {
      const accepted = ["image/png", "image/jpeg", "image/bmp"];

      if (!event) {
        this.$store.dispatch("error/displayError", {
          text: "Оберіть файл!",
        });
        return;
      }

      if (event.size >= 2000000) {
        this.$store.dispatch("error/displayError", {
          text: "Розмір файлу повинен бути менше 2Mb!",
        });
        return;
      }

      if (!accepted.includes(event?.type)) {
        this.$store.dispatch("error/displayError", {
          text: "Даний формат не підтримується! Оберіть png, jpg або bmp файл!",
        });
      }

      this.$store.dispatch("account/changeAvatar", event);
    },
    changePassword() {
      if (!this.isValid) {
        return;
      }

      this.$store.dispatch("account/changePassword", {
        password: this.currentPassword,
        newPassword: this.newPassword,
        confirmPassword: this.confirmPassword,
      });

      this.newPassword = "";
      this.currentPassword = "";
      this.confirmPassword = "";
    },
  },
};
</script>

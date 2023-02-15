<template>
  <v-container align-center class="fill-height" justify-center>
    <v-card max-width="800" min-width="250" width="700">
      <v-card-title class="headline text-center">З поверненням</v-card-title>
      <v-card-text>
        <v-form v-model="isFormValid" @submit.prevent="submit">
          <v-text-field
            v-model="email"
            :disabled="$store.state.auth.process.stage == 2"
            :rules="emailRules"
            color="pink"
            label="Email"
            outlined
            prepend-icon="mdi-email"
            type="email"
            variant="outlined"
          ></v-text-field>

          <v-text-field
            v-if="
              $store.state.auth.process.stage == 2 &&
              !$store.state.auth.status.isConfirmed
            "
            v-model="verificationCode"
            :rules="verificationCodeRules"
            color="pink"
            label="Verification Code"
            outlined
            prepend-icon="mdi-key"
            type="number"
          ></v-text-field>

          <v-text-field
            v-if="$store.state.auth.process.stage == 2"
            v-model="password"
            :rules="passwordRules"
            color="pink"
            label="Password"
            outlined
            prepend-icon="mdi-lock"
            type="password"
          ></v-text-field>

          <v-text-field
            v-if="
              $store.state.auth.process.stage == 2 &&
              !$store.state.auth.status.isConfirmed
            "
            v-model="confirmPassword"
            :rules="confirmPasswordRules"
            color="pink"
            label="Confirm Password"
            outlined
            prepend-icon="mdi-lock"
            type="password"
          ></v-text-field>
        </v-form>
        <v-card-actions class="justify-start">
          <v-btn
            :disabled="!isFormValid"
            :loading="$store.state.auth.status.isLoading"
            color="pink"
            type="submit"
            @click="submit"
          >
            <span class="white--text px-8">Далі</span>
          </v-btn>
          <a
            v-if="
              $store.state.auth.process.stage == 2 &&
              $store.state.auth.status.isConfirmed
            "
            class="blue--text px-8"
            >Відновити пароль</a
          >
        </v-card-actions>
      </v-card-text>
    </v-card>
  </v-container>
</template>
<script>
export default {
  data() {
    return {
      stage: 1,
      email: "",
      verificationCode: "",
      password: "",
      confirmPassword: "",
      isFormValid: false,
      emailRules: [
        (v) => !!v || "Email is required",
        (v) => /.+@.+/.test(v) || "Email must be valid",
      ],
      verificationCodeRules: [
        (v) => !!v || "Verification code is required",
        (v) => (v && v.length === 8) || "Verification code must be 8 digits",
      ],
      passwordRules: [
        (v) => !!v || "Password is required",
        (v) =>
          (v && v.length >= 8) || "Password must contain at least 8 characters",
      ],
      confirmPasswordRules: [
        (v) => !!v || "Please confirm password",
        (v) => v === this.password || "Passwords do not match",
      ],
    };
  },
  mounted() {
    this.$store.dispatch("auth/logout");
  },
  methods: {
    submit() {
      if (!this.isFormValid) {
        return;
      }

      if (this.$store.state.auth.process.stage == 1) {
        this.$store.dispatch("auth/checkAccountConfirmation", this.email);
        return;
      }
      if (
        this.$store.state.auth.process.stage == 2 &&
        this.$store.state.auth.status.isConfirmed
      ) {
        this.$store.dispatch("auth/login", {
          email: this.email,
          password: this.password,
        });
      }
      if (
        this.$store.state.auth.process.stage == 2 &&
        !this.$store.state.auth.status.isConfirmed
      ) {
        this.$store.dispatch("auth/confirmAccount", {
          code: this.verificationCode,
          password: this.password,
          confirmPassword: this.confirmPassword,
        });
      }
    },
  },
};
</script>

<style scoped></style>

<template>
  <v-container align-center class="fill-height" justify-center>
    <v-card max-width="800" min-width="250" width="700">
      <v-card-title class="headline">Login</v-card-title>
      <v-card-subtitle>With ❤ by StudentMaster Team</v-card-subtitle>
      <v-card-text>
        <v-form v-model="isFormValid">
          <v-text-field
            v-model="email"
            :rules="emailRules"
            label="Email"
            prepend-icon="mdi-email"
            type="email"
          ></v-text-field>
          <v-text-field
            v-model="password"
            :rules="passwordRules"
            label="Password"
            prepend-icon="mdi-lock"
            type="password"
          ></v-text-field>
        </v-form>
        <v-card-actions class="justify-start">
          <v-btn :disabled="!isFormValid" type="submit">
            <span class="white--text px-8">Login</span>
          </v-btn>
        </v-card-actions>
      </v-card-text>
    </v-card>
  </v-container>
</template>
<script>
export default {
  data() {
    return {
      email: "",
      password: "",
      isFormValid: false,
      emailRules: [
        (v) => !!v || "Email is required",
        (v) => /.+@.+/.test(v) || "Email must be valid",
      ],
      passwordRules: [
        (v) => !!v || "Password is required",
        (v) => v.length >= 8 || "Password must be at least 8 characters",
      ],
    };
  },
};
</script>

<style scoped></style>

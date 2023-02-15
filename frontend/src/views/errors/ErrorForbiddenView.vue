<template>
  <v-container align-center cclass="fill-height" justify-center>
    <v-alert
      border="bottom"
      class="text-center"
      colored-border
      elevation="2"
      type="error"
    >
      <h1>Доступ до цієї сторінки обмежено</h1>
      Зверніться до адміністратора сайту, якщо ви вважаєте, що це помилка.
    </v-alert>
  </v-container>
</template>

<script>
export default {
  name: "ErrorForbiddenView",
};
</script>

<style scoped></style>

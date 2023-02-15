<template>
  <v-item-group>
    <v-container>
      <v-row>
        <v-col cols="12" md="4">
          <v-item v-slot="{ selectedClass, toggle }">
            <v-card
              :class="['d-flex align-center', selectedClass]"
              dark
              height="200"
              @click="toggle"
            >
              <div class="text-h3 flex-grow-1 text-center">
                <v-icon color="orange" disabled x-large
                  >mdi-account-group
                </v-icon>
                Користувачі
              </div>
            </v-card>
          </v-item>
        </v-col>
        <v-col cols="12" md="4">
          <v-item v-slot="{ selectedClass, toggle }">
            <v-card
              :class="['d-flex align-center', selectedClass]"
              dark
              height="200"
              @click="toggle"
            >
              <div class="text-h3 flex-grow-1 text-center">
                <v-icon color="orange" disabled x-large> mdi-group</v-icon>
                Групи
              </div>
            </v-card>
          </v-item>
        </v-col>

        <v-col cols="12" md="4">
          <v-item v-slot="{ selectedClass, toggle }">
            <v-card
              :class="['d-flex align-center', selectedClass]"
              dark
              height="200"
              @click="toggle"
            >
              <div class="text-h3 flex-grow-1 text-center">
                <v-icon color="orange" disabled x-large> mdi-text-long</v-icon>
                Предмети
              </div>
            </v-card>
          </v-item>
        </v-col>
      </v-row>

      <v-row>
        <v-col cols="12" md="4">
          <v-item v-slot="{ selectedClass, toggle }">
            <v-card
              :class="['d-flex align-center', selectedClass]"
              dark
              height="200"
              @click="toggle"
            >
              <div class="text-h3 flex-grow-1 text-center">
                <v-icon color="orange" disabled x-large>
                  mdi-console-line
                </v-icon>
                Консоль
              </div>
            </v-card>
          </v-item>
        </v-col>

        <v-col cols="12" md="4">
          <v-item v-slot="{ selectedClass, toggle }">
            <v-card
              :class="['d-flex align-center', selectedClass]"
              dark
              height="200"
              @click="toggle"
            >
              <div class="text-h3 flex-grow-1 text-center">
                <v-icon color="orange" disabled x-large> mdi-chat</v-icon>
                Чат
              </div>
            </v-card>
          </v-item>
        </v-col>
      </v-row>
    </v-container>
  </v-item-group>
</template>

<script>
export default {
  name: "AdminDashboardView",
};
</script>

<style scoped></style>

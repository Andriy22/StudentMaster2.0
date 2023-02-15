<template>
  <v-app id="inspire">
    <v-navigation-drawer
      v-model="drawer"
      :mini-variant.sync="mini"
      app
      clipped
      disable-resize-watcher
    >
      <v-list v-if="$store.state.auth.status.loggedIn">
        <v-list-item class="px-2">
          <v-list-item-avatar>
            <v-img
              :src="`https://localhost:7189/static/avatars/${$store.state.account.profile.avatar}`"
              alt="avatar"
            ></v-img>
          </v-list-item-avatar>

          <v-list-item-title
            >{{ $store.state.auth.user.fullName }}
          </v-list-item-title>

          <v-btn icon @click="mini = !mini">
            <v-icon>mdi-chevron-left</v-icon>
          </v-btn>
        </v-list-item>

        <v-list-item link>
          <v-list-item-content>
            <v-list-item-subtitle
              >{{ $store.state.auth.user.userName }}
            </v-list-item-subtitle>
          </v-list-item-content>
        </v-list-item>
      </v-list>

      <v-divider></v-divider>

      <v-list dense>
        <router-link
          v-for="(item, index) in routes"
          :key="item.text + index"
          :to="item.link"
          style="text-decoration: none; color: inherit"
        >
          <v-list-item
            v-if="item.onlyAuth || !item.onlyAuth"
            :key="item.text"
            :link="!item.isSpacer"
          >
            <v-subheader v-if="item.isSpacer && item.isSubHeader"
              >{{ item.text }}
            </v-subheader>
            <v-divider v-if="item.isSpacer"></v-divider>

            <v-list-item-action v-if="!item.isSpacer">
              <v-icon>{{ item.icon }}</v-icon>
            </v-list-item-action>
            <v-list-item-content v-if="!item.isSpacer">
              <v-list-item-title> {{ item.text }}</v-list-item-title>
            </v-list-item-content>
          </v-list-item>
        </router-link>
      </v-list>
    </v-navigation-drawer>

    <v-app-bar app clipped-left dense>
      <v-app-bar-nav-icon @click="drawer = !drawer"></v-app-bar-nav-icon>

      <v-toolbar-title>StudentMaster</v-toolbar-title>

      <v-spacer></v-spacer>
      <v-icon
        v-if="$vuetify.theme.dark"
        class="nav-item"
        @click="$vuetify.theme.dark = !$vuetify.theme.dark"
        >mdi-moon-waning-crescent
      </v-icon>
      <v-icon
        v-if="!$vuetify.theme.dark"
        class="nav-item"
        @click="$vuetify.theme.dark = !$vuetify.theme.dark"
      >
        mdi-weather-sunny
      </v-icon>

      <v-menu
        v-if="$store.state.auth.status.loggedIn"
        :offset-y="true"
        bottom
        transition="slide-y-transition"
      >
        <template v-slot:activator="{ on, attrs }">
          <v-btn outlined v-bind="attrs" v-on="on">
            {{ $store.state.auth.user.userName }}
          </v-btn>
        </template>
        <v-list>
          <router-link
            class="text-decoration-none text--primary"
            to="/account/profile"
          >
            <v-list-item class="cursor-pointer">
              <v-list-item-icon>
                <v-icon>mdi-cog</v-icon>
              </v-list-item-icon>
              <v-list-item-title>Налаштування</v-list-item-title>
            </v-list-item>
          </router-link>
          <v-list-item
            class="cursor-pointer"
            @click="$store.dispatch('auth/logout')"
          >
            <v-list-item-icon>
              <v-icon>mdi-exit-to-app</v-icon>
            </v-list-item-icon>
            <v-list-item-title>Вийти</v-list-item-title>
          </v-list-item>
        </v-list>
      </v-menu>
    </v-app-bar>

    <v-main>
      <router-view></router-view>
    </v-main>

    <v-snackbar v-model="$store.state.error.showError" timeout="3000">
      {{ $store.state.error.error }}
      <template v-slot:action="{ attrs }">
        <v-btn
          color="red"
          text
          v-bind="attrs"
          @click="
            $store.dispatch('error/displayError', { isHidden: true, text: '' })
          "
        >
          Close
        </v-btn>
      </template>
    </v-snackbar>
  </v-app>
</template>

<script>
export default {
  data: () => ({
    drawer: false,
    mini: false,
    routes: [
      {
        isSpacer: true,
        isSubHeader: true,
        text: "Адмін панель",
        link: "",
        onlyAuth: true,
      },
      {
        icon: "mdi-view-dashboard",
        text: "Головна",
        link: "/admin/dashboard",
        onlyAuth: true,
      },
    ],
  }),
  mounted() {
    this.$store.dispatch("account/getAvatar");
  },
};
</script>

<style lang="scss">
.backend-status {
  padding: 1rem;
}

.nav-item {
  margin-right: 10px;
}

.cursor-pointer {
  cursor: pointer;
}
</style>

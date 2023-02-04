<template>
  <v-app id="inspire">
    <v-navigation-drawer
      v-model="drawer"
      :mini-variant.sync="mini"
      app
      clipped
      disable-resize-watcher
    >
      <v-list>
        <v-list-item class="px-2">
          <v-list-item-avatar>
            <v-img
              alt="avatar"
              src="https://randomuser.me/api/portraits/men/85.jpg"
            ></v-img>
          </v-list-item-avatar>

          <v-list-item-title>Александрук Андрій</v-list-item-title>

          <v-btn icon @click="mini = !mini">
            <v-icon>mdi-chevron-left</v-icon>
          </v-btn>
        </v-list-item>

        <v-list-item link>
          <v-list-item-content>
            <v-list-item-title class="text-h6"
              >admin@gmail.com
            </v-list-item-title>
            <v-list-item-subtitle>Admin</v-list-item-subtitle>
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
    </v-app-bar>

    <v-main>
      <router-view></router-view>
    </v-main>
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
        text: "Admin",
        link: "",
        onlyAuth: true,
      },
      {
        icon: "mdi-view-dashboard",
        text: "Головна",
        link: "/chat/private/settings",
        onlyAuth: true,
      },
    ],
  }),
};
</script>

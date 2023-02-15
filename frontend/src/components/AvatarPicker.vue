<template>
  <v-dialog
    v-model="show"
    :fullscreen="$vuetify.breakpoint.xs"
    transition="dialog-bottom-transition"
    width="500"
  >
    <v-card>
      <v-toolbar color="primary" dark>
        <v-btn dark icon @click="show = false">
          <v-icon>close</v-icon>
        </v-btn>
        <v-toolbar-title>Select an Avatar</v-toolbar-title>
        <v-spacer></v-spacer>
      </v-toolbar>
      <v-layout v-if="avatars" row wrap>
        <v-flex v-for="avatar in avatars" :key="avatar.id" d-flex sm3 xs4>
          <v-card class="d-flex" flat tile>
            <v-card-text class="d-flex">
              <v-avatar
                :class="{ current: avatar.id === currentAvatar }"
                class="avatar-picker-avatar"
                size="96"
                @click="selectAvatar(avatar)"
              >
                <img :src="'/avatars/' + avatar.path" />
              </v-avatar>
            </v-card-text>
          </v-card>
        </v-flex>
      </v-layout>
    </v-card>
  </v-dialog>
</template>

<script>
export default {
  props: {
    currentAvatar: {
      type: String,
      required: true,
    },
    value: Boolean,
  },
  async mounted() {
    // await this.$store.dispatch("fetchAvatars");
  },
  computed: {
    avatars() {
      return this.$store.state.avatars;
    },
    show: {
      get() {
        return this.value;
      },
      set(value) {
        this.$emit("input", value);
      },
    },
  },
  methods: {
    selectAvatar(avatar) {
      this.$emit("selected", avatar.id);
      this.show = false;
    },
  },
};
</script>

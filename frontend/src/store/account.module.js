import AccountService from "@/services/account.service";

export const account = {
  namespaced: true,
  state: {
    status: {
      isLoading: false,
    },
    profile: {
      avatar: "default.png",
    },
  },
  actions: {
    changeAvatar({ commit, dispatch }, file) {
      commit("startLoading");
      AccountService.changeAvatar(file)
        .then((res) => {
          commit("changeAvatar", res.data);
          dispatch(
            "error/displayError",
            { text: "Ваш аватар успішно змінено!" },
            { root: true }
          );
        })
        .finally(() => {
          commit("stopLoading");
        });
    },

    getAvatar({ commit }) {
      commit("startLoading");
      AccountService.getAvatar()
        .then((res) => {
          commit("changeAvatar", res.data);
        })
        .finally(() => {
          commit("stopLoading");
        });
    },

    changePassword({ commit, dispatch }, data) {
      commit("startLoading");
      AccountService.changePassword(data)
        .then(() => {
          dispatch(
            "error/displayError",
            { text: "Ваш пароль успішно змінено!" },
            { root: true }
          );
        })
        .finally(() => {
          commit("stopLoading");
        });
    },
  },
  getters: {},
  mutations: {
    changeAvatar(state, avatarName) {
      state.profile.avatar = avatarName;
    },
    startLoading(state) {
      state.status.isLoading = true;
    },
    stopLoading(state) {
      state.status.isLoading = false;
    },
  },
};

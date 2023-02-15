export const error = {
  namespaced: true,
  state: {
    error: "",
    showError: false,
  },
  actions: {
    displayError(context, data) {
      context.commit("SHOW_ERROR", data);
    },
  },
  getters: {},
  mutations: {
    SHOW_ERROR(state, data) {
      state.showError = !data.isHidden;
      state.error = data.text;
    },
  },
};

import AuthService from "../services/auth.service";
import AccountService from "../services/account.service";
import tokenService from "@/services/token.service";
import Router from "@/router";

const user = JSON.parse(localStorage.getItem("auth"));

const initialState = user
  ? {
      process: { stage: 1 },
      status: { loggedIn: true, isLoading: false, isConfirmed: false },
      user,
    }
  : {
      process: { stage: 1 },
      status: { loggedIn: false, isLoading: false, isConfirmed: false },
      user: null,
    };

export const auth = {
  namespaced: true,
  state: initialState,
  actions: {
    login({ commit, dispatch }, data) {
      commit("startLoading");
      return AuthService.login(data).then(
        (user) => {
          commit("loginSuccess", user);
          commit("stopLoading");
          return Promise.resolve(user);
        },
        (error) => {
          dispatch(
            "error/displayError",
            { text: error?.response?.data["error"] },
            { root: true }
          );
          commit("loginFailure");
          commit("stopLoading");
          return Promise.reject(error);
        }
      );
    },
    confirmAccount({ commit, dispatch }, data) {
      commit("startLoading");
      AccountService.confirmAccount(data).then(
        () => {
          commit("stopLoading");
          dispatch(
            "error/displayError",
            { text: "Ваш аккаунт підтверджено! Ви можете авторизуватись. " },
            { root: true }
          );
          dispatch("logout");
        },
        () => {
          commit("stopLoading");
        }
      );
    },
    checkAccountConfirmation({ commit, dispatch }, email) {
      commit("startLoading");
      AuthService.checkAccountConfirmation(email).then(
        (confirmed) => {
          commit("setConfirmationStatus", confirmed);
          commit("stopLoading");
          commit("setProcessStage", 2);
        },
        (error) => {
          commit("stopLoading");
          commit("setProcessStage", 1);
          dispatch(
            "error/displayError",
            { text: error?.response?.data["error"] },
            { root: true }
          );
        }
      );
    },
    logout({ commit }) {
      AuthService.logout();
      commit("setProcessStage", 1);
      commit("logout");

      Router.push("/login");
    },
    refreshToken({ commit }, user) {
      commit("refreshToken", user);
    },
  },
  mutations: {
    loginSuccess(state, user) {
      state.status.loggedIn = true;
      state.user = user;
      tokenService.setUser(user);
    },

    startLoading(state) {
      state.status.isLoading = true;
    },

    setConfirmationStatus(state, status) {
      state.status.isConfirmed = status;
    },
    setProcessStage(state, stage) {
      state.process.stage = stage;
    },
    stopLoading(state) {
      state.status.isLoading = false;
    },
    loginFailure(state) {
      state.status.loggedIn = false;
      state.user = null;
    },
    logout(state) {
      state.status.loggedIn = false;
      state.user = null;
    },
    refreshToken(state, user) {
      state.status.loggedIn = true;
      state.user = user;
    },
  },
};

import Vue from "vue";
import Vuex from "vuex";
import { auth } from "@/store/auth.module";
import { error } from "@/store/error.module";
import { account } from "@/store/account.module";

Vue.use(Vuex);

export default new Vuex.Store({
  state: {},
  getters: {},
  mutations: {},
  actions: {},
  modules: {
    auth,
    error,
    account,
  },
});

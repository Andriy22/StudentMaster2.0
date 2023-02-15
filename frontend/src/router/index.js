import Vue from "vue";
import VueRouter from "vue-router";
import HomeView from "../views/HomeView.vue";
import LoginView from "../views/LoginView";
import AdminDashboardView from "../views/admin/AdminDashboardView";
import store from "../store/index";
import ErrorForbiddenView from "@/views/errors/ErrorForbiddenView";
import AccountProfileView from "@/views/account/AccountProfileView";

Vue.use(VueRouter);

const routes = [
  {
    path: "/",
    name: "home",
    component: HomeView,
  },
  {
    path: "/login",
    name: "login",
    component: LoginView,
  },
  {
    path: "/forbidden",
    name: "forbidden",
    component: ErrorForbiddenView,
  },
  {
    path: "/account/profile",
    name: "profile",
    component: AccountProfileView,
    meta: { requiresAuth: true },
  },
  {
    path: "/admin/dashboard",
    name: "admin-dashboard",
    component: AdminDashboardView,
    meta: { requiresAuth: true, requiredRole: "Admin" },
  },
  {
    path: "/about",
    name: "about",
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () =>
      import(/* webpackChunkName: "about" */ "../views/AboutView.vue"),
  },
];

const router = new VueRouter({
  routes,
  mode: "history",
});

router.beforeEach((to, from, next) => {
  // check if the user is authenticated
  if (to.matched.some((record) => record.meta.requiresAuth)) {
    if (!store.state.auth.status.loggedIn) {
      // if the user is not authenticated, redirect to the login page
      next({
        path: "/login",
      });
    } else {
      if (
        to.matched.some((record) => record.meta.requiredRole) &&
        !store.state.auth.user.roles.includes(to.meta.requiredRole)
      ) {
        next({
          path: "/forbidden",
        });
      } else {
        // if the user is authenticated, allow the navigation
        next();
      }
    }
  } else {
    // if the route doesn't require authentication, allow the navigation
    next();
  }
});

export default router;

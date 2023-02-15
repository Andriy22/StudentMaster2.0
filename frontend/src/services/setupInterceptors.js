import axiosInstance from "./api";
import TokenService from "./token.service";

const setup = (store) => {
  axiosInstance.interceptors.request.use(
    (config) => {
      const token = TokenService.getLocalAccessToken();
      if (token) {
        config.headers["Authorization"] = "Bearer " + token;
      }
      return config;
    },
    (error) => {
      return Promise.reject(error);
    }
  );

  axiosInstance.interceptors.response.use(
    (res) => {
      return res;
    },
    async (err) => {
      const originalConfig = err.config;

      if (err.response.status !== 401) {
        if (err.response.data?.error) {
          store.dispatch("error/displayError", {
            isHidden: false,
            text: err.response.data?.error,
          });
        } else {
          if (err.response.data?.errors) {
            store.dispatch("error/displayError", {
              isHidden: false,
              text: Object.values(err.response.data?.errors)[0],
            });
          }
        }
      }

      if (originalConfig.url !== "/auth/authorize" && err.response) {
        // Access Token was expired

        if (err.response.status === 401 && !originalConfig._retry) {
          originalConfig._retry = true;

          try {
            const rs = await axiosInstance.post("/auth/refresh", {
              accessToken: TokenService.getLocalAccessToken(),
              refreshToken: TokenService.getLocalRefreshToken(),
            });

            const user = rs.data;

            store.dispatch("account/getAvatar");
            store.dispatch("auth/refreshToken", user);
            TokenService.updateLocalAccessToken(user.token);
            TokenService.updateLocalRefreshToken(user.refreshToken);

            return axiosInstance(originalConfig);
          } catch (_error) {
            store.dispatch("error/displayError", {
              isHidden: false,
              text: "Your token is expired, please relogin!",
            });
            store.dispatch("auth/logout");
            return Promise.reject(_error);
          }
        }
      }

      return Promise.reject(err);
    }
  );
};

export default setup;

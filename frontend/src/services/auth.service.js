import api from "./api";
import TokenService from "./token.service";

class AuthService {
  login({ email, password }) {
    return api
      .post("/auth/authorization", {
        email,
        password,
      })
      .then((response) => {
        if (response.data.token) {
          TokenService.setUser(response.data);
        }

        return response.data;
      });
  }

  checkAccountConfirmation(email) {
    return api
      .get(`/account/check-account-confirmation/${email}`)
      .then((response) => {
        return response.data;
      });
  }

  logout() {
    TokenService.removeUser();
  }
}

export default new AuthService();

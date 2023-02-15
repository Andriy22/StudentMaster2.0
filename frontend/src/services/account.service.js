import api from "./api";

class AccountService {
  confirmAccount(data) {
    return api
      .post("/account/confirm-account", {
        code: data.code,
        password: data.password,
        confirmPassword: data.password,
      })
      .then((response) => {
        return response.data;
      });
  }

  changePassword(data) {
    return api.post("/account/change-password", data).then((response) => {
      return response.data;
    });
  }

  changeAvatar(file) {
    const data = new FormData();

    data.append("file", file);
    return api
      .post("/account/change-avatar", data, {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      })
      .then((response) => response);
  }

  getAvatar() {
    return api.get("/account/get-avatar").then((response) => response);
  }
}

export default new AccountService();

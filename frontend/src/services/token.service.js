class TokenService {
  getLocalRefreshToken() {
    const user = JSON.parse(localStorage.getItem("auth"));
    return user?.refreshToken;
  }

  getLocalAccessToken() {
    const user = JSON.parse(localStorage.getItem("auth"));
    return user?.accessToken;
  }

  updateLocalAccessToken(token) {
    const user = JSON.parse(localStorage.getItem("auth"));
    user.token = token;
    localStorage.setItem("auth", JSON.stringify(user));
  }

  updateLocalRefreshToken(token) {
    const user = JSON.parse(localStorage.getItem("auth"));
    user.refreshToken = token;
    localStorage.setItem("auth", JSON.stringify(user));
  }

  getUser() {
    return JSON.parse(localStorage.getItem("auth"));
  }

  setUser(user) {
    localStorage.setItem("auth", JSON.stringify(user));
  }

  removeUser() {
    localStorage.removeItem("auth");
  }
}

export default new TokenService();

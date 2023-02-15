import axios from "axios";
import { API } from "@/config";

const instance = axios.create({
  baseURL: API,
  headers: {
    "Content-Type": "application/json",
  },
});

export default instance;

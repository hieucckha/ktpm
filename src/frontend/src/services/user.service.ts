import { editUserDto } from "./../api/store/auth/interface";
import axios from "../api/AxiosClient";
import { UserProfileDto } from "../api/store/auth/interface";
import localStorageService from "./localStorage.service";

const userService = {
	async create(username: string, password: string) {
		const response = await axios.post("/users", { username, password });
		return response.data;
	},
	async getProfile() {
		const response = await axios.get("api/auth");
		localStorageService.setItem("user", response.data?.email);
		return response.data;
	},
	async updateProfile(user: editUserDto) {
		const response = await axios.put("/api/user", user);
		return response.data;
	},
};
export default userService;

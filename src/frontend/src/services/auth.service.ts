// AuthService.js
import axios from "../api/AxiosClient";

const AuthService = {
	async login(email: string, password: string) {
		const response = await axios.post("/api/auth", { email, password });
		return response.data;
	},

	async signup(
		email: string,
		password: string,
		firstName: string,
		lastName: string,
		studentId: string
	) {
		const response = await axios.post("/api/user", {
			email,
			password,
			firstName,
			lastName,
			studentId,
		});
		return response.data;
	},

	async signInGoogle(credential: string) {
		const response = await axios.post("/api/auth/google", { credential });
		return response.data;
	},

	async signInFacebook(accessToken: string) {
		const response = await axios.post("/api/auth/facebook", { accessToken });
		return response.data;
	},
	async resetPassword(email: string) {
		const response = await axios.post("/api/auth/reset-password", { email });
		return response.data;
	},
	async confirmResetPassword(email: string, code: string, password: string, confirmPassword: string) {
		const response = await axios.post("/api/auth/reset-password/confirm", { email, code, password, confirmPassword});
		return response.data;
	},
	async confirmEmail(email: string, code: string) {
		const response = await axios.post(`/api/auth/activate/confirm`, {
			email,
			code,
		});
		return response;
	},
};

export default AuthService;

import { useMutation, useQueryClient } from "@tanstack/react-query";

import userService from "../../../services/user.service";

import localStorageService from "../../../services/localStorage.service";
import AuthServices from "../../../services/auth.service";

import type {
	SignInData,
	SignInFacebookData,
	SignInGoogleData,
	// UserProfileDto,
	resetPasswordDto,
	signUpDto,
	resetPasswordConfirmDto,
	editUserDto,
} from "./interface";

export const useSignInMutation = () =>
	useMutation({
		mutationFn: (data: SignInData) =>
			AuthServices.login(data.email, data.password),
		retry: false,
		onSuccess(data) {
			localStorageService.setItem("auth", data.token);
		},
	});

export const useSignInGoogleMutation = () =>
	useMutation({
		mutationFn: (data: SignInGoogleData) =>
			AuthServices.signInGoogle(data.credential),
		retry: false,
		onSuccess(data) {
			localStorageService.setItem("auth", data.token);
		},
	});

export const useSignInFacebookMutation = () =>
	useMutation({
		mutationFn: (data: SignInFacebookData) =>
			AuthServices.signInFacebook(data.accessToken),
		retry: false,
		onSuccess(data) {
			localStorageService.setItem("auth", data.token);
		},
	});

export const userUpdateProfileMutation = () => {
	const queryClient = useQueryClient();
	return useMutation({
		// eslint-disable-next-line @typescript-eslint/no-shadow
		mutationFn: (user: editUserDto) => userService.updateProfile(user),
		retry: false,
		onSuccess() {
			queryClient.invalidateQueries({ queryKey: ["auth"] });
		},
	});
};

export const userSignUpMutation = () =>
	useMutation({
		// eslint-disable-next-line @typescript-eslint/no-shadow
		mutationFn: (user: signUpDto) =>
			AuthServices.signup(
				user.email,
				user.password,
				user.firstName,
				user.lastName,
				user.studentId
			),
		retry: false,

	});
export const useResetPasswordMutation = () =>
	useMutation({
		// eslint-disable-next-line @typescript-eslint/no-shadow, @typescript-eslint/no-unsafe-return
		mutationFn: (data: resetPasswordDto) => AuthServices.resetPassword(data.email),
		retry: false,
	});

export const userConfirmResetPasswordMutation = () =>
	// eslint-disable-next-line react-hooks/rules-of-hooks
	useMutation({
		mutationFn: (data: resetPasswordConfirmDto) =>
			AuthServices.confirmResetPassword(data.email, data.code, data.password, data.confirmPassword),
		retry: false,
	});


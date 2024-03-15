import { useState, type FC } from "react";
import { Link, useLocation, useNavigate } from "react-router-dom";
import FacebookLogo from "../assets/facebook-logo.svg";

import {
	useSignInFacebookMutation,
	useSignInGoogleMutation,
	useSignInMutation,
} from "../api/store/auth/mutations";
import type {
	SignInData,
	SignInFacebookData,
	SignInGoogleData,
} from "../api/store/auth/interface";
import { GoogleLogin, type CredentialResponse } from "@react-oauth/google";
// import { GoogleLogin } from "@react-oauth/google";
import { useLogin } from "react-facebook";
import Header from "./LandingPage/Header";
import { Axios } from "axios";
import { message } from "antd";

interface SignInProps {
	afterLoginUrl?: string;
}

/**
 * Sign-in page.
 */
const SignIn: FC<SignInProps> = ({
	afterLoginUrl = "/home",
}: SignInProps): JSX.Element => {
	const navigate = useNavigate();
	const location = useLocation();
	const [email, setEmail] = useState("");
	const [password, setPassword] = useState("");

	const handleEmailChange = (
		event: React.ChangeEvent<HTMLInputElement>
	): void => {
		setEmail(event.target.value);
	};
	const handlePasswordChange = (
		event: React.ChangeEvent<HTMLInputElement>
	): void => {
		setPassword(event.target.value);
	};

	const signInMutation = useSignInMutation();
	const handleFormSubmit = async (
		event: React.FormEvent<HTMLFormElement>
	): Promise<void> => {
		event.preventDefault();

		const data: SignInData = {
			email,
			password,
		};
		signInMutation.mutate(data, {
			onSuccess() {
				var origin = location.state?.from?.pathname || afterLoginUrl;

				navigate(origin);
			},
			onError(error: any) {
				message.error(error?.response.data.title);
				console.error(error);
			},
		});
	};

	const { login: facebookLogin, isLoading: isFacebookLogin } = useLogin();
	const signInFacebookMutation = useSignInFacebookMutation();
	const handleFacebookLogin = async () => {
		try {
			const response = await facebookLogin({
				scope: "email",
			});

			const data: SignInFacebookData = {
				accessToken: response.authResponse.accessToken,
			};

			signInFacebookMutation.mutate(data, {
				onSuccess() {
					var origin = location.state?.from?.pathname || afterLoginUrl;

					navigate(origin);
				},
				onError(error) {
					console.error(error);
				},
			});
		} catch (error: any) {
			console.log(error);
		}
	};

	const signInGoogleMutation = useSignInGoogleMutation();
	const handleGoogleLogin = async (response: CredentialResponse) => {
		if (response.credential === undefined) {
			return;
		}

		const data: SignInGoogleData = {
			credential: response.credential,
		};

		signInGoogleMutation.mutate(data, {
			onSuccess() {
				var origin = location.state?.from?.pathname || afterLoginUrl;

				navigate(origin);
			},
			onError(error) {
				console.error(error);
			},
		});
	};

	return (
		<div className="flex flex-col h-screen bg-white overflow-auto">
			{/* Page content. */}

			<Header />
			<main className="grow ">
				<section className=" h-full">
					<div className="max-w-6xl mx-auto px-4 sm:px-6">
						<div className="pt-32 pb-12 md:pt-40 md:pb-20">
							{/* Page header */}
							<div className="max-w-3xl mx-auto text-center pb-12 md:pb-20">
								<h1 className="h1">Welcome back.</h1>
							</div>

							{/* Form */}
							<div className="max-w-sm mx-auto">
								<form onSubmit={handleFormSubmit}>
									<div className="flex flex-wrap -mx-3 mb-4">
										<div className="w-full px-3">
											<label
												className="block text-gray-800 text-sm font-medium mb-1"
												htmlFor="email"
											>
												Email
											</label>
											<input
												id="email"
												value={email}
												type="email"
												onChange={handleEmailChange}
												className="form-input w-full text-gray-800"
												placeholder="Enter your email address"
												required
											/>
										</div>
									</div>
									<div className="flex flex-wrap -mx-3 mb-4">
										<div className="w-full px-3">
											<div className="flex justify-between">
												<label
													className="block text-gray-800 text-sm font-medium mb-1"
													htmlFor="password"
												>
													Password
												</label>
												<Link
													to="/reset-password"
													className="text-sm font-medium text-blue-600 hover:underline"
													tabIndex={-1}
												>
													Reset password
												</Link>
											</div>
											<input
												id="password"
												value={password}
												onChange={handlePasswordChange}
												type="password"
												className="form-input w-full text-gray-800"
												placeholder="Enter your password"
												required
											/>
										</div>
									</div>
									<div className="flex flex-wrap -mx-3 mb-4">
										<div className="w-full px-3">
											<div className="flex justify-between">
												<label className=" flex items-center">
													<input type="checkbox" className="form-checkbox" />
													<span className="text-gray-600 ml-2">
														Keep me signed in
													</span>
												</label>
											</div>
										</div>
									</div>
									<div className="flex flex-wrap -mx-3 mt-6 mb-3">
										<div className="w-full px-3">
											<button className="btn text-white bg-black hover:bg-black w-full">
												Sign in
											</button>
										</div>
									</div>
									<div className="flex flex-wrap -mx-3 mb-3">
										<div className="w-full px-3">
											<button
												className="btn px-0 bg-white hover:bg-slate-50 w-full relative flex items-center"
												style={{ padding: "0 0 0 0 " }}
												onClick={handleFacebookLogin}
												disabled={isFacebookLogin}
											>
												<img
													src={FacebookLogo}
													alt="Facebook"
													className="w-4 h-4 fill-current text-white opacity-75 flex-shrink-0 mx-4"
												/>
												<span className="flex-auto pl-16 pr-8 -ml-16">
													Sign in with Facebook
												</span>
											</button>
										</div>
									</div>
									<div className="flex flex-wrap -mx-3">
										<div className="w-full px-3">
											<GoogleLogin
												size="large"
												locale="en_US"
												width={383}
												onSuccess={handleGoogleLogin}
												onError={() => console.log("error")}
											/>
										</div>
									</div>
								</form>
								<div className="text-gray-600 text-center mt-6">
									Don’t you have an account?{" "}
									<Link
										to="/sign-up"
										className="text-blue-600 hover:underline transition duration-150 ease-in-out"
									>
										Sign up
									</Link>
								</div>
							</div>
						</div>
					</div>
				</section>
			</main>
		</div>
	);
};
export default SignIn;

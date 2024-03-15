/* eslint-disable @typescript-eslint/no-unsafe-member-access */
/* eslint-disable @typescript-eslint/no-unsafe-assignment */
/* eslint-disable @typescript-eslint/no-misused-promises */
import { useState, type FC, useEffect } from "react";
import { Link, useNavigate } from "react-router-dom";
import { Select, Toast } from "flowbite-react";

import { userSignUpMutation } from "../api/store/auth/mutations";
import type { signUpDto } from "../api/store/auth/interface";
import Header from "./LandingPage/Header";
import Swal from 'sweetalert2';
/**
 * Sign up page.
 */
const SignUp: FC = (): JSX.Element => {
	const navigate = useNavigate();
	const [form, setForm] = useState({
		firstName: "",
		lastName: "",
		email: "",
		password: "",
		confirmPassword: "",
		studentId: "",
	});
	const [errMatch, setErrMatch] = useState(false);
	const [isStudent, setIsStudent] = useState(false);
	// eslint-disable-next-line @typescript-eslint/explicit-function-return-type
	const handleChange = (
		e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
	):void => {
		const { id, value } = e.target;
		setForm((prevState) => ({
			...prevState,
			[id]: value,
		}));
	};
	useEffect(() => {
		if (form.password !== form.confirmPassword) {
			setErrMatch(true);
		} else {
			setErrMatch(false);
		}
	}, [form.password, form.confirmPassword]);

	const mutation = userSignUpMutation();

	// sent to backend to verify
	// eslint-disable-next-line require-await
	const handleFormSubmit = async (
		event: React.FormEvent<HTMLFormElement>
		// eslint-disable-next-line @typescript-eslint/require-await
	): Promise<void> => {
		event.preventDefault();

		const data: signUpDto = {
			firstName: form.firstName,
			lastName: form.lastName,
			email: form.email,
			password: form.password,
			studentId: form.studentId ?? null,
		};
		mutation.mutate(data, {
			onSuccess() {
				Swal.fire({
					title: 'Success',
					text: 'Signup successfully, Have a nice day !!!',
					icon: 'success',
					confirmButtonText: 'Ok'
				}).then(() => {

					navigate("/sign-in");
				})
			},
			onError(error: any) {
				console.error(error);
				Swal.fire({
					title: 'Fail',
					text: error.response.data.title,
					icon: 'error',
				})
				Toast({
					title: "Fail",
					// description: error.message,
					// type: "error",
					duration: 500,
					// isClosable: true,
				});
			},
		});
	};

	return (
		<div className="flex flex-col bg-white h-screen overflow-auto">
			{/* Page content. */}
			<Header />

			<main className="grow h-screen">
				<section className=" h-full">
					<div className="max-w-6xl mx-auto px-4 sm:px-6">
						<div className="pt-32 pb-12 md:pt-40 md:pb-20">
							{/* Page header */}
							<div className="max-w-3xl mx-auto text-center pb-12 md:pb-20">
								<h1 className="h1">Welcome.</h1>
							</div>

							{/* Form */}
							<div className="max-w-sm mx-auto">
								<form onSubmit={handleFormSubmit}>
									<div className="flex flex-wrap -mx-3 mb-4">
										<div className="w-1/2 px-3">
											<label
												className="block text-gray-800 text-sm font-medium mb-1"
												htmlFor="first-name"
											>
												Fist name
											</label>
											<input
												id="firstName"
												type="text"
												className="form-input w-full text-gray-800"
												placeholder="Enter your first name"
												required
												onChange={handleChange}
											/>
										</div>
										<div className="w-1/2 px-3">
											<label
												className="block text-gray-800 text-sm font-medium mb-1"
												htmlFor="last-name"
											>
												Last name
											</label>
											<input
												id="lastName"
												type="text"
												className="form-input w-full text-gray-800"
												placeholder="Enter your last name"
												required
												onChange={handleChange}
											/>
										</div>
									</div>
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
												type="email"
												className="form-input w-full text-gray-800"
												placeholder="Enter your email address"
												required
												onChange={handleChange}
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
											</div>
											<input
												id="password"
												type="password"
												className="form-input w-full text-gray-800"
												placeholder="Enter your password"
												required
												onChange={handleChange}
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
													Confirm password
												</label>
											</div>
											<input
												id="confirmPassword"
												type="password"
												className="form-input w-full text-gray-800"
												placeholder="Enter your confirm password"
												required
												onChange={handleChange}
											/>
										</div>
									</div>
									{errMatch && (
										<div className="flex flex-wrap -mx-3 mb-4">
											<div className="w-full px-3">
												<div className="flex justify-between">
													<p className="text-red-600">
														Password does not match
													</p>
												</div>
											</div>
										</div>
									)}
									<div className="flex flex-wrap -mx-3 mb-4">
										<div className="w-full px-3">
											<div className="flex justify-between">
												<label
													className="block text-gray-800 text-sm font-medium mb-1"
													htmlFor="role"
												>
													Role
												</label>
											</div>
											<Select
												defaultValue="Teacher"
												onChange={() => {
													setIsStudent(!isStudent);
												}}
												id="role"
												className=" w-full text-gray-800"
												required
											>
												<option>Teacher</option>
												<option>Student</option>
											</Select>
										</div>
									</div>
									{isStudent && (
										<div className="flex flex-wrap -mx-3 mb-4">
											<div className="w-full px-3">
												<div className="flex justify-between">
													<label
														className="block text-gray-800 text-sm font-medium mb-1"
														htmlFor="studentId"
													>
														Student Id
													</label>
												</div>
												<input
													id="studentId"
													type="text"
													className="form-input w-full text-gray-800"
													placeholder="Enter your studentId"
													required
													onChange={handleChange}
												/>
											</div>
										</div>
									)}

									<div className="flex flex-wrap -mx-3 mb-3 mt-6">
										<div className="w-full px-3">
											<button className="btn text-white bg-gray-900 hover:bg-gray-700 w-full">
												Sign up
											</button>
										</div>
									</div>
								</form>
								<div className="text-gray-600 text-center mt-6">
									Already have an account? {}
									<Link
										to="/sign-in"
										className="text-blue-600 hover:underline transition duration-150 ease-in-out"
									>
										Sign in
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

export default SignUp;

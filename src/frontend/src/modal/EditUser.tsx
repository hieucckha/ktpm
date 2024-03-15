/* eslint-disable @typescript-eslint/explicit-function-return-type */
/* eslint-disable @typescript-eslint/no-unsafe-assignment */
import { useState, type FC, useEffect } from "react";
import { Button, Label, Modal, TextInput, Toast } from "flowbite-react";

import useAuth from "../hooks/auth";
import type { UserProfileDto, editUserDto } from "../api/store/auth/interface";
import { userUpdateProfileMutation } from "../api/store/auth/mutations";
import { Email } from "read-excel-file";
import { App } from "antd";

interface EditUserProps {
	handleCloseModalEditUser: () => void;
	openModal: boolean;
}

const EditUser: FC<EditUserProps> = ({
	handleCloseModalEditUser,
	openModal,
}): JSX.Element => {
	const [formData, setFormData] = useState<UserProfileDto>({
		fullName: "",
		lastName: "",
		firstName: "",
		studentId: "",
		email: "",
		id: 0,
		role: "",
	});

	const { data: user, isLoading } = useAuth();
	const app = App.useApp();
	const { message } = app;

	useEffect(() => {
		if (!user) {
			return;
		}

		/**
		 * Get user profile.
		 */

		setFormData({
			fullName: user?.fullName ?? "",
			lastName: user?.lastName ?? "",
			firstName: user?.firstName ?? "",
			studentId: user?.studentId ?? "",
			email: user?.email ?? "",
			id: user?.id ?? null,
			role: "",
		});
	}, [user]);

	if (isLoading) {
		return <></>;
	}

	const handleChange = (error: React.ChangeEvent<HTMLInputElement>) => {
		// setFormData({
		// 	...formData,
		// 	[error.target.id]: error.target.value,
		// });
		const { id, value } = error.target;
		setFormData((prevState) => ({
			...prevState,
			[id]: value,
		}));
	};

	const updateProfile = userUpdateProfileMutation();

	const handleSubmit = (error: React.FormEvent) => {
		error.preventDefault();
		const data: editUserDto = {
			firstName: formData.firstName,
			lastName: formData.lastName,
			studentId: formData.studentId,
			// email: formData.email,
		};
		updateProfile.mutate(data, {
			onSuccess() {
				handleCloseModalEditUser();
				message.success("Update profile successfully");
			},
			onError(error: any) {
				console.error(error);
				message.error(error.response.data.title);
			},
		});
	};

	const handleCloseModal = (): void => {
		handleCloseModalEditUser();
	};

	return (
		<Modal show={openModal} size="md" onClose={handleCloseModal} popup>
			<Modal.Header />
			<Modal.Body>
				<div className="space-y-6">
					<h3 className="text-xl font-medium text-gray-900 dark:text-white">
						Edit profile{" "}
					</h3>

					<form onSubmit={handleSubmit}>
						<div>
							<div className="mb-2 block">
								<Label htmlFor="email" value="Email" />
							</div>
							<TextInput
								id="email"
								type="email"
								placeholder="example@gnail.com"
								value={formData.email}
								// required
								// onChange={handleChange}
								disabled
							/>
						</div>
						<div>
							<div className="mb-2 block">
								<Label htmlFor="name" value="Name" />
							</div>
							<TextInput
								id="name"
								placeholder="Nguyen Van A"
								value={formData.fullName}
								// onChange={handleChange}
								disabled
							/>
						</div>

						<div>
							<div className="mb-2 block">
								<Label htmlFor="name" value="First Name" />
							</div>
							<TextInput
								id="firstName"
								type="text"
								placeholder="Nguyen"
								value={formData.firstName}
								required
								onChange={handleChange}
							/>
						</div>

						<div>
							<div className="mb-2 block">
								<Label htmlFor="name" value="Last Name" />
							</div>
							<TextInput
								id="lastName"
								type="text"
								placeholder="Van A"
								value={formData.lastName}
								required
								onChange={handleChange}
							/>
						</div>
						<div className={formData.role === "Student" ? "" : "hidden"}>
							<div className="mb-2 block">
								<Label htmlFor="studentId" value="Student Id" />
							</div>
							<TextInput
								id="studentId"
								type="text"
								placeholder="20127001"
								value={formData.studentId}
								onChange={handleChange}
								// required
							/>
						</div>

						{/* <div className="w-full text-center flex justify-end">
							<Button onClick={handleSubmit}>Update</Button>
						</div> */}
						<div className="flex flex-wrap -mx-3 mb-3 mt-6">
							<div className="w-full px-3">
								<button className="btn text-white bg-gray-900 hover:bg-gray-700 w-full">
									Update
								</button>
							</div>
						</div>
					</form>
				</div>
			</Modal.Body>
		</Modal>
	);
};

export default EditUser;

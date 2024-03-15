/* eslint-disable @typescript-eslint/explicit-function-return-type */
/* eslint-disable @typescript-eslint/no-unsafe-assignment */
import { useState, type FC, useEffect } from "react";
import { Label, Modal, TextInput } from "flowbite-react";
import { ToggleSwitch } from "flowbite-react";
import useAuth from "../hooks/auth";
import type { UserProfileDto, editUserDto } from "../api/store/auth/interface";
import { userUpdateProfileMutation } from "../api/store/auth/mutations";
import { App, Spin } from "antd";
import { ClassQuery } from "../api/store/class/interface";
import { useEditClassMutation } from "../api/store/class/mutation";
import useClassDetail from "../hooks/useClassDetail";
import classService from "../services/class.service";
import { AxiosError } from "axios";

interface EditUserProps {
	handleCloseModalEditUser: () => void;
	openModal: boolean;
}

interface EditClassProps {
	handleCloseModalEditClass: () => void;
	openModal: boolean;
	classId: string;
}

const EditClass: FC<EditClassProps> = ({
	handleCloseModalEditClass,
	openModal,
	classId = "",
}): JSX.Element => {
	const [switch1, setSwitch1] = useState(false);
	const [formData, setFormData] = useState<ClassQuery>({
		name: "",
		id: 0,
		description: "",
	});

	// const { data: user, isLoading } = useAuth();
	const { data, isLoading } = useClassDetail(classId);
	const app = App.useApp();
	const { message } = app;
	useEffect(() => {
		if (!data) {
			return;
		}
		setSwitch1(data?.isActivated);
		setFormData({
			name: data?.name ?? "",
			description: data?.description ?? "",
			id: data?.id ?? null,
		});
	}, [data]);

	const handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
		// setFormData({
		// 	...formData,
		// 	[error.target.id]: error.target.value,
		// });
		const { id, value } = event.target;
		setFormData((prevState) => ({
			...prevState,
			[id]: value,
		}));
	};

	const editClass = useEditClassMutation(classId);

	const handleSubmit = (event: React.FormEvent) => {
		event.preventDefault();
		const data: ClassQuery = {
			name: formData.name,
			description: formData.description,
			id: formData.id,
		};
		editClass.mutate(data, {
			onSuccess() {
				handleCloseModalEditClass();
				message.success("Update class successfully");
			},
			onError(error: any) {
				console.error(error);

				message.error(error.response.data.title);
			},
		});
	};

	const handleCloseModal = (): void => {
		handleCloseModalEditClass();
	};
	const handleToggle = () => {
		setSwitch1(!switch1);
		classService
			.toggleActivateClass(classId)
			.then(() => {
				message.success("Change status of class success");
			})
			.catch((err: AxiosError) => {
				const mess = (err.response?.data as any).title ?? "Change status of class failed";

				message.error(mess);
			});
	};

	if (isLoading) {
		return null;
	}

	return (
		<Modal show={openModal} size="md" onClose={handleCloseModal} popup>
			<Modal.Header></Modal.Header>
			<Modal.Body>
				<div className="space-y-2">
					<h3 className="text-xl font-medium text-gray-900 dark:text-white">
						Class Setting
					</h3>
					<form onSubmit={handleSubmit}>
						<div>
							<div className="mb-2 block">
								<Label htmlFor="name" value="Class name" />
							</div>
							<TextInput
								id="name"
								type="text"
								placeholder="Class name"
								value={formData.name}
								required
								onChange={handleChange}
							/>
						</div>

						<div>
							<div className="mb-2 block">
								<Label htmlFor="description" value="Description" />
							</div>
							<TextInput
								id="description"
								type="text"
								placeholder="Description"
								value={formData.description}
								required
								onChange={handleChange}
							/>
						</div>
						{classId !== "" && (
							<div>
								<div className="mb-2 block">
									<Label htmlFor="active" value="Active" />
								</div>
								<ToggleSwitch
									id="active"
									checked={switch1}
									label="Toggle active"
									onChange={handleToggle}
								/>
							</div>
						)}
						<div className="flex flex-wrap -mx-3 mb-3 mt-6">
							<div className="w-full px-3">
								<button className="btn text-white bg-gray-900 hover:bg-gray-700 w-full">
									{editClass.isPending && <Spin fullscreen />}
									Save
								</button>
							</div>
						</div>
					</form>
				</div>
			</Modal.Body>
		</Modal>
	);
};

export default EditClass;

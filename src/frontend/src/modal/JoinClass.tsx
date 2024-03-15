import { useState, type FC } from "react";
import { Button, Label, Modal, TextInput } from "flowbite-react";
import { useJoinClassMutation } from "../api/store/class/mutation";
import { App } from "antd";
import { AxiosError } from "axios";
interface JoinClassProps {
	handleCloseModalJoinClass: () => void;
	openModal: boolean;
}

const JoinClass: FC<JoinClassProps> = ({
	handleCloseModalJoinClass,
	openModal,
}): JSX.Element => {
	const [formData, setFormData] = useState({
		code: "",
	});
	const { notification } = App.useApp();
	// eslint-disable-next-line @typescript-eslint/explicit-function-return-type
	const handleChange = (
		error: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
	) => {
		setFormData({
			...formData,
			[error.target.id]: error.target.value,
		});
	};
	const mutation = useJoinClassMutation();
	// eslint-disable-next-line @typescript-eslint/explicit-function-return-type
	const handleSubmit: React.MouseEventHandler<HTMLButtonElement> = async (
		event
		// eslint-disable-next-line @typescript-eslint/require-await
	): Promise<void> => {
		event.preventDefault();
		mutation.mutate(formData, {
			onSuccess() {
				notification.success({
					message: "Join class success",
					description: "You have joined the class successfully",
				});
				handleCloseModalJoinClass();
			},
			onError(error) {
				if (error instanceof AxiosError) {
					const data = error?.response?.data;
					if (data.title) {
						return notification.error({
							message: "Join class failed",
							description: data.title,
						});
					}
				}

				return notification.error({
					message: "Join class failed",
					description: "Please check your code again",
				});
			},
		});
	};

	return (
		<>
			<Modal
				show={openModal}
				size="md"
				popup
				onClose={handleCloseModalJoinClass}
			>
				<Modal.Header />
				<Modal.Body>
					<div className="space-y-6">
						<h3 className="text-xl font-medium text-gray-900 dark:text-white">
							Join class
						</h3>
						<div>
							<div className="mb-2 block">
								<Label htmlFor="code" value="Class Code" />
							</div>
							<TextInput
								id="code"
								onChange={handleChange}
								value={formData.code}
								placeholder="ABC_5435"
								required
							/>
						</div>

						<div className="w-full flex justify-end">
							<Button onClick={handleSubmit}>Join</Button>
						</div>
					</div>
				</Modal.Body>
			</Modal>
		</>
	);
};

export default JoinClass;

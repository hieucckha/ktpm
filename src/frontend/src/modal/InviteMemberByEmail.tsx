import { useState, type FC } from "react";
import { Button  } from "flowbite-react";
import { UserOutlined } from '@ant-design/icons';
import { useInviteClassMutationByEmail } from "../api/store/class/mutation";
import { App, Flex, Input, Modal } from "antd";
import { AxiosError } from "axios";
import { useParams } from "react-router-dom";
interface InviteClassProps {
	handleCloseModalInviteClass: () => void;
	openModal: boolean;
}

const InviteMemberByEmail: FC<InviteClassProps> = ({
	handleCloseModalInviteClass,
	openModal,
}): JSX.Element => {
    const {id} = useParams();
	const [formData, setFormData] = useState({
		email: "",
        courseId: id as string,
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
	const mutation = useInviteClassMutationByEmail();
	// eslint-disable-next-line @typescript-eslint/explicit-function-return-type
	const handleSubmit: React.MouseEventHandler<HTMLButtonElement> = async (
		event
		// eslint-disable-next-line @typescript-eslint/require-await
	): Promise<void> => {
		event.preventDefault();
		mutation.mutate(formData, {
			onSuccess() {
				notification.success({
                    message: "Invite class successfully",
                    description: "Please check your email",
                });
                handleCloseModalInviteClass();
			},
			onError(error) {
				if (error instanceof AxiosError) {
					const data = error?.response?.data;
					if (data.title) {
						return notification.error({
							message: "Invite class failed",
							description: data.title,
						});
					}
				}

                return notification.error({
                    message: "Invite class failed",
                    description: "Something went wrong",
                });
			},
		});
	};

	return (
		<>
			<Modal title="Invite by email" open={openModal} 
            onOk={handleSubmit} onCancel={handleCloseModalInviteClass}
            width={600}
            footer={[
               <Flex key="footer" justify="end">
                <Button className="flex-end" key="submit" onClick={handleSubmit}>
                    Invite
                </Button>
                </Flex>
            ]}
            >
            <Input  size="large" className="invisible" placeholder="Email" prefix={<UserOutlined />} />

         <Input size="large" value={formData.email} id="email" name="email" onChange={handleChange} placeholder="Email" prefix={<UserOutlined />} />
      </Modal>
		</>
	);
};

export default InviteMemberByEmail;

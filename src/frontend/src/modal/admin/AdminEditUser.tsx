import React, { useState } from "react";
import { Button, Form, Input, Modal, Radio, Space, message } from "antd";
import EditUser from "../EditUser";
import { EditUserMutation } from "../../api/store/class/mutation";



interface ModalProps {
	handleCancel: () => void;
	openModal: boolean;
	data?: any;
}
type LayoutType = Parameters<typeof Form>[0]["layout"];
const AdminEditUser: React.FC<ModalProps> = ({
	handleCancel,
	openModal,
	data,
}) => {
	const [form] = Form.useForm();
	const [formLayout, setFormLayout] = useState<LayoutType>("horizontal");

	const onFormLayoutChange = ({ layout }: { layout: LayoutType }) => {
		setFormLayout(layout);
	};
	const mutation = EditUserMutation();
	const formItemLayout =
		formLayout === "horizontal"
			? { labelCol: { span: 4 }, wrapperCol: { span: 14 } }
			: null;
	const handleSubmit = (values: any) => {
		mutation.mutate({id :data?.key , ...values}, {
			onSuccess: () => {
				message.success("Edit user successfully");
				handleCancel();
			},
			onError: (error) => {
				message.error(error.message);
			},
		});
	}

	return (
		<>
			<Modal
				title="Edit User"
				open={openModal}
				onOk={() => {
					form.validateFields().then(() => form.submit());
					// handleCancel();
				}}
				onCancel={handleCancel}
			>
				<Form
					{...formItemLayout}
					layout={formLayout}
					form={form}
					initialValues={data}
					onValuesChange={onFormLayoutChange}
					style={{ maxWidth: formLayout === "inline" ? "none" : 600 }}
					onFinish={(values) => handleSubmit(values)}
					
				>
					
					<Form.Item
						label="First Name"
						name="firstName"
						rules={[
							{
								required: true,
							},
						]}
					>
						<Input
							required
							value={data.firstName}
							className="w-full rounded text-sm border-gray-200 font-si"
							defaultValue={data.firstName}
							placeholder="van A"
						/>
					</Form.Item>
					<Form.Item
						label="Last Name"
						name="lastName"
						rules={[
							{
								required: true,
							},
						]}
					>
						<Input
							required
							value={data.lastName}
							className="rounded text-sm border-gray-200"
							defaultValue={data.lastName}
							placeholder="Nguyen"
						/>
					</Form.Item>
					{data.role === "Student" ? (
						<Form.Item label="Student ID" name="studentId">
							<Input
							value={data.studentId}
								className="rounded text-sm border-gray-200"
								defaultValue={data.studentId ?? ""}
								placeholder="13465443"
							/>
						</Form.Item>
					) : null}
				
				</Form>
			</Modal>
		</>
	);
};

export default AdminEditUser;

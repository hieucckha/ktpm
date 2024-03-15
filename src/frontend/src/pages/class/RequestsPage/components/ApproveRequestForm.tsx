import { App, Button, Form, InputNumber, Space } from "antd";
import { useApproveRequest } from "../../../../api/store/request/mutation";
import { useQueryClient } from "@tanstack/react-query";

interface ApproveRequestForm {
	classId: string;
	reqId: string;
	gradeValue?: number;
	onClose: () => void;
}

function ApproveRequestForm({
	gradeValue,
	onClose,
	classId,
	reqId,
}: ApproveRequestForm) {
	const queryClient = useQueryClient();
	const [form] = Form.useForm();
	const { message } = App.useApp();
	const { mutate: approveReqMutate, isPending: isApprovePending } =
		useApproveRequest(classId);

	const handleApproveRequest = (values) => {
		approveReqMutate(
			{
				req_id: reqId,
				gradeValue: values.gradeValue,
			},
			{
				onSuccess() {
					message.success("Approved successfully");
					queryClient.invalidateQueries({ queryKey: ["request", reqId] });
				},
				onError() {
					message.error("Approved failed");
				},
				onSettled() {
					form.resetFields();
					onClose();
				},
			}
		);
	};

	return (
		<Form
			form={form}
			layout="vertical"
			initialValues={{
				gradeValue,
			}}
			onFinish={handleApproveRequest}
		>
			<Form.Item
				name="gradeValue"
				label="New Grade"
				rules={[
					{
						required: true,
						message: "Please input new grade",
					},
				]}
			>
				<InputNumber min={0} max={10} />
			</Form.Item>

			<Space>
				<Button
					type="default"
					onClick={() => {
						form.resetFields();
						onClose();
					}}
				>
					Cancel
				</Button>
				<Button
					className="bg-blue-500 hover:!bg-blue-600"
					type="primary"
					htmlType="submit"
					loading={isApprovePending}
				>
					Approve
				</Button>
			</Space>
		</Form>
	);
}

export default ApproveRequestForm;

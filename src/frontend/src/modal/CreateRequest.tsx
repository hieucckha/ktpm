import { type FC } from "react";
import { Modal } from "flowbite-react";
// import { useAddGradeMutation } from "../api/store/class/mutation";
import { App, Form, Input, InputNumber, Select, Button, Space } from "antd";
import { useParams } from "react-router-dom";
import { listGradeAllClassQuery } from "../api/store/class/queries";
import { useCreateGradeRequest } from "../api/store/request/mutation";
import { AxiosError } from "axios";
import { useMemo } from "react";
interface CreateRequestProps {
	handleClose: () => void;
	open: boolean;
}

const CreateRequest: FC<CreateRequestProps> = ({
	handleClose,
	open,
}): JSX.Element => {
	const [form] = Form.useForm();
	const { id } = useParams();
	const { message } = App.useApp();
	const { data: listDataDetail } = listGradeAllClassQuery(id as string);
	const { mutate: createGradeMutate, isPending } = useCreateGradeRequest();

	const colOptions = useMemo(() => {
		const gradeCol = listDataDetail?.gradeCompositionDtos?.filter(
			(item) => item.isFinal
		);
		return gradeCol?.map((col) => {
			return {
				label: col.name.toString(),
				value: col.id,
			};
		});
	}, [listDataDetail?.gradeCompositionDtos]);

	const handleFinishForm = (values: any) => {
		createGradeMutate(values, {
			onSuccess: () => {
				message.success("Create request successfully");
				handleClose();
			},
			onError: (error) => {
				if (error instanceof AxiosError) {
					return message.error(error.response?.data.title ?? error.message);
				}
				message.error(error.message);
			},
		});
	};

	const filterOption = (
		input: string,
		option?: { label: string; value: string }
	) => (option?.label ?? "").toLowerCase().includes(input.toLowerCase());

	return (
		<Modal show={open} size="md" popup onClose={handleClose}>
			<Modal.Header />
			<Modal.Body>
				<div className="space-y-6">
					<h3 className="text-xl font-medium text-gray-900 dark:text-white">
						Create request
					</h3>
					<Form form={form} layout="vertical" onFinish={handleFinishForm}>
						<Form.Item
							name="gradeCompositionId"
							label="Grade Composition"
							rules={[
								{
									required: true,
									message: "You must select a column for request grade",
								},
							]}
						>
							<Select<Number>
								showSearch
								placeholder="Select column"
								optionFilterProp="children"
								filterOption={filterOption}
								options={colOptions}
							/>
						</Form.Item>

						<Form.Item
							name="exceptedGrade"
							label="Grade"
							rules={[
								{
									required: true,
									message: "You must your expected grade",
								},
							]}
						>
							<InputNumber
								className="w-full"
								max={10}
								min={0}
								placeholder="Input your expected grade"
							/>
						</Form.Item>

						<Form.Item
							name="reason"
							label="Reason"
							rules={[
								{
									required: true,
									message: "You must input reason",
								},
							]}
						>
							<Input.TextArea
								className="w-full"
								placeholder="Input your reason"
								showCount
								maxLength={500}
								rows={3}
							/>
						</Form.Item>
					</Form>
					<Space className="w-full flex justify-end">
						<Button
							onClick={() => {
								form.resetFields();
								handleClose();
							}}
						>
							Cancel
						</Button>
						<Button
							loading={isPending}
							className="twp bg-blue-500"
							onClick={() => form.validateFields().then(() => form.submit())}
							type="primary"
						>
							Create
						</Button>
					</Space>
				</div>
			</Modal.Body>
		</Modal>
	);
};

export default CreateRequest;

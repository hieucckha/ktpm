import { useState, type FC, useEffect } from "react";
import { Label, Modal, TextInput, Textarea } from "flowbite-react";
// import { useAddGradeMutation } from "../api/store/class/mutation";
import { App, Form, Input, InputNumber, Select, Button, Space } from "antd";
import { useParams } from "react-router-dom";
import {
	listGradeAllClassQuery,
	listGradeOneStudentQuery,
} from "../api/store/class/queries";
import { useEditGradeMutation } from "../api/store/class/mutation";
interface AddGradeProps {
	handleCloseModalAddGrade: () => void;
	openModal: boolean;
}

const AddGrade: FC<AddGradeProps> = ({
	handleCloseModalAddGrade,
	openModal,
}): JSX.Element => {
	const [form] = Form.useForm();
	const { id } = useParams();
	const { message } = App.useApp();
	const { data: listDataDetail } = listGradeAllClassQuery(id as string);
	const {mutate: editGradeMutate, isPending} = useEditGradeMutation();

    const handleFinishForm = (values: any) => {
        editGradeMutate(values, {
            onSuccess: () => {
                message.success("Edit grade successfully");
                handleCloseModalAddGrade();
            },
            onError: (error) => {
                message.error(error.message);
            }
        })
    }
	
	const filterOption = (
		input: string,
		option?: { label: string; value: string }
	) => (option?.label ?? "").toLowerCase().includes(input.toLowerCase());

	return (
		<>
			<Modal
				show={openModal}
				size="md"
				popup
				onClose={handleCloseModalAddGrade}
			>
				<Modal.Header />
				<Modal.Body>
					<div className="space-y-6">
						<h3 className="text-xl font-medium text-gray-900 dark:text-white">
							Edit grade
						</h3>
						<Form form={form} layout="vertical" onFinish={handleFinishForm}>
							<Form.Item name="studentId" label="Student ID" rules={[
                                {
                                    required: true,
                                    message: "You must select student ID to edit grade"
                                }
                            ]}>
							

                                <Input
									className="w-full"
									placeholder="Select a studentId"
								/>
							</Form.Item>

							<Form.Item name="gradeCompositionId" label="Grade Composition" rules={[
                                {
                                    required: true,
                                    message: "You must select a column to edit grade"
                                }
                            ]}>
								<Select<Number>
									showSearch
									placeholder="Select column"
									optionFilterProp="children"
									onChange={(value) => {
										console.log(value);
									}}
									filterOption={filterOption}
									options={listDataDetail?.gradeCompositionDtos?.map((col) => {
										return {
											label: col.name.toString(),
											value: col.id,
										};
									})}
								/>
							</Form.Item>

							<Form.Item name="gradeValue" label="Grade" rules={[
                                {
                                    required: true,
                                    message: "You must input grade value"
                                },
                            ]}>
								<InputNumber
									className="w-full"
									max={10}
									min={0}
									placeholder="Input student grade"
								/>
							</Form.Item>
						</Form>
						<Space className="w-full flex justify-end">
							<Button onClick={() => {
                                form.resetFields();
                                handleCloseModalAddGrade();
                            }}>Cancel</Button>
							<Button loading={isPending} className="twp bg-blue-500" onClick={() => form.validateFields().then(() => form.submit())} type="primary">Create</Button>
                        </Space>
					</div>
				</Modal.Body>
			</Modal>
		</>
	);
};

export default AddGrade;

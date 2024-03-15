import React, { useEffect, useState } from "react";
import {
	Form,
	Input,
	InputNumber,
	Popconfirm,
	Table,
	Typography,
	Button,
	App,
	Space,
	Spin,
	Tooltip,
} from "antd";
import type { DragEndEvent } from "@dnd-kit/core";
import {
	DndContext,
	PointerSensor,
	useSensor,
	useSensors,
} from "@dnd-kit/core";
import { restrictToVerticalAxis } from "@dnd-kit/modifiers";
import {
	SortableContext,
	arrayMove,
	useSortable,
	verticalListSortingStrategy,
} from "@dnd-kit/sortable";
import { CSS } from "@dnd-kit/utilities";
import { useNavigate, useParams } from "react-router-dom";
import {
	gradeCompositions,
	newGradeCompositions,
} from "../../../api/store/class/interface";

interface EditableCellProps extends React.HTMLAttributes<HTMLElement> {
	editing: boolean;
	dataIndex: string;
	title: any;
	inputType: "number" | "text";
	record: gradeCompositions;
	index: number;
	children: React.ReactNode;
}

const EditableCell: React.FC<EditableCellProps> = ({
	editing,
	dataIndex,
	title,
	inputType,
	record,
	index,
	children,
	...restProps
}) => {
	const inputNode = inputType === "number" ? <InputNumber /> : <Input />;
	return (
		<td {...restProps}>
			{editing ? (
				<Form.Item
					name={dataIndex}
					style={{ margin: 0 }}
					rules={[
						{
							required: true,
							message: `Please Input ${title}!`,
						},
					]}
				>
					{inputNode}
				</Form.Item>
			) : (
				children
			)}
		</td>
	);
};
interface RowProps extends React.HTMLAttributes<HTMLTableRowElement> {
	"data-row-key": string;
}
const RowDragable = (props: RowProps) => {
	const {
		attributes,
		listeners,
		setNodeRef,
		transform,
		transition,
		isDragging,
	} = useSortable({
		id: props["data-row-key"],
	});

	const style: React.CSSProperties = {
		...props.style,
		transform: CSS.Transform.toString(transform && { ...transform, scaleY: 1 }),
		transition,
		cursor: "move",
		...(isDragging ? { position: "relative", zIndex: 9999 } : {}),
	};

	return (
		<tr
			{...props}
			ref={setNodeRef}
			style={style}
			{...attributes}
			{...listeners}
		/>
	);
};
import useClassDetail from "../../../hooks/useClassDetail";
import {
	useAddNewGradeComposit,
	useApproveGradeComposit,
	useDeleteNewGradeComposit,
	useUpdateGradeColumn,
	useUpdateOrderGradeComposit,
} from "../../../api/store/gradeComposits/mutation";
import { UserRole } from "../../../api/store/auth/interface";
import useAuth from "../../../hooks/auth";
import {
	CheckCircleOutlined,
	DeleteOutlined,
	EditOutlined,
} from "@ant-design/icons";

const addKeyWithId = (array: any) => {
	let arrClone = array?.map((item: any) => ({ ...item, key: item.id }));
	return arrClone;
};
const GradeStructure: React.FC = () => {
	const { id } = useParams();
	if (!id) return null;
	const { data: user } = useAuth();
	const navigate = useNavigate();
	const { message } = App.useApp();
	const { data, isLoading } = useClassDetail();
	const mutation = useUpdateOrderGradeComposit();
	const mutationAddGradeColumn = useAddNewGradeComposit();
	const mutationUpdateGradeColumn = useUpdateGradeColumn();
	const mutationDeleteGradeColumn = useDeleteNewGradeComposit();
	const mutationApproveGradeColumn = useApproveGradeComposit();

	const [form] = Form.useForm();
	const [gradeCompositions, setGradeCompositions] = useState<
		gradeCompositions[]
	>([]);
	const [editingKey, setEditingKey] = useState(0);

	const isEditing = (record: gradeCompositions) => record.id == editingKey;
	function isGradeScaleSumValid(gradeScaleArray: gradeCompositions[]) {
		const sum = gradeScaleArray.reduce(
			(total, item) => total + (item.gradeScale || 0),
			0
		);
		return sum <= 100;
	}
	const sensors = useSensors(
		useSensor(PointerSensor, {
			activationConstraint: {
				// https://docs.dndkit.com/api-documentation/sensors/pointer#activation-constraints
				distance: 1,
			},
		})
	);
	const reOrderArray = () => {
		if (!Array.isArray(gradeCompositions)) return;
		const array = gradeCompositions?.map((item, idx) => {
			return {
				...item,
				order: idx + 1,
			};
		});

		mutation.mutate(array, {
			onSuccess() {},
			onError(error) {
				console.log(error);
			},
		});
		// setGradeCompositions(array)
	};
	useEffect(() => {
		if (gradeCompositions) {
			reOrderArray();
		}
	}, [gradeCompositions]);
	useEffect(() => {
		if (data) {
			setGradeCompositions(addKeyWithId(data.gradeCompositions));
		}
	}, [data]);
	useEffect(() => {
		if (user) {
			if (user.role == UserRole.Student) {
				navigate("/home");
			}
		}
	}, [user]);

	if (isLoading)
		return (
			<div>
				<Spin fullscreen />
			</div>
		);

	const onDragEnd = ({ active, over }: DragEndEvent) => {
		if (active.id !== over?.id) {
			setGradeCompositions((prev: gradeCompositions[]) => {
				const activeIndex = prev.findIndex((i) => i.id === active.id);
				const overIndex = prev.findIndex((i) => i.id === over?.id);
				return arrayMove(prev, activeIndex, overIndex);
			});
			// save order
		}
	};
	const edit = (record: Partial<gradeCompositions>) => {
		form.setFieldsValue({ name: "", gradeScale: 0, ...record });
		if (record.id) {
			setEditingKey(record.id);
		}
	};

	const cancel = () => {
		setEditingKey(0);
	};
	const handleAdd = () => {
		const newData: newGradeCompositions = {
			name: `New gradeCompositions`,
			description: `New gradeCompositions`,
			gradeScale: 0,
			courseId: +id,
		};
		mutationAddGradeColumn.mutate(newData, {
			onSuccess() {},
			onError(error: any) {
				console.log(error);
			},
		});
	};

	const save = async (key: React.Key) => {
		try {
			const row = (await form.validateFields()) as gradeCompositions;
			if (isNaN(row.gradeScale)) {
				message.error("GradeScale must be a number");
				return;
			}
			row.gradeScale = isNaN(row.gradeScale) ? 0 : Number(row.gradeScale);

			const newData = [...gradeCompositions];
			const index = newData.findIndex((item) => key === item.id);
			if (index > -1) {
				const item = newData[index];

				newData.splice(index, 1, {
					...item,
					...row,
				});
				if (!isGradeScaleSumValid(newData)) {
					message.error("GradeScale sum must be less than 100");
					return;
				}
				mutationUpdateGradeColumn.mutate(
					{
						...item,
						...row,
					},
					{
						onSuccess() {
							message.success("Update Grade Composition successfully");
						},
						onError() {
							message.error("Update Grade Composition failed");
						},
					}
				);

				setGradeCompositions(newData);
				setEditingKey(0);
			} else {
				newData.push(row);
				setGradeCompositions(newData);
				setEditingKey(0);
			}
		} catch (errInfo) {
			console.log("Validate Failed:", errInfo);
		}
	};

	const handleDelete = async (gradeId: number) => {
		mutationDeleteGradeColumn.mutate(gradeId);
		message.success("Delete Grade Composition successfully");
	};

	const handleConfirm = async (gradeId: number) => {
		mutationApproveGradeColumn.mutate(gradeId);
		const index = gradeCompositions.findIndex((item) => item.id === gradeId);
		if (gradeCompositions[index].isFinal == false) {
			message.success("Set Grade Composition final successfully");
		} else {
			message.success("Set Grade Composition not final successfully");
		}
	};

	const columns = [
		{
			title: "Name",
			dataIndex: "name",
			width: "40%",
			editable: true,
		},
		{
			title: "GradeScale",
			dataIndex: "gradeScale",
			width: "40%",
			editable: true,
		},
		{
			title: "",
			width: "15%",
			align: "center",
			dataIndex: "operation",
			render: (_: any, record: gradeCompositions) => {
				const editable = isEditing(record);
				return editable ? (
					<span>
						<Typography.Link
							onClick={() => save(record.id)}
							style={{ marginRight: 8 }}
						>
							Save
						</Typography.Link>
						<Popconfirm
							title="Sure to cancel?"
							onConfirm={cancel}
							okButtonProps={{ className: "bg-blue-500" }}
						>
							<a>Cancel</a>
						</Popconfirm>
					</span>
				) : (
					<Space className="gap-x-5">
						<Typography.Link
							disabled={editingKey !== 0}
							onClick={() => edit(record)}
						>
							<Tooltip placement="bottom" title="Edit" arrow>
								<EditOutlined className="text-xl" />
							</Tooltip>
						</Typography.Link>
						<Popconfirm
							title="Sure to delete?"
							onConfirm={() => handleDelete(record.id)}
							okButtonProps={{ className: "bg-blue-500" }}
						>
							<Tooltip placement="bottom" title="Delete grade struct" arrow>
								<a className="text-red-500 hover:text-red-600">
									<DeleteOutlined className="text-xl" />
								</a>
							</Tooltip>
						</Popconfirm>
						{record && record.isFinal == false ? (
							<Popconfirm
								title="Sure to confirm?"
								onConfirm={() => handleConfirm(record.id)}
								okButtonProps={{ className: "bg-blue-500" }}
							>
								<Tooltip placement="bottom" title="Mark as final" arrow>
									<a className="text-green-500 hover:text-green-600">
										<CheckCircleOutlined className="text-xl" />
									</a>
								</Tooltip>
							</Popconfirm>
						) : (
							<Popconfirm
								title="Sure no confirm?"
								onConfirm={() => handleConfirm(record.id)}
								okButtonProps={{ className: "bg-blue-500" }}
							>
								<Tooltip placement="bottom" title="Mark as not final" arrow>
									<a className="text-red-500 hover:text-red-600">
										<CheckCircleOutlined className="text-xl" />
									</a>
								</Tooltip>
							</Popconfirm>
						)}
					</Space>
				);
			},
		},
	];

	const mergedColumns = columns?.map((col) => {
		if (!col.editable) {
			return col;
		}
		return {
			...col,
			onCell: (record: gradeCompositions) => {
				return {
					record,
					inputType: col.dataIndex === "age" ? "number" : "text",
					dataIndex: col.dataIndex,
					title: col.title,
					editing: isEditing(record),
				};
			},
		};
	});

	return (
		<div className="w-full p-6">
			<div className="row grid justify-items-end">
				<Button
					onClick={handleAdd}
					ghost
					type="primary"
					style={{ marginBottom: 16 }}
				>
					Add a grade structure
				</Button>
			</div>
			<DndContext
				sensors={sensors}
				modifiers={[restrictToVerticalAxis]}
				onDragEnd={onDragEnd}
			>
				<SortableContext
					// rowKey array
					items={gradeCompositions?.map((i) => i.id) ?? []}
					strategy={verticalListSortingStrategy}
				>
					<Form form={form} component={false}>
						<Table
							className="[&_tr]:!z-0"
							components={{
								body: {
									cell: EditableCell,
									row: RowDragable,
								},
							}}
							bordered
							dataSource={gradeCompositions}
							columns={mergedColumns as any}
							rowClassName="editable-row"
							pagination={false}
						/>
					</Form>
				</SortableContext>
			</DndContext>
		</div>
	);
};

export default GradeStructure;

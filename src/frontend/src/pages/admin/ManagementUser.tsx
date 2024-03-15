import {
	ArrowUpOutlined,
	ImportOutlined,
	LockOutlined,
	MoreOutlined,
	SearchOutlined,
	UnlockOutlined,
	UploadOutlined,
} from "@ant-design/icons";
import React, { useRef, useState, useEffect } from "react";
import Highlighter from "react-highlight-words";
import type { InputRef, MenuProps } from "antd";
import {
	Button,
	Dropdown,
	FloatButton,
	Input,
	Menu,
	Space,
	Switch,
	Table,
	Tag,
	message,
} from "antd";
import type { ColumnType, ColumnsType } from "antd/es/table";
import type { FilterConfirmProps } from "antd/es/table/interface";
import {
	classQueryWithoutParams,
	userQueryResult,
} from "../../api/store/admin/queries";
import AdminEditUser from "../../modal/admin/AdminEditUser";
import moment from "moment";
import {
	lockUserMutation,
	unlockUserMutation,
} from "../../api/store/class/mutation";
import UploadModal from "../../modal/UploadModal";
import UploadStudentModal from "../../modal/UploadStudentModel";

interface DataType {
	key: number;
	fullName: string;
	role: string;
	status: string;
	lockoutEnd: string;
	firstName: string;
	lastName: string;
	studentId: string;
	email: string;
}

type DataIndex = keyof DataType;

const ManagementUser: React.FC = () => {
	const [searchText, setSearchText] = useState("");
	const [searchedColumn, setSearchedColumn] = useState("");
	const [isModalVisible, setIsModalVisible] = useState(false);
	const [classId, setClassId] = useState("");
	const searchInput = useRef<InputRef>(null);
	const { data: listData, isLoading } = userQueryResult();
	const mutation = lockUserMutation();
	const mutationUnBan = unlockUserMutation();
	const [formData, setFormData] = useState({
		key: 0,
		fullName: "",
		role: "",
		lockoutEnd: "",
		firstName: "",
		lastName: "",
		studentId: "",
		email: "",
	});

	const handleSearch = (
		selectedKeys: string[],
		confirm: (param?: FilterConfirmProps) => void,
		dataIndex: DataIndex
	) => {
		confirm();
		setSearchText(selectedKeys[0]);
		setSearchedColumn(dataIndex);
	};

	const getColumnSearchProps = (
		dataIndex: DataIndex
	): ColumnType<DataType> => ({
		filterDropdown: ({
			setSelectedKeys,
			selectedKeys,
			confirm,
			clearFilters,
			close,
		}) => (
			<div style={{ padding: 8 }} onKeyDown={(e) => e.stopPropagation()}>
				<Input
					ref={searchInput}
					placeholder={`Search ${dataIndex}`}
					value={selectedKeys[0]}
					onChange={(e) =>
						setSelectedKeys(e.target.value ? [e.target.value] : [])
					}
					onPressEnter={() =>
						handleSearch(selectedKeys as string[], confirm, dataIndex)
					}
					style={{ marginBottom: 8, display: "block" }}
				/>
				<div className="grid justify-items-end">
					<Button
						type="link"
						size="small"
						onClick={() => {
							close();
						}}
					>
						close
					</Button>
				</div>
			</div>
		),
		filterIcon: (filtered: boolean) => (
			<SearchOutlined style={{ color: filtered ? "#1677ff" : undefined }} />
		),
		onFilter: (value, record) =>
			record[dataIndex]
				.toString()
				.toLowerCase()
				.includes((value as string).toLowerCase()),
		onFilterDropdownOpenChange: (visible) => {
			if (visible) {
				setTimeout(() => searchInput.current?.select(), 100);
			}
		},
		sorter: (a, b) => a.fullName.length - b.fullName.length,
		render: (text) =>
			searchedColumn === dataIndex ? (
				<Highlighter
					highlightStyle={{ backgroundColor: "#ffc069", padding: 0 }}
					searchWords={[searchText]}
					autoEscape
					textToHighlight={text ? text.toString() : ""}
				/>
			) : (
				text
			),
	});

	const data = listData?.map((item) => ({
		key: item.id,
		fullName: item.fullName,
		email: item.email,
		lastName: item.lastName,
		firstName: item.firstName,
		studentId: item.studentId,
		role: item.role,
		status: item.status,
		lockoutEnd: item.lockoutEnd,
	}));

	const columns: ColumnsType<DataType> = [
		{
			title: "Full Name",
			dataIndex: "fullName",
			key: "fullName",
			width: "25%",
			...getColumnSearchProps("fullName"),
			render: (text, record) => {
				return (
					<div
						className="text-blue-600 hover:cursor-pointer justify-center rounded-md"
						onClick={() => {
							setIsModalVisible(true);
							setFormData({
								key: record.key,
								fullName: record.fullName,
								role: record.role,
								firstName: record.firstName,
								lastName: record.lastName,
								studentId: record.studentId,
								email: record.email,
								lockoutEnd: record.lockoutEnd,
							});
						}}
					>
						{record.fullName}
					</div>
				);
			},
		},

		{
			title: "Role",
			dataIndex: "role",
			key: "role",
			width: "15%",
			onFilter: (value, record) => record.role === value,
			filters: [
				{
					text: "Teacher",
					value: "Teacher",
				},
				{
					text: "Student",
					value: "Student",
				},
				{
					text: "Admin",
					value: "Admin",
				},
			],
			filterSearch: true,
			render: (text, record) => {
				return (
					<div>
						{record.role === "Admin" ? (
							<Tag color="cyan">{record.role}</Tag>
						) : record.role === "Teacher" ? (
							<Tag color="orange">{record.role}</Tag>
						) : (
							<Tag color="green">{record.role}</Tag>
						)}
					</div>
				);
			},
		},
		{
			title: "Status",
			dataIndex: "status",
			key: "status",
			width: "15%",
			onFilter: (value, record) => record.status === value,
			filters: [
				{
					text: "Locked",
					value: "Locked",
				},
				{
					text: "Active",
					value: "Active",
				},
				{
					text: "Banned",
					value: "Banned",
				},
			],
			filterSearch: true,
			render: (text, record) => {
				return (
					<div>
						{record.status === "Locked" ? (
							<Space>
								<Tag color="red">{record.status}</Tag>
								<div className="text-red-500">
									Unlocked on the day-{" "}
									{moment(record.lockoutEnd).format("DD/MM/YYYY")}
								</div>
							</Space>
						) : record.status === "Banned" ? (
							<Tag color="orange">{record.status}</Tag>
						) : (
							<Tag color="green">{record.status}</Tag>
						)}
					</div>
				);
			},
		},
		{
			title: "",
			key: "action",
			width: "1%",
			render: (text, record) => {
				return (
					<div className="flex justify-center">
						<Dropdown
							overlay={
								<Menu>
									{record.status === "Active" ? (
										<Menu.Item
											key="1"
											icon={<LockOutlined />}
											onClick={() => {
												mutation.mutate(record.key, {
													onSuccess() {
														message.success("Ban successfully");
													},
													onError(error: any) {
														console.error(error);

														message.error(error.response.data.title);
													},
												});
											}}
										>
											Ban
										</Menu.Item>
									) : (
										<Menu.Item
											key="2"
											icon={<UnlockOutlined />}
											onClick={() => {
												mutationUnBan.mutate(record.key, {
													onSuccess() {
														message.success("Unban successfully");
													},
													onError(error: any) {
														console.error(error);

														message.error(error.response.data.title);
													},
												});
											}}
										>
											Unban
										</Menu.Item>
									)}
								</Menu>
							}
							trigger={["click"]}
						>
							<Button
								type="link"
								icon={<MoreOutlined />}
								onClick={(e) => e.preventDefault()}
							/>
						</Dropdown>
					</div>
				);
			},
		},
	];

	const [isOpenImportStudent, setIsImportStudent] = useState(false);
	const onChange = (checked: boolean) => {
		setIsImportStudent(checked);
	};
	const handleCloseModal = () => {
		setIsImportStudent(false);
	};
	if (isLoading) return <div>loading...</div>;

	return (
		<div className="p-6">
			<FloatButton.Group
				trigger="hover"
				style={{ right: 24 }}
				icon={<ArrowUpOutlined />}
			>
				<FloatButton icon={<ImportOutlined />} onClick={() => onChange(true)} />
			</FloatButton.Group>

			<Table
				bordered
				title={() => <div>User Table</div>}
				columns={columns}
				dataSource={data}
			/>

			{isModalVisible && (
				<AdminEditUser
					openModal={isModalVisible}
					handleCancel={() => {
						setIsModalVisible(false);
						// setClassId("");
					}}
					data={formData}
				/>
			)}

			{isOpenImportStudent && (
				<UploadStudentModal
					openModal={isOpenImportStudent}
					handleCloseModalUpload={handleCloseModal}
				/>
			)}
		</div>
	);
};

export default ManagementUser;

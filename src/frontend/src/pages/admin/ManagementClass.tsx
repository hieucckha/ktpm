import { SearchOutlined } from "@ant-design/icons";
import React, { useRef, useState, useEffect } from "react";
import Highlighter from "react-highlight-words";
import type { InputRef } from "antd";
import { Button, Input, Space, Spin, Table } from "antd";
import type { ColumnType, ColumnsType } from "antd/es/table";
import type { FilterConfirmProps } from "antd/es/table/interface";
import { classQueryWithoutParams } from "../../api/store/admin/queries";
import { Link } from "react-router-dom";
import EditClass from "../../modal/EditClassModal";

interface DataType {
	key: number;
	name: string;
	creatorName: string;
	isActivated: string;
	numberOfStudents: number;
	numberOfTeachers: number;
}

type DataIndex = keyof DataType;

const ManagementClass: React.FC = () => {
	const [searchText, setSearchText] = useState("");
	const [searchedColumn, setSearchedColumn] = useState("");
	const [isModalVisible, setIsModalVisible] = useState(false);
	const [classId, setClassId] = useState("");
	const searchInput = useRef<InputRef>(null);
	const { data: listData, isLoading } = classQueryWithoutParams();

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
		sorter: (a, b) => a.name.length - b.name.length,
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
		name: item.name,
		creatorName: item.creatorName,
		isActivated: item.isActivated ? "Active" : "Inactive",
		numberOfStudents: item.numberOfStudents,
		numberOfTeachers: item.numberOfTeachers,
	}));

	const columns: ColumnsType<DataType> = [
		{
			title: "Name",
			dataIndex: "name",
			key: "name",
			width: "25%",
			...getColumnSearchProps("name"),
			render: (text, record) => {
				return (
					<div
						className="text-blue-600 hover:cursor-pointer justify-center rounded-md"
						onClick={() => {
							setIsModalVisible(true);
							setClassId(record.key as unknown as string);
						}}
					>
						{record.name}
					</div>
				);
			},
		},
		{
			title: "Creator Name",
			dataIndex: "creatorName",
			key: "creatorName",
			width: "20%",
			...getColumnSearchProps("creatorName"),
		},
		{
			title: "Activate",
			dataIndex: "isActivated",
			key: "isActivated",
			width: "15%",
			onFilter: (value, record) => record.isActivated === value,
			filters: [
				{
					text: "Active",
					value: "Active",
				},
				{
					text: "Inactive",
					value: "Inactive",
				},
			],
			filterSearch: true,
			render: (text, record) => {
				return (
					<div>
						{record.isActivated === "Active" ? (
							<div className="text-green-600 justify-center rounded-md">
								{record.isActivated}
							</div>
						) : (
							<div className="text-red-600 justify-center rounded-md">
								{record.isActivated}
							</div>
						)}
					</div>
				);
			},
		},
		{
			title: "Number Of Students",
			dataIndex: "numberOfStudents",
			key: "numberOfStudents",
			width: "20%",
		},
		{
			title: "Number Of Teachers",
			dataIndex: "numberOfTeachers",
			key: "numberOfTeachers",
			width: "20%",
		},
	];

	if (isLoading) return  (
				<div className="w-full h-full flex justify-center items-center">
					<Spin />
				</div>
			);

	return (
		<div className="p-6">
			<Table bordered title={() => <div>Class Table</div>} columns={columns} dataSource={data} />
			{isModalVisible && (
				<EditClass
					openModal={isModalVisible}
					handleCloseModalEditClass={() => {
						setIsModalVisible(false);
						setClassId(""); 
					}}
					classId={classId}
				/>
			)}
		</div>
	);
};

export default ManagementClass;

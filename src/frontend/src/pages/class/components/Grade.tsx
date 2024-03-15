import { FC, useEffect, useState } from "react";

import { FloatButton, Table, TableProps } from "antd";
import { useParams } from "react-router-dom";
import {
	listGradeAllClassQuery,
	listGradeOneStudentQuery,
} from "../../../api/store/class/queries";
import { EditOutlined, PlusOutlined } from "@ant-design/icons";
import useAuth from "../../../hooks/auth";
import AddGrade from "../../../modal/AddGrade";
import CreateRequest from "../../../modal/CreateRequest";
import { UserRole } from "../../../api/store/auth/interface";

const Grade: FC = () => {
	const { id } = useParams();
	const [gradeColumn, setGradeColumn] = useState<any[]>([]);
	const [isOpenModalAddGrade, setIsOpenModalAddGrade] = useState(false);
	const [isOpenModalRequestGrade, setIsOpenModalRequestGrade] = useState(false);
	const { data: user } = useAuth();

	const { data: gradeData } =
		user?.role === UserRole.Teacher
			? listGradeAllClassQuery(id as string)
			: listGradeOneStudentQuery(id as string, user?.studentId ?? "");
	const [student, setStudent] = useState<any[]>([]);

	useEffect(() => {
		if (gradeData) {
			let gradeComposition = gradeData?.gradeCompositionDtos;

			if (user?.role === UserRole.Student) {
				gradeComposition = gradeComposition.filter((item) => item.isFinal);
			}

			setGradeColumn([
				{ title: "Full Name", dataIndex: "name", key: "name" },
				{ title: "Student ID", dataIndex: "studentID", key: "studentID" },
				...gradeComposition
					?.sort((a, b) => a.order - b.order)
					.map((item) => ({
						title: `${item.name} (${
							item.gradeScale && item.gradeScale.toString()
						}%)`,
						dataIndex: item.id,
						key: item.id,
					})),
				{
					title: "Total",
					dataIndex: "total",
					key: "total",
					render: (value, record) => {
						let total = 0;
						gradeComposition.forEach((item) => {
							total += record[item.id] * (item.gradeScale / 100);
						});
						return total;
					},
				},
			]);

			setStudent([
				...gradeData?.students?.map((item) => {
					let temp = {
						key: item.studentId,
						name: item.studentName,
						studentID: item.studentId,
					};
					item?.gradeDto?.forEach((grade) => {
						temp[grade.gradeCompositionId] = grade.gradeValue;
					});
					return temp;
				}),
			]);
		}
	}, [gradeData]);

	const handleOpenModalAddGrade = () => {
		setIsOpenModalAddGrade(true);
	};

	const tableProps: TableProps<any> =
		user?.role === UserRole.Teacher
			? {
					pagination: {
						hideOnSinglePage: true,
						showLessItems: true,
						showSizeChanger: true,
						position: ["bottomRight"],
						showQuickJumper: true,
						showPrevNextJumpers: true,
						defaultPageSize: 10,
						defaultCurrent: 1,
						showTotal: (total, range) => {
							return `${range[0]}-${range[1]} of ${total} students`;
						},
					},
					summary: (data) => (
						<Table.Summary.Row>
							<Table.Summary.Cell index={0} colSpan={2} align="center">
								Average
							</Table.Summary.Cell>
							{gradeData?.gradeCompositionDtos?.map((item) => {
								return (
									<Table.Summary.Cell key={item.id} index={item.order + 1}>
										{parseFloat(
											(
												data.reduce((sum, record) => {
													return sum + record[item.id];
												}, 0) / data.length
											).toFixed(2)
										)}
									</Table.Summary.Cell>
								);
							})}
						</Table.Summary.Row>
					),
			  }
			: {
					pagination: false,
			  };

	return (
		<>
			<div className="w-full p-6">
				<Table
					bordered
					columns={gradeColumn}
					dataSource={student}
					{...tableProps}
				/>
			</div>
			{user?.role === "Teacher" ? (
				<FloatButton
					onClick={handleOpenModalAddGrade}
					type="primary"
					style={{ right: 24 }}
					icon={<EditOutlined />}
				/>
			) : (
				<FloatButton
					onClick={() => setIsOpenModalRequestGrade(true)}
					type="primary"
					style={{ right: 24 }}
					icon={<PlusOutlined />}
				/>
			)}
			{isOpenModalAddGrade && (
				<AddGrade
					openModal={isOpenModalAddGrade}
					handleCloseModalAddGrade={() => setIsOpenModalAddGrade(false)}
				/>
			)}
			{isOpenModalRequestGrade && (
				<CreateRequest
					open={isOpenModalRequestGrade}
					handleClose={() => setIsOpenModalRequestGrade(false)}
				/>
			)}
		</>
	);
};

export default Grade;

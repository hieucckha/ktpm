import moment from "moment";
import {
	RequestDto,
	RequestStatus,
} from "../../../../api/store/class/interface";
import { App, Button, Popconfirm, Popover, Space, Tooltip } from "antd";
import {
	CheckOutlined,
	CloseOutlined,
	QuestionCircleOutlined,
} from "@ant-design/icons";
import { useState } from "react";
import ApproveRequestForm from "./ApproveRequestForm";
import { useParams } from "react-router-dom";
import { useRejectRequest } from "../../../../api/store/request/mutation";
import RequestCommentModal from "../modal/RequestCommentModal";
import { useQueryClient } from "@tanstack/react-query";

interface RequestCardProps extends RequestDto {
	isTeacherView?: boolean;
}

function RequestCard({
	id,
	studentId,
	studentName,
	status,
	gradeId,
	gradeName,
	currentGrade,
	expectedGrade,
	reason,
	updatedAt,
	createdAt,
	isTeacherView = false,
}: RequestCardProps) {
	const queryClient = useQueryClient();
	const { message } = App.useApp();
	const [openApprovePopover, setOpenApprovePopover] = useState(false);
	const [openCommentModal, setOpenCommentModal] = useState(false);
	const { id: classId } = useParams();
	const { mutate: rejectReqMutate } = useRejectRequest(classId);

	if (!classId) return null;

	const hide = () => {
		setOpenApprovePopover(false);
	};

	const handleOpenChange = (newOpen: boolean) => {
		setOpenApprovePopover(newOpen);
	};

	const handleRejectRequest = () =>
		new Promise((resolve, reject) => {
			rejectReqMutate(id, {
				onSuccess() {
					message.success("Rejected successfully");
					queryClient.invalidateQueries({ queryKey: ["request", id] });
					resolve(null);
				},
				onError() {
					message.error("Rejected failed");
					reject();
				},
			});
		});

	const getTextColorClass = (status: RequestStatus) => {
		switch (status) {
			case RequestStatus.Pending:
				return "text-blue-500";
			case RequestStatus.Approved:
				return "text-green-500";
			case RequestStatus.Rejected:
				return "text-red-500";
		}
	};

	const handleOpenCommentModal = () => {
		setOpenCommentModal(true);
	};

	return (
		<>
			<div
				id={id.toString()}
				className="flex flex-col justify-between cursor-pointer bg-slate-200 rounded hover:opacity-80 hover:shadow-lg p-3 gap-x-5"
				onClick={(e) => {
					e.stopPropagation();
					handleOpenCommentModal();
				}}
			>
				<div className="grid grid-cols-4 gap-x-2">
					{/* ACTIONS */}
					{isTeacherView && status === RequestStatus.Pending && (
						<div className="absolute right-10">
							<Space>
								<Tooltip title="Approve Request" placement="bottomRight">
									<div
										onClick={(e) => {
											e.stopPropagation();
										}}
									>
										<Popover
											arrow
											content={
												<ApproveRequestForm
													reqId={id.toString()}
													classId={classId}
													gradeValue={currentGrade}
													onClose={hide}
												/>
											}
											title="Approve Request"
											trigger="click"
											destroyTooltipOnHide
											open={openApprovePopover}
											onOpenChange={(open) => {
												handleOpenChange(open);
											}}
											placement="topRight"
										>
											<Button
												className="bg-green-500 hover:!bg-green-600"
												type="primary"
												shape="circle"
												size="small"
												icon={<CheckOutlined />}
											/>
										</Popover>
									</div>
								</Tooltip>
								<Tooltip title="Reject Request" placement="bottomRight">
									<div
										onClick={(e) => {
											e.stopPropagation();
										}}
									>
										<Popconfirm
											title="Reject request"
											description="Are you sure to reject this request?"
											okText="Yes"
											cancelText="No"
											okType="danger"
											placement="bottomRight"
											onConfirm={handleRejectRequest}
											icon={<QuestionCircleOutlined style={{ color: "red" }} />}
										>
											<Button
												className="bg-red-500 hover:!bg-red-600"
												type="primary"
												shape="circle"
												size="small"
												icon={<CloseOutlined />}
											/>
										</Popconfirm>
									</div>
								</Tooltip>
							</Space>
						</div>
					)}
					{/* ------------ */}
					<div>
						<strong>Student ID:</strong> {studentId}
					</div>
					{studentName && (
						<div>
							<strong>Student Name:</strong> {studentName}
						</div>
					)}
					<div>
						<strong>Grade Column: </strong> {gradeName}
					</div>
					<div className="flex gap-x-1">
						<strong>Status: </strong>
						<span className={getTextColorClass(status)}>
							<strong>{status}</strong>
						</span>
					</div>
				</div>
				<div className="grid grid-cols-4  gap-x-2">
					<div className="col-span-2">
						<strong>Current Grade: </strong>
						{currentGrade}
					</div>
					<div className="col-span-2">
						<strong>Expected Grade: </strong>
						{expectedGrade}
					</div>
				</div>
				<div className="grid grid-cols-4 gap-x-2">
					<div className="col-span-2">
						<strong>Reason: </strong>
						{reason}
					</div>
					<div className="col-span-2">
						<strong>Request Day: </strong>
						{moment(createdAt).format("DD/MM/YYYY")}
					</div>
				</div>
			</div>
			<RequestCommentModal
				reqId={id.toString()}
				classId={classId}
				open={openCommentModal}
				onClose={() => setOpenCommentModal(false)}
			/>
		</>
	);
}

export default RequestCard;

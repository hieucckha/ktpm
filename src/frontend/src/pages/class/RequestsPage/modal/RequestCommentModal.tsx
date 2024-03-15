import {
	Badge,
	Button,
	Empty,
	List,
	Modal,
	ModalProps,
	Space,
	Spin,
} from "antd";
import { useGetRequest } from "../../../../api/store/request/queries";
import UnexpectedError from "../../../errors/UnexpectedError";
import { RequestStatus } from "../../../../api/store/class/interface";
import ReactQuill from "react-quill";
import "react-quill/dist/quill.snow.css";
import { useRef, useState } from "react";
import { SendOutlined } from "@ant-design/icons";
import { useAddRequestComment } from "../../../../api/store/request/mutation";
import useAuthQuery from "../../../../api/store/auth/queries";

interface RequestCommentModalProps extends Pick<ModalProps, "open" | "title"> {
	reqId: string;
	classId: string;
	onClose: () => void;
}

function RequestCommentModal({
	title,
	open,
	onClose,
	reqId,
	classId,
}: RequestCommentModalProps) {
	const quillRef = useRef<ReactQuill>(null);
	const [comment, setComment] = useState("");
	const { data: myId } = useAuthQuery((state) => state.id);
	const { data, isLoading, isError, isSuccess, error } = useGetRequest(
		reqId.toString(),
		(data) => ({
			comments: data.comments,
			status: data.status,
		})
	);
	const { mutate: addComment, isPending } = useAddRequestComment(reqId);

	const handleCancel = () => {
		onClose();
		setComment("");
	};

	const handleAddComment = () => {
		addComment(
			{
				comment: comment.trim(),
				req_id: reqId,
			},
			{
				onSuccess() {
					setComment("");
				},
				onError() {},
			}
		);
	};

	return (
		<Modal
			className="my-4"
			destroyOnClose
			centered
			title={
				title || (
					<Space>
						<span>Request Comments</span>
						<Badge
							title="Number of comments"
							count={data?.comments.length ?? 0}
							style={{ backgroundColor: "#52c41a" }}
						/>
					</Space>
				)
			}
			open={open}
			onCancel={handleCancel}
			footer={null}
		>
			{isError && <UnexpectedError error={error} />}
			{isLoading && (
				<div className="w-full flex justify-center m-4">
					<Spin />
				</div>
			)}

			{isSuccess && data.comments.length === 0 && (
				<div className="w-full flex justify-center m-4">
					<Empty />
				</div>
			)}
			{isSuccess && data.comments.length !== 0 && (
				<List
					className="mb-4"
					itemLayout="horizontal"
					dataSource={data.comments}
					renderItem={(item, index) => (
						<List.Item key={item.id}>
							<List.Item.Meta
								className={
									item.userId.toString() === myId.toString()
										? "text-right"
										: "text-left"
								}
								title={
									item.userId.toString() === myId.toString() ? (
										<Badge.Ribbon
											text={"You"}
											color={"green"}
											placement={"start"}
										>
											<span>{item.name}</span>
										</Badge.Ribbon>
									) : (
										<span>{item.name}</span>
									)
								}
								description={
									<div dangerouslySetInnerHTML={{ __html: item.comment }}></div>
								}
							/>
						</List.Item>
					)}
					pagination={{
						position: "bottom",
						pageSize: 5,
						hideOnSinglePage: true,
						align: "end",
					}}
				/>
			)}
			{isSuccess && data.status === RequestStatus.Pending && (
				<div className="flex flex-row gap-x-2">
					<ReactQuill
						ref={quillRef}
						className="h-full flex-1"
						theme="snow"
						modules={{
							toolbar: [
								["bold", "italic", "underline", "strike", "blockquote"],
								[
									{ list: "ordered" },
									{ list: "bullet" },
									{ indent: "-1" },
									{ indent: "+1" },
								],
								["link"],
								["clean"],
							],
						}}
						value={comment}
						onChange={setComment}
						placeholder="Write your comment"
					/>
					<Button
						disabled={
							quillRef.current
								? quillRef.current?.getEditor().getText().trim() === ""
								: true
						}
						className="mb-1 border-none self-end w-[32px] h-[32px] flex justify-center items-center rounded-full disabled:text-inherit disabled:bg-inherit disabled:opacity-100 disabled:cursor-not-allowed shadow-none text-sky-500 hover:text-sky-500 hover:bg-gray-300 hover:opacity-70"
						type="primary"
						shape="circle"
						icon={<SendOutlined />}
						onClick={handleAddComment}
						loading={isPending}
					/>
				</div>
			)}
		</Modal>
	);
}

export default RequestCommentModal;

import { FC } from "react";
import moment from "moment";
import { App, Badge, Spin, Tooltip } from "antd";
import useClassDetail from "../../../hooks/useClassDetail";
import { BellOutlined, BellTwoTone } from "@ant-design/icons";
import { useNavigate, useParams } from "react-router-dom";
import { RequestStatus } from "../../../api/store/class/interface";

const Overview: FC = () => {
	const { message } = App.useApp();
	const { id: classId } = useParams();
	const { data, isLoading } = useClassDetail();
	const navigate = useNavigate();

	if (!data) return null;
	const copylink = () => {
		navigator.clipboard.writeText(data?.inviteLink);
		message.success("Copied link to clipboard");
	};
	const copyCode = () => {
		navigator.clipboard.writeText(data?.inviteCode);
		message.success("Copied code to clipboard");
	};
	return isLoading ? (
		<div>
			<Spin fullscreen />
		</div>
	) : (
		<div className="h-screen w-full flex justify-center bg-slate-50 pt-6">
			<div className="w-4/5">
				<img
					className="rounded-t-lg mb-5 w-full"
					src="https://gstatic.com/classroom/themes/img_reachout.jpg"
					alt="Header"
				/>
				<div className="flex flex-col md:flex-row gap-y-4 md:gap-x-4 w-full">
					<a className="flex h-40 gap-3  p-6 bg-white border border-gray-200 rounded-lg shadow  dark:bg-gray-800 dark:border-gray-700  flex-shrink-0">
						<div className="flex flex-col">
							<div className="flex flex-row justify-between">
								<p className="mb-2 font-bold pt-2  tracking-tight text-gray-900 dark:text-white pb-4">
									Class code:{" "}
								</p>
								<Tooltip placement="bottom" title="Copy class code" arrow>
									<svg
										onClick={copyCode}
										className="cursor-pointer"
										xmlns="http://www.w3.org/2000/svg"
										width="24"
										height="24"
										viewBox="0 0 24 24"
										fill="none"
										stroke="#000000"
										strokeWidth="2"
										strokeLinecap="round"
										strokeLinejoin="round"
									>
										<rect
											x="9"
											y="9"
											width="13"
											height="13"
											rx="2"
											ry="2"
										></rect>
										<path d="M5 15H4a2 2 0 0 1-2-2V4a2 2 0 0 1 2-2h9a2 2 0 0 1 2 2v1"></path>
									</svg>
								</Tooltip>
							</div>
							<p className="font-normal flex-end  text-gray-700 dark:text-gray-400 block">
								{data?.inviteCode}
							</p>
							<div className="flex flex-row mt-3 justify-between">
								<p className="mb-2 font-bold pt-2  tracking-tight text-gray-900 dark:text-white pb-4">
									Invite:{" "}
								</p>
								<Tooltip placement="bottom" title="Copy link invite" arrow>
									<svg
										onClick={copylink}
										xmlns="http://www.w3.org/2000/svg"
										className="cursor-pointer hover:to-blue-600"
										width="24"
										height="24"
										viewBox="0 0 24 24"
										fill="none"
										stroke="#000000"
										strokeWidth="2"
										strokeLinecap="round"
										strokeLinejoin="round"
									>
										<path d="M10 13a5 5 0 0 0 7.54.54l3-3a5 5 0 0 0-7.07-7.07l-1.72 1.71"></path>
										<path d="M14 11a5 5 0 0 0-7.54-.54l-3 3a5 5 0 0 0 7.07 7.07l1.71-1.71"></path>
									</svg>
								</Tooltip>
							</div>
						</div>
					</a>
					<div className="flex w-full flex-row justify-between">
						<div className="flex-row w-full ">
							<div className="p-6 w-full bg-white border mb-3 border-gray-200 rounded-lg shadow dark:bg-gray-800 dark:border-gray-700">
								<a href="#">
									<h5 className="mb-2 text-2xl font-semibold tracking-tight text-gray-900 dark:text-white">
										{data?.name}
									</h5>
								</a>
								<p className="mb-3 font-normal text-gray-500 dark:text-gray-400">
									{data?.description}
								</p>
							</div>

							{data.requests && (
								<div className="flex flex-col w-full">
									<span className="text-xl font-bold my-4">
										Grade Review Request List
									</span>
									{data.requests.map((request) => (
										<Badge.Ribbon
											key={request.id}
											text={request.status}
											color={
												request.status === RequestStatus.Pending
													? "cyan"
													: request.status === RequestStatus.Approved
													  ? "green"
													  : "red"
											}
										>
											<div
												className="p-6 hover:cursor-pointer w-full bg-white border mb-3 border-gray-200 rounded-lg shadow hover:drop-shadow-md dark:bg-gray-800 dark:border-gray-700"
												onClick={() =>
													navigate(`/class/${classId}/requests#${request.id}`)
												}
											>
												<button className="w-full flex items-center flex-row gap-x-3 ">
													<BellTwoTone className="text-2xl" />
													<div className="flex flex-col justify-between leading-normal gap-y-1 text-left">
														<span className="font-bold tracking-tight text-gray-900 dark:text-white">
															{request.studentName} has request:{" "}
															{request.reason}{" "}
														</span>
														<span className="font-normal text-gray-700 dark:text-gray-400">
															{moment(request.createdAt).format("DD/MM/YYYY")}
														</span>
													</div>
												</button>
											</div>
										</Badge.Ribbon>
									))}
								</div>
							)}
						</div>
					</div>
				</div>
			</div>
		</div>
	);
};
export default Overview;

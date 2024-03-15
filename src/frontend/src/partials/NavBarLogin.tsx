/* eslint-disable @typescript-eslint/no-unsafe-assignment */
import { useState, type FC, useEffect, useContext } from "react";
import { Link, useNavigate } from "react-router-dom";

import localStorageService from "../services/localStorage.service";
import EditUser from "../modal/EditUser";
import useAuth from "../hooks/auth";
import CreateClass from "../modal/CreateClass";
import JoinClass from "../modal/JoinClass";
import { QueryClient } from "@tanstack/react-query";
import { UserRole } from "../api/store/auth/interface";
import { Avatar, Badge, Button, Divider, Dropdown, List, Tooltip } from "antd";
import { BellOutlined, NotificationOutlined } from "@ant-design/icons";
import notificationService from "../services/notification.service";
import { NotificationContext } from "../context/NotificationContext";
import moment from "moment";

/**
 * Navigation bar.
 */
const NavBarLogin: FC = () => {
	const queryClient = new QueryClient();
	const [isOpenModalEditUser, setIsOpenModalEditUser] = useState(false);
	const [isOpenModalCreateClass, setIsOpenModalCreateClass] = useState(false);
	const [isOpenModalJoinClass, setIsOpenModalJoinClass] = useState(false);
	const [profile, setProfile] = useState<{ name: string; email: string }>({
		name: "",
		email: "",
	});
	const navigate = useNavigate();
	const { notifications, setNotifications } = useContext(NotificationContext);

	const handleSignOut = async () => {
		const token = localStorageService.getItem("auth");
		await queryClient.clear();
		await queryClient.removeQueries();
		if (token !== null) {
			localStorageService.removeItem("auth");
			notificationService.clear();
			setNotifications([]);
		}
		navigate(user.role === UserRole.Admin ? "/admin/sign-in" : "/", {
			replace: true,
		});
	};
	const handleOpenModalEditUser = (): void => {
		setIsOpenModalEditUser(true);
	};
	const handleCloseModalEditUser = (): void => {
		setIsOpenModalEditUser(false);
	};

	const handleCloseModalCreateClass = (): void => {
		setIsOpenModalCreateClass(false);
	};
	const handleOpenModalCreateClass = (): void => {
		setIsOpenModalCreateClass(true);
	};
	const handleCloseModalJoinClass = (): void => {
		setIsOpenModalJoinClass(false);
	};
	const handleOpenModalJoinClass = (): void => {
		setIsOpenModalJoinClass(true);
	};
	const clearAll = () => {
		notificationService.clear();
		setNotifications([]);
	}
	const { data: user } = useAuth();

	useEffect(() => {
		if (!user) {
			return;
		}

		/**
		 * Get user profile.
		 */
		setProfile({
			name: user?.fullName ?? "",
			email: user?.email ?? "",
		});
	}, [user]);

	return (
		<nav className="fixed top-0 z-50 w-full bg-white border-b border-gray-200 dark:bg-gray-800 dark:border-gray-700">
			<div className="px-3 py-3 lg:px-5 lg:pl-3">
				<div className="flex items-center justify-between">
					<div className="flex items-center justify-start rtl:justify-end">
						<button
							type="button"
							className="inline-flex items-center p-2 text-sm text-gray-500 rounded-lg sm:hidden hover:bg-gray-100 focus:outline-none focus:ring-2 focus:ring-gray-200 dark:text-gray-400 dark:hover:bg-gray-700 dark:focus:ring-gray-600"
						>
							<span className="sr-only">Open sidebar</span>
							<svg
								className="w-6 h-6"
								aria-hidden="true"
								fill="currentColor"
								viewBox="0 0 20 20"
								xmlns="http://www.w3.org/2000/svg"
							>
								<path
									clipRule="evenodd"
									fillRule="evenodd"
									d="M2 4.75A.75.75 0 012.75 4h14.5a.75.75 0 010 1.5H2.75A.75.75 0 012 4.75zm0 10.5a.75.75 0 01.75-.75h7.5a.75.75 0 010 1.5h-7.5a.75.75 0 01-.75-.75zM2 10a.75.75 0 01.75-.75h14.5a.75.75 0 010 1.5H2.75A.75.75 0 012 10z"
								/>
							</svg>
						</button>
						<Link to={"/home"} className="flex ms-2 md:me-24">
							<img
								src="/icons/ssandwich.ico"
								className="h-8 me-3"
								alt="FlowBite Logo"
							/>
							<span className="self-center text-xl font-semibold sm:text-2xl whitespace-nowrap dark:text-white">
								Classroom
							</span>
						</Link>
					</div>
					<div className="flex items-center gap-x-2">
						<Dropdown
							dropdownRender={(menu) => (
								<div className="bg-slate-50 w-[400px] border border-solid rounded-xl drop-shadow-lg p-4">
									<div className="text-center">Notifications</div>
									<Divider className="my-2" />
									<List
										pagination={{ pageSize: 5, hideOnSinglePage: true }}
										dataSource={notifications}
										renderItem={(item: any, index) => {
											
											//conditions type of notification
											let renderContent = null;
											if(item.Type === 0){
												const link = `/class/${item.ClassId}/grade`;
												renderContent = <List.Item className="hover:bg-white">
													<List.Item.Meta
														avatar={
															<Avatar
																src={`https://xsgames.co/randomusers/avatar.php?g=pixel&key=${index}`}
															/>
														}
														title={<a href={link}>{item.Title}</a>}
														description={moment(item.Time).format("DD/MM/YYYY")}
													/>
													{item.Description}
												</List.Item>
											}
											else if(item.Type === 1){
												const link = `/class/${item.ClassId}/requests#${item.RequestId}`;
												renderContent = <List.Item className="hover:bg-white">
													<List.Item.Meta
														avatar={
															<Avatar
																src={`https://xsgames.co/randomusers/avatar.php?g=pixel&key=${index}`}
															/>
														}
														title={<a href={link}>{item.Title}</a>}
														description={moment(item.Time).format("DD/MM/YYYY")}
													/>
													{item.Description}
												</List.Item>
											}
											else if(item.Type === 2){
												const link = `/class/${item.ClassId}/requests#${item.RequestId}`;
												renderContent = <List.Item className="hover:bg-white">
													<List.Item.Meta
														avatar={
															<Avatar
																src={`https://xsgames.co/randomusers/avatar.php?g=pixel&key=${index}`}
															/>
														}
														title={<a href={link}>{item.Title}</a>}
														description={moment(item.Time).format("DD/MM/YYYY")}
													/>
													{item.Description}
												</List.Item>
											}
											else if(item.Type === 3){
												const link = `/class/${item.ClassId}/grade`;
												renderContent = <List.Item className="hover:bg-white">
													<List.Item.Meta
														avatar={
															<Avatar
																src={`https://xsgames.co/randomusers/avatar.php?g=pixel&key=${index}`}
															/>
														}
														title={<a href={link}>{item.Title}</a>}
														description={moment(item.Time).format("DD/MM/YYYY")}
													/>
													{item.Description}
												</List.Item>
											}
											else if (item.Type === 4){
												const link = `/class/${item.ClassId}/grade`;
												renderContent = <List.Item className="hover:bg-white">
													<List.Item.Meta
														avatar={
															<Avatar
																src={`https://xsgames.co/randomusers/avatar.php?g=pixel&key=${index}`}
															/>
														}
														title={<a href={link}>{item.Title}</a>}
														description={moment(item.Time).format("DD/MM/YYYY")}
													/>
													{item.Description}
												</List.Item>
											}

											return renderContent;


											// <List.Item className="hover:bg-white">
											// 	<List.Item.Meta
											// 		avatar={
											// 			<Avatar
											// 				src={`https://xsgames.co/randomusers/avatar.php?g=pixel&key=${index}`}
											// 			/>
											// 		}
											// 		title={<a href="https://ant.design">{item.Title}</a>}
											// 		description={moment(item.Time).format("DD/MM/YYYY")}

											// 	/>
											// 	{item.Description}
											// </List.Item>
											}}
									></List>
									<div className="text-center hover:cursor-pointer" onClick={clearAll}>Clear all</div>
								</div>
							)}
						>
							<Badge count={notifications.length}>
								<Button
									type="text"
									shape="circle"
									icon={<BellOutlined />}
									size="large"
								/>
							</Badge>
						</Dropdown>
						{user?.role === "Teacher" ? (
							<Tooltip placement="bottom" title="Add class" arrow>
								<button
									type="button"
									className="flex text-sm  rounded-full focus:bg-gray-200 hover:bg-gray-200 dark:hover:bg-gray-700"
									aria-expanded="false"
									onClick={handleOpenModalCreateClass}
								>
									<span className="sr-only">Create class</span>
									<svg
										focusable="false"
										width={24}
										height={24}
										viewBox="0 0 24 24"
										className="hover:ring-gray-200 dark:hover:ring-gray-300 w-8 h-8"
									>
										<path d="M20 13h-7v7h-2v-7H4v-2h7V4h2v7h7v2z" />
									</svg>
								</button>
							</Tooltip>
						) : (
							user?.role === "Student" && (
								<Tooltip placement="bottom" title="Join class" arrow>
									<button
										type="button"
										className="flex text-sm  rounded-full focus:bg-gray-200 hover:bg-gray-200 dark:hover:bg-gray-700"
										aria-expanded="false"
										onClick={handleOpenModalJoinClass}
									>
										<span className="sr-only">Join class</span>
										<svg
											focusable="false"
											width={24}
											height={24}
											viewBox="0 0 24 24"
											className="hover:ring-gray-200 dark:hover:ring-gray-300 w-8 h-8"
										>
											<path d="M20 13h-7v7h-2v-7H4v-2h7V4h2v7h7v2z" />
										</svg>
									</button>
								</Tooltip>
							)
						)}
						<Tooltip placement="bottom" title="Account" arrow>
							<Dropdown
								trigger={["click"]}
								menu={{ items: [] }}
								dropdownRender={(menu) => (
									<div className="z-50 my-4 text-base bg-white divide-y divide-gray-100 rounded  dark:bg-gray-700 dark:divide-gray-600 shadow-lg shadow-gray-500-100">
										<div className="px-4 py-3" role="none">
											<p
												className="text-sm text-gray-900 dark:text-white"
												role="none"
											>
												{profile.name}
											</p>
											<p
												className="text-sm font-medium text-gray-900 truncate dark:text-gray-300"
												role="none"
											>
												{profile.email}
											</p>
										</div>
										<ul className="py-1" role="none">
											{user.role !== UserRole.Admin && (
												<li>
													<button
														onClick={() => navigate("/home")}
														className="w-full text-left px-4 py-2 text-sm text-gray-700 hover:bg-gray-100 dark:text-gray-300 dark:hover:bg-gray-600 dark:hover:text-white"
														role="menuitem"
													>
														Dashboard
													</button>
												</li>
											)}
											<li className=" hover:bg-gray-100 dark:text-gray-300 dark:hover:bg-gray-600 dark:hover:text-white ">
												{/* <a href="#" className="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100 dark:text-gray-300 dark:hover:bg-gray-600 dark:hover:text-white" role="menuitem">Settings</a> */}
												<button
													onClick={handleOpenModalEditUser}
													className="w-full text-left block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100 dark:text-gray-300 dark:hover:bg-gray-600 dark:hover:text-white  "
													role="menuitem"
												>
													Settings
												</button>
											</li>
											<li>
												<button
													className="w-full text-left px-4 py-2 text-sm text-gray-700 hover:bg-gray-100 dark:text-gray-300 dark:hover:bg-gray-600 dark:hover:text-white"
													role="menuitem"
													onClick={handleSignOut}
												>
													Sign out
												</button>
											</li>
										</ul>
									</div>
								)}
							>
								<button
									type="button"
									className="flex text-sm bg-gray-800 rounded-full focus:ring-4 focus:ring-gray-300 dark:focus:ring-gray-600"
									aria-expanded="false"
									data-dropdown-toggle="dropdown-user"
								>
									<span className="sr-only">Open user menu</span>
									<img
										className="w-8 h-8 rounded-full"
										src="https://flowbite.com/docs/images/people/profile-picture-5.jpg"
										alt="user photo"
									/>
								</button>
							</Dropdown>
						</Tooltip>
						{/* <div className="flex items-center ms-3">
							<div>
								<button
									type="button"
									className="flex text-sm bg-gray-800 rounded-full focus:ring-4 focus:ring-gray-300 dark:focus:ring-gray-600"
									aria-expanded="false"
									data-dropdown-toggle="dropdown-user"
								>
									<span className="sr-only">Open user menu</span>
									<img
										className="w-8 h-8 rounded-full"
										src="https://flowbite.com/docs/images/people/profile-picture-5.jpg"
										alt="user photo"
									/>
								</button>
								<div
									className="z-50 hidden my-4 text-base list-none bg-white divide-y divide-gray-100 rounded shadow dark:bg-gray-700 dark:divide-gray-600"
									id="dropdown-user"
								>
									<div className="px-4 py-3" role="none">
										<p
											className="text-sm text-gray-900 dark:text-white"
											role="none"
										>
											{profile.name}
										</p>
										<p
											className="text-sm font-medium text-gray-900 truncate dark:text-gray-300"
											role="none"
										>
											{profile.email}
										</p>
									</div>
									<ul className="py-1" role="none">
										<li>
											<Link
												to={"/home"}
												className="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100 dark:text-gray-300 dark:hover:bg-gray-600 dark:hover:text-white"
												role="menuitem"
											>
												Dashboard
											</Link>
										</li>
										<li className=" hover:bg-gray-100 dark:text-gray-300 dark:hover:bg-gray-600 dark:hover:text-white">
											{/* <a href="#" className="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100 dark:text-gray-300 dark:hover:bg-gray-600 dark:hover:text-white" role="menuitem">Settings</a> */}
						{/* <button
												onClick={handleOpenModalEditUser}
												className="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100 dark:text-gray-300 dark:hover:bg-gray-600 dark:hover:text-white"
												role="menuitem"
											>
												Settings
											</button>
										</li>
										<li>
											<button
												className="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100 dark:text-gray-300 dark:hover:bg-gray-600 dark:hover:text-white"
												role="menuitem"
												onClick={handleSignOut}
											>
												Sign out
											</button>
										</li>
									</ul>
								</div>
							</div>
						</div >  */}
						{isOpenModalEditUser && (
							<EditUser
								openModal={isOpenModalEditUser}
								handleCloseModalEditUser={handleCloseModalEditUser}
							/>
						)}
						{isOpenModalCreateClass && (
							<CreateClass
								openModal={isOpenModalCreateClass}
								handleCloseModalCreateClass={handleCloseModalCreateClass}
							/>
						)}
						{isOpenModalJoinClass && (
							<JoinClass
								openModal={isOpenModalJoinClass}
								handleCloseModalJoinClass={handleCloseModalJoinClass}
							/>
						)}
					</div>
				</div>
			</div>
		</nav>
	);
};

export default NavBarLogin;

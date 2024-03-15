import { type FC, useState, useEffect } from "react";
import CardClass from "../component/card/CardClass";
import { useGetAllClassesQuery } from "../api/store/class/queries";
import useAuth from "../hooks/auth";
import { ClassQuery } from "../api/store/class/interface";
import CreateClass from "../modal/CreateClass";
import JoinClass from "../modal/JoinClass";
import { useLocation } from "react-router-dom";
import { App, Spin } from "antd";

const Dashboard: FC = (): JSX.Element => {
	const location = useLocation();
	const { notification } = App.useApp();
	const { data: user } = useAuth();
	const [isOpenModalCreateClass, setIsOpenModalCreateClass] = useState(false);
	const [isOpenModalJoinClass, setIsOpenModalJoinClass] = useState(false);

	const { data, isLoading } = useGetAllClassesQuery(user?.id);

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

	useEffect(() => {
		if (location.state?.showToast) {
			if (location.state.type == "success") {
				notification.success({
					message: location.state.message,
				});
			} else {
				notification.error({
					message: location.state.message,
				});
			}
		}
	}, []);

	return (
		<div className="">
			<div className=" content-center dark:border-gray-700 mt-14">
				<section className="bg-white dark:bg-gray-900 h-screen content-center p-5">
					{isLoading ? (
						<div>
							<Spin fullscreen />
						</div>
					) : data?.length === 0 ? (
						<div className="py-8 px-4 mx-auto max-w-screen-xl text-center lg:py-16">
							<div className="content-center justify-between">
								<img
									className="w-auto h-96 inline-flex justify-center items-center"
									src="https://www.gstatic.com/classroom/empty_states_home.svg"
									alt="empty states"
								/>
							</div>
							<p className="mb-8 text-lg font-medium text-gray-500 lg:text-xl sm:px-16 lg:px-48 dark:text-gray-400">
								Add a class to get started.
							</p>
							<div className="flex flex-col space-y-4 sm:flex-row sm:justify-center sm:space-y-0">
								{user?.role === "Teacher" ? (
									<button
										onClick={handleOpenModalCreateClass}
										className="inline-flex justify-center items-center py-3 px-5 text-base font-medium text-center text-white rounded-lg bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 dark:focus:ring-blue-900"
									>
										Create class
										<svg
											className="w-3.5 h-3.5 ms-2 rtl:rotate-180"
											aria-hidden="true"
											xmlns="http://www.w3.org/2000/svg"
											fill="none"
											viewBox="0 0 14 10"
										>
											<path
												stroke="currentColor"
												strokeLinecap="round"
												strokeLinejoin="round"
												strokeWidth={2}
												d="M1 5h12m0 0L9 1m4 4L9 9"
											/>
										</svg>
									</button>
								) : (
									<button
										onClick={handleOpenModalJoinClass}
										className="inline-flex justify-center items-center py-3 px-5 sm:ms-4 text-base font-medium text-center text-gray-900 rounded-lg border border-gray-300 hover:bg-gray-100 focus:ring-4 focus:ring-gray-100 dark:text-white dark:border-gray-700 dark:hover:bg-gray-700 dark:focus:ring-gray-800"
									>
										Join class
									</button>
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
					) : (
						<div className="flex flex-wrap gap-16">
							{data?.map(
								(item: ClassQuery) =>
									item.isActivated && (
										<CardClass
											key={item.id}
											id={item.id} // Assuming there's an ID property in your ClassDto
											name={item.name}
											description={item.description}
										/>
									)
							)}
						</div>
					)}
				</section>
			</div>
		</div>
	);
};
export default Dashboard;

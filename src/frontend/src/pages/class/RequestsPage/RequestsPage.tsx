import useClassDetail from "../../../hooks/useClassDetail";
import RequestCard from "./components/RequestCard";
import { Empty, Result, Spin } from "antd";
import UnexpectedError from "../../errors/UnexpectedError";
import useAuthQuery from "../../../api/store/auth/queries";
import { UserRole } from "../../../api/store/auth/interface";

export default function RequestsPage() {
	const { data: myRole } = useAuthQuery((state) => state.role);
	const {
		data: classDetail,
		isLoading,
		isSuccess,
		isError,
		error,
	} = useClassDetail();

	return (
		<div className="w-full p-6">
			{isError && (
				<div className="w-full h-full flex justify-center items-center">
					<UnexpectedError error={error} />
				</div>
			)}
			{isLoading && (
				<div className="w-full h-full flex justify-center items-center">
					<Spin />
				</div>
			)}
			{isSuccess && classDetail.requests.length === 0 && <Empty />}
			{isSuccess && (
				<div className="flex flex-col w-full gap-y-5">
					{classDetail.requests.map((item) => (
						<RequestCard
							key={item.id}
							{...item}
							isTeacherView={myRole === UserRole.Teacher}
						/>
					))}
				</div>
			)}
		</div>
	);
}

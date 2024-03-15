import { type FC, useEffect } from "react";
import { useNavigate, useParams } from "react-router-dom";
import classService from "../services/class.service";
import { AxiosError } from "axios";

const JoinClassCode: FC = () => {
	const navigate = useNavigate();
	const { code } = useParams();

	const fetch = (): void => {
		if (code === undefined) {
			return;
		}

		classService
			.joinCourseByCode(code)
			.then((res) => {
				if (res.status == 200) {
					navigate("/home", {
						state: {
							showToast: true,
							type: "success",
							message: "You successfully join new course",
						},
					});
				}
			})
			.catch(
				(
					err: AxiosError<{
						type: string;
						title: string;
						status: number;
						instance: string;
						errors: {
							field: string;
							detail: string;
						}[];
					}>
				) => {
					navigate("/home", {
						state: {
							showToast: true,
							type: "error",
							message: err.response?.data.title,
						},
					});
				}
			);
	};

	useEffect(() => {
		fetch();
	}, []);

	return <div></div>;
};

export default JoinClassCode;

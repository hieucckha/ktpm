import { type FC, useEffect } from "react";
import { useNavigate, useParams } from "react-router-dom";
import classService from "../services/class.service";
import { AxiosError } from "axios";

const JoinClassEmail: FC = () => {
	const navigate = useNavigate();
	const { token } = useParams();

	const fetch = (): void => {
		if (token === undefined) {
			return;
		}

		classService
			.joinCourseByInvitationLink(token)
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

	return (
		<div></div>
		// <div className="flex flex-col min-h-screen overflow-hidden">
		// 	{/* Page content. */}

		// 	<main className="grow h-screen">
		// 		<section className="bg-gradient-to-b from-gray-100 to-white h-full">
		// 			<div className="max-w-6xl mx-auto px-4 sm:px-6">
		// 				<div className="pt-32 pb-12 md:pt-40 md:pb-20">
		// 					{/* Page header */}
		// 					<div className="max-w-3xl mx-auto text-center pb-12 md:pb-20">
		// 						<h1 className="h1 mb-3">Join new class.</h1>
		// 						<h2 className="h2">{message}</h2>
		// 					</div>
		// 				</div>
		// 			</div>
		// 		</section>
		// 	</main>
		// </div>
	);
};

export default JoinClassEmail;

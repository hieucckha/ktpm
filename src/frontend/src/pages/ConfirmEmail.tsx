import { useState, type FC, useEffect} from "react";
import { useSearchParams } from "react-router-dom";
import AuthService from "../services/auth.service";
import Header from "./LandingPage/Header";

const ConfirmEmail: FC = () => {
	const [searchParams] = useSearchParams();

	const email = searchParams.get("email");
	const confirmToken = searchParams.get("code");

	const [isConfirmed, setIsConfirmed] = useState(false);

	const fetch = (): void => {
		 AuthService.confirmEmail(email!, confirmToken!)
		.then((res) => {
			if (res.status == 200) {
				setIsConfirmed(true);
			}
		}).catch((err) => {
			console.error(err);
		});
	};

	useEffect(() => {
		fetch();
	});

	return (
		<div className="flex flex-col min-h-screen overflow-hidden">
			{/* Page content. */}

			<Header />
			<main className="grow h-screen">
				<section className="bg-gradient-to-b from-gray-100 to-white h-full">
					<div className="max-w-6xl mx-auto px-4 sm:px-6">
						<div className="pt-32 pb-12 md:pt-40 md:pb-20">
							{/* Page header */}
							<div className="max-w-3xl mx-auto text-center pb-12 md:pb-20">
								<h1 className="h1 mb-3">Confirm Email.</h1>
								{isConfirmed ? (
									<p className="h2">
										Your account is activate. Please login again!
									</p>
								) : (
									<p>Not confirm</p>
								)}
							</div>
						</div>
					</div>
				</section>
			</main>
		</div>
	);
};

export default ConfirmEmail;

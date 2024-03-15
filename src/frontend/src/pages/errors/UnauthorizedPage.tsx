import { Button, Result } from "antd";
import { UserRole } from "../../api/store/auth/interface";
import { useNavigate } from "react-router-dom";

interface UnauthorizedPageProps {
	userRole?: UserRole;
}

function UnauthorizedPage({ userRole }: UnauthorizedPageProps) {
	const navigate = useNavigate();
	const isAdmin = userRole === UserRole.Admin;
	return (
		<div className="h-screen-dvh bg-white flex justify-center items-center">
			<Result
				status="403"
				title="403"
				subTitle="Sorry, you are not authorized to access this page."
				extra={
					<Button
						onClick={() =>
							navigate(isAdmin ? "/admin/dashboard" : "/home", {
								replace: true,
							})
						}
					>
						{" "}
						{isAdmin ? "Back to Dashboard" : "Back to Home"}
					</Button>
				}
			/>
		</div>
	);
}

export default UnauthorizedPage;

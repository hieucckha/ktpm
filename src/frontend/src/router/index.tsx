import { createBrowserRouter } from "react-router-dom";

import { App } from "../App";
import LandingPage from "../pages/LandingPage/index";
import AuthLayout from "../layout/auth/AuthLayout";
import SignIn from "../pages/SignIn";
import SignUp from "../pages/SignUp";
import UnauthorizeLayout from "../layout/auth/UnauthorizeLayout";
import AppLayout from "../layout/AppLayout";
import NotFound from "../pages/NotFound";
import ResetPassword from "../pages/ResetPassword";
import ConfirmEmail from "../pages/ConfirmEmail";
import ConfirmResetPassword from "../pages/ConfirmResetPassword";
import Overview from "../pages/class/components/Overview";
import Dashboard from "../layout/Dashboard";
import ClassLayout from "../layout/ClassLayout";
import ClassroomMember from "../pages/class/components/ClassroomMember";
import GradeStructure from "../pages/class/components/GradeStructure";
import Grade from "../pages/class/components/Grade";
import JoinClassEmail from "../pages/JoinClassEmail";
import JoinClassCode from "../pages/JoinClassCode";
import AdminLayout from "../layout/Admin";
import RoleLayout from "../layout/RoleLayout";
import UnauthorizedPage from "../pages/errors/UnauthorizedPage";
import ManagementClass from "../pages/admin/ManagementClass";
import RequestsPage from "../pages/class/RequestsPage/RequestsPage";
import ManagementUser from "../pages/admin/ManagementUser";

const BrowserRouter = createBrowserRouter([
	{
		element: <App />,
		errorElement: <NotFound />,
		children: [
			{
				element: <UnauthorizeLayout authenticatedUrl="/admin/classes" />,
				children: [
					{
						path: "/admin/sign-in",
						element: <SignIn afterLoginUrl={"/admin/classes"} />,
					},
				],
			},

			{
				element: <UnauthorizeLayout />,
				children: [
					{
						path: "/",
						element: <LandingPage />,
					},

					{
						path: "/sign-in",
						element: <SignIn />,
					},
					{
						path: "/sign-up",
						element: <SignUp />,
					},
					{
						path: "/reset-password",
						element: <ResetPassword />,
					},
					{
						path: "/reset-password/confirm",
						element: <ConfirmResetPassword />,
					},
					{
						path: "/activate-account/confirm",
						element: <ConfirmEmail />,
					},
				],
			},
			{
				element: <AuthLayout />,
				children: [
					{
						element: <RoleLayout roles={["Teacher", "Student"]} />,
						children: [
							{
								path: "/course/invite-email/confirm/:token",
								element: <JoinClassEmail />,
							},
							{
								path: "/course/invite/:code",
								element: <JoinClassCode />,
							},
							{
								element: <AppLayout />,
								children: [
									{
										path: "/home",
										element: <Dashboard />,
									},
									{
										path: "/class/:id/*",
										element: <ClassLayout />,
										children: [
											{
												index: true,
												element: <AppLayout />,
											},
											{
												path: "overview",
												element: <Overview />,
											},
											{
												path: "people",
												element: <ClassroomMember />,
											},
											{
												path: "grade-structure",
												element: <GradeStructure />,
											},
											{
												path: "grade",
												element: <Grade />,
											},
											{
												path: "requests",
												element: <RequestsPage />,
											},
										],
									},
								],
							},
						],
					},
					{
						element: <RoleLayout roles={["Admin"]} />,
						children: [
							{
								path: "/admin",
								element: <AdminLayout />,
								children: [
									{
										path: "dashboard",
										element: <div>Admin Dashboard</div>,
									},
									{
										path: "classes",
										element: <ManagementClass />,
									},
									{
										path: "users",
										element: <ManagementUser />,
									},
								],
							},
						],
					},
				],
			},
			{
				path: "/403",
				element: <UnauthorizedPage />,
			},
		],
	},
]);

export default BrowserRouter;

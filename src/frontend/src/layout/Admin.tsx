import type { FC } from "react";

import NavBarLogin from "../partials/NavBarLogin";
import Sidebar from "../partials/Sidebar";
import { Outlet } from "react-router-dom";

/**
 * Admin page.
 */
const AdminLayout: FC = () => {
	return (
		<div className="bg-white">
			<NavBarLogin />
			<div className="bg-white w-full h-screen flex">
				<div className="flex-none sm:w-64 h-full ...">
					<Sidebar />
				</div>
				<div className="grow h-full ...">
					<div className="w-full h-full">
						<div className="content-center dark:border-gray-700 mt-14"></div>
						<Outlet />
					</div>
				</div>
			</div>

		</div>
	);
};

export default AdminLayout;

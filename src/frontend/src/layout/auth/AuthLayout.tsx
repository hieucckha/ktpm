import { Outlet, Navigate, useLocation } from "react-router-dom";

import LocalStorageService from "../../services/localStorage.service";
import type { FC } from "react";

/**
 * Unauthorize layout.
 */
const AuthLayout: FC = (): JSX.Element => {
	const token = LocalStorageService.getItem("auth");
	const location = useLocation();

	return token ? (
		<Outlet />
	) : (
		<Navigate to="/sign-in" replace state={{ from: location }} />
	);
};

export default AuthLayout;

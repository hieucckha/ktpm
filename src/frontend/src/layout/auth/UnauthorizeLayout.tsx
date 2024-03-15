import type { FC } from "react";
import { Outlet, Navigate } from "react-router-dom";

import LocalStorageService from "../../services/localStorage.service";

interface UnauthorizeLayoutProps {
	// Navigate to this url when user logged in
	authenticatedUrl?: string;
}

/**
 * Unauthorize layout.
 */
const UnauthorizeLayout: FC<UnauthorizeLayoutProps> = ({ authenticatedUrl = "/home"}): JSX.Element => {
	// eslint-disable-next-line @typescript-eslint/no-unsafe-assignment
	const token = LocalStorageService.getItem("auth");
	return token ? <Navigate to={authenticatedUrl} /> : <Outlet />;
};
export default UnauthorizeLayout;

import { Outlet, Navigate, useLocation, Link } from "react-router-dom";

import LocalStorageService from "../services/localStorage.service";
import { useEffect, type FC } from "react";
import useAuth from "../hooks/auth";
import NotFound from "../pages/NotFound";
import localStorageService from "../services/localStorage.service";

/**
 * Unauthorize layout.
 */
interface RoleLayoutProps {
	roles: string[];
}

const RoleLayout: FC<RoleLayoutProps> = ({roles} ): JSX.Element => {
	const { data: userData, isLoading } = useAuth();
	
	
	if (isLoading) return null; 

	const userHasRequiredRole = !!(userData && roles.includes(userData.role));
	if (!userHasRequiredRole) {
		return <Navigate to={'/403'} replace />; 
	}

	return (
		<>
			{userData &&  roles.includes(userData?.role) &&(
				<Outlet />
			)}
		</>
	);
};

export default RoleLayout;
